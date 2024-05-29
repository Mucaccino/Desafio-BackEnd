using Microsoft.EntityFrameworkCore;
using Motto.DTOs;
using Motto.Entities;
using Namotion.Reflection;
using Motto.Repositories.Interfaces;
using Motto.Services.Interfaces;

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
        /// <param name="userId">The ID of the user.</param>
        /// <param name="registerModel">The rental request.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a ServiceResult object that 
        /// contains the registered rental if successful, or an error message if the registration fails.</returns>        
        public async Task<ServiceResult<Rental>> RegisterRental(int userId, RentalCreateRequest registerModel)
        {
            var deliveryDriver = await _deliveryDriverUserRepository.GetById(userId);
            if (deliveryDriver == null)
            {
                return ServiceResult<Rental>.Failed("O entregador não existe.");
            }

            if (!deliveryDriver.DriverLicenseType.Contains("A"))
            {
                return ServiceResult<Rental>.Failed("O entregador não está habilitado na categoria A.");
            }

            var rentalPlan = await _rentalPlanRepository.GetById(registerModel.RentalPlanId);
            if (rentalPlan == null)
            {
                return ServiceResult<Rental>.Failed("Plano de aluguel inválido.");
            }

            var rental = new Rental
            {
                DeliveryDriverId = deliveryDriver.Id,
                MotorcycleId = registerModel.MotorcycleId,
                RentalPlanId = registerModel.RentalPlanId,
                StartDate = DateTime.Today.AddDays(1),
                ExpectedEndDate = DateTime.Today.AddDays(rentalPlan.Days)
            };

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
        /// <param name="userId">The ID of the user.</param>
        /// <param name="rentalId">The ID of the rental.</param>
        /// <param name="endDate">The end date of the rental.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a ServiceResult object that 
        /// contains the delivery response if successful, or an error message if the delivery fails.</returns>
        public async Task<ServiceResult<RentalDeliverResponse>> DeliverMotorcycle(int userId, int rentalId, DateTime endDate)
        {
            var rental = await _rentalRepository.GetById(rentalId);
            if (rental == null)
            {
                return ServiceResult<RentalDeliverResponse>.Failed("Aluguel não encontrado.");
            }

            if (rental.DeliveryDriverId != userId)
            {
                return ServiceResult<RentalDeliverResponse>.Failed("Você não está autorizado a entregar esta moto.");
            }

            if (!rental.EndDate.HasValidNullability())
            {
                return ServiceResult<RentalDeliverResponse>.Failed("A moto já foi entregue.");
            }

            if (endDate < rental.StartDate)
            {
                return ServiceResult<RentalDeliverResponse>.Failed("A data de entrega não pode ser anterior à data de retirada.");
            }

            var rentalPlan = await _rentalPlanRepository.GetById(rental.RentalPlanId);
            if (rentalPlan == null)
            {
                return ServiceResult<RentalDeliverResponse>.Failed("Plano de aluguel inválido.");
            }

            rental.EndDate = endDate;
            var totalCostInfo = GetTotalCost(rental, rentalPlan, endDate);

            try
            {
                await _rentalRepository.Update(rental);
                return ServiceResult<RentalDeliverResponse>.Successed(new RentalDeliverResponse
                {
                    Cost = totalCostInfo,
                    Message = "Entrega registrada com sucesso."
                });
            }
            catch (DbUpdateException ex)
            {
                return ServiceResult<RentalDeliverResponse>.Failed("Erro ao salvar os dados no banco de dados: " + ex.Message);
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
        public async Task<ServiceResult<TotalCostResponse>> GetTotalCostById(int id, DateTime endDate)
        {
            var rental = await _rentalRepository.GetById(id);
            if (rental == null)
            {
                return ServiceResult<TotalCostResponse>.Failed("Aluguel não encontrado.");
            }

            if (endDate < rental.StartDate)
            {
                return ServiceResult<TotalCostResponse>.Failed("A data de entrega não pode ser anterior à data de retirada.");
            }

            var totalCostInfo = GetTotalCost(rental, rental.RentalPlan, endDate);

            return ServiceResult<TotalCostResponse>.Successed(totalCostInfo);
        }
        
        /// <summary>
        /// Calculates the total cost of a rental based on the provided rental, rental plan, and end date.
        /// </summary>
        /// <param name="rental">The rental object.</param>
        /// <param name="rentalPlan">The rental plan object.</param>
        /// <param name="endDate">The end date of the rental. If not provided, the current date is used.</param>
        /// <returns>A TotalCostModel object containing the base cost, penalty cost, and total cost of the rental.</returns>
        public TotalCostResponse GetTotalCost(Rental rental, RentalPlan rentalPlan, DateTime endDate)
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

            return new TotalCostResponse
            {
                BaseCost = rental.TotalCost - rental.PenaltyCost,
                PenaltyCost = rental.PenaltyCost,
                TotalCost = rental.TotalCost
            };
        }

    }
}

