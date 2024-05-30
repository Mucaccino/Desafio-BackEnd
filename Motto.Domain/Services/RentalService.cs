using Microsoft.EntityFrameworkCore;
using Motto.Entities;
using Motto.Repositories.Interfaces;
using Motto.Services.Interfaces;
using Motto.Services.Results;
using Motto.Domain.Models;
using Motto.Domain.Services.Results;

namespace Motto.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IDeliveryDriverUserRepository _deliveryDriverUserRepository;
        private readonly IRentalPlanRepository _rentalPlanRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalService"/> class.
        /// </summary>
        /// <param name="rentalRepository">The rental repository.</param>
        /// <param name="deliveryDriverUserRepository">The delivery driver repository.</param>
        /// <param name="rentalPlanRepository">The rental plan repository.</param>        
        public RentalService(
            IRentalRepository rentalRepository,
            IDeliveryDriverUserRepository deliveryDriverUserRepository,
            IRentalPlanRepository rentalPlanRepository)
        {
            _rentalRepository = rentalRepository;
            _deliveryDriverUserRepository = deliveryDriverUserRepository;
            _rentalPlanRepository = rentalPlanRepository;
        }

        /// <summary>
        /// Registers a rental for a given user and rental request.
        /// </summary>
        /// <param name="rental">The rental model <see cref="Rental"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a ServiceResult object that 
        /// contains the registered rental if successful, or an error message if the registration fails.</returns>        
        public async Task<ServiceResult<Rental>> Register(Rental rental)
        {
            var deliveryDriver = rental.DeliveryDriver ?? await _deliveryDriverUserRepository.GetById(rental.DeliveryDriverId);
            if (deliveryDriver == null)
            {
                return ServiceResult<Rental>.Failed("O entregador não existe.");
            }

            if (!deliveryDriver.DriverLicenseType.Contains("A"))
            {
                return ServiceResult<Rental>.Failed("O entregador não está habilitado na categoria A.");
            }

            var rentalPlan = await _rentalPlanRepository.GetById(rental.RentalPlanId);
            if (rentalPlan == null)
            {
                return ServiceResult<Rental>.Failed("Plano de aluguel inválido.");
            }

            rental.StartDate = DateTime.Today.AddDays(1);
            rental.ExpectedEndDate = DateTime.Today.AddDays(rentalPlan.Days);

            try
            {
                await _rentalRepository.Add(rental);
                return ServiceResult<Rental>.Successed(rental);
            }
            catch (DbUpdateException ex)
            {
                return ServiceResult<Rental>.Failed("Erro ao salvar os dados no banco de dados: " + ex.Message);
            }
        }

        /// <summary>
        /// Delivers a motorcycle for a given user and rental.
        /// </summary>
        /// <param name="deliveryDriverId">The ID of the user.</param>
        /// <param name="rentalId">The ID of the rental.</param>
        /// <param name="endDate">The end date of the rental.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a ServiceResult object that 
        /// contains the delivery response if successful, or an error message if the delivery fails.</returns>
        public async Task<ServiceResult<RentalDeliverResult>> Deliver(int deliveryDriverId, int rentalId, DateTime endDate = default(DateTime))
        {
            var rental = await _rentalRepository.GetById(rentalId);
            if (rental == null)
            {
                return ServiceResult<RentalDeliverResult>.Failed("Aluguel não encontrado.");
            }

            if (rental.DeliveryDriverId != deliveryDriverId)
            {
                return ServiceResult<RentalDeliverResult>.Failed("Você não está autorizado a entregar esta moto.");
            }

            if (rental.EndDate != default(DateTime))
            {
                return ServiceResult<RentalDeliverResult>.Failed("A moto já foi entregue.");
            }

            if (endDate < rental.StartDate)
            {
                return ServiceResult<RentalDeliverResult>.Failed("A data de entrega não pode ser anterior à data de retirada.");
            }

            var rentalPlan = await _rentalPlanRepository.GetById(rental.RentalPlanId);
            if (rentalPlan == null)
            {
                return ServiceResult<RentalDeliverResult>.Failed("Plano de aluguel inválido.");
            }

            rental.EndDate = (endDate == default(DateTime)) ? DateTime.Today : endDate;

            var totalCostInfo = GetTotalCost(rental, rentalPlan, endDate);

            try
            {
                await _rentalRepository.Update(rental);
                return ServiceResult<RentalDeliverResult>.Successed(new RentalDeliverResult
                {
                    Cost = totalCostInfo,
                    Message = "Entrega registrada com sucesso."
                });
            }
            catch (DbUpdateException ex)
            {
                return ServiceResult<RentalDeliverResult>.Failed("Erro ao salvar os dados no banco de dados: " + ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all rentals from the repository asynchronously.
        /// </summary>
        /// <returns>An enumerable collection of Rental objects.</returns>
        public async Task<IEnumerable<Rental>> GetAll()
        {
            return await _rentalRepository.GetAll();
        }

        /// <summary>
        /// Retrieves a rental by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the rental.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the rental if found, or null if not found.</returns>
        public async Task<Rental?> GetById(int id)
        {
            return await _rentalRepository.GetById(id);
        }

        /// <summary>
        /// Retrieves the total cost of a rental by its ID and end date.
        /// </summary>
        /// <param name="id">The ID of the rental.</param>
        /// <param name="endDate">The end date of the rental.</param>
        /// <returns>An asynchronous task that returns a ServiceResult containing the total cost as a TotalCostModel if successful, or a failed ServiceResult with an error message if the rental is not found or the end date is earlier than the start date.</returns>
        public async Task<ServiceResult<TotalCostModel>> GetTotalCostById(int id, DateTime endDate)
        {
            var rental = await _rentalRepository.GetById(id);
            if (rental == null)
            {
                return ServiceResult<TotalCostModel>.Failed("Aluguel não encontrado.");
            }

            if (endDate < rental.StartDate)
            {
                return ServiceResult<TotalCostModel>.Failed("A data de entrega não pode ser anterior à data de retirada.");
            }

            var totalCostInfo = GetTotalCost(rental, rental.RentalPlan, endDate);

            return ServiceResult<TotalCostModel>.Successed(totalCostInfo);
        }
        
        /// <summary>
        /// Calculates the total cost of a rental based on the provided rental, rental plan, and end date.
        /// </summary>
        /// <param name="rental">The rental object.</param>
        /// <param name="rentalPlan">The rental plan object.</param>
        /// <param name="endDate">The end date of the rental. If not provided, the current date is used.</param>
        /// <returns>A TotalCostModel object containing the base cost, penalty cost, and total cost of the rental.</returns>
        public TotalCostModel GetTotalCost(Rental rental, RentalPlan rentalPlan, DateTime endDate = default(DateTime))
        {
            if (endDate == default(DateTime)) endDate = DateTime.Today;

            if (endDate < rental.ExpectedEndDate)
            {
                int daysLate = (int)(rental.ExpectedEndDate - endDate).TotalDays;

                decimal lateFeePercentage = 0.0m;
                if (rentalPlan.Days == 7)
                {
                    lateFeePercentage = 0.2m;
                }
                else if (rentalPlan.Days == 15)
                {
                    lateFeePercentage = 0.4m;
                }

                decimal lateFee = rentalPlan.DailyCost * daysLate * lateFeePercentage;
                rental.PenaltyCost = lateFee;

                rental.TotalCost += lateFee;
            }
            else if (endDate > rental.ExpectedEndDate)
            {
                int additionalDays = (int)(endDate - rental.ExpectedEndDate).TotalDays;
                rental.PenaltyCost = 50 * additionalDays;

                rental.TotalCost += rental.PenaltyCost;
            }

            return new TotalCostModel
            {
                BaseCost = rental.TotalCost - rental.PenaltyCost,
                PenaltyCost = rental.PenaltyCost,
                TotalCost = rental.TotalCost
            };
        }

    }
}

