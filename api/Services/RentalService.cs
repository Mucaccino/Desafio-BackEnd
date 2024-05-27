using Microsoft.EntityFrameworkCore;
using Motto.DTOs;
using Motto.Models;
using Namotion.Reflection;
using Motto.Repositories.Interfaces;
using Motto.Services.Interfaces;

namespace Motto.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IDeliveryDriverRepository _deliveryDriverRepository;
        private readonly IRentalPlanRepository _rentalPlanRepository;

        public RentalService(
            IRentalRepository rentalRepository,
            IDeliveryDriverRepository deliveryDriverRepository,
            IRentalPlanRepository rentalPlanRepository)
        {
            _rentalRepository = rentalRepository;
            _deliveryDriverRepository = deliveryDriverRepository;
            _rentalPlanRepository = rentalPlanRepository;
        }

        public async Task<ServiceResult<Rental>> RegisterRental(int userId, CreateRentalRequest registerModel)
        {
            var deliveryDriver = await _deliveryDriverRepository.GetById(userId);
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

        public async Task<ServiceResult<RentalDeliveryResponse>> DeliverMotorcycle(int userId, int rentalId, DateTime endDate)
        {
            var rental = await _rentalRepository.GetById(rentalId);
            if (rental == null)
            {
                return ServiceResult<RentalDeliveryResponse>.Failed("Aluguel não encontrado.");
            }

            if (rental.DeliveryDriverId != userId)
            {
                return ServiceResult<RentalDeliveryResponse>.Failed("Você não está autorizado a entregar esta moto.");
            }

            if (!rental.EndDate.HasValidNullability())
            {
                return ServiceResult<RentalDeliveryResponse>.Failed("A moto já foi entregue.");
            }

            if (endDate < rental.StartDate)
            {
                return ServiceResult<RentalDeliveryResponse>.Failed("A data de entrega não pode ser anterior à data de retirada.");
            }

            var rentalPlan = await _rentalPlanRepository.GetById(rental.RentalPlanId);
            if (rentalPlan == null)
            {
                return ServiceResult<RentalDeliveryResponse>.Failed("Plano de aluguel inválido.");
            }

            rental.EndDate = endDate;
            var totalCostInfo = GetTotalCost(rental, rentalPlan, endDate);

            try
            {
                await _rentalRepository.SaveChanges();
                return ServiceResult<RentalDeliveryResponse>.Successed(new RentalDeliveryResponse
                {
                    Cost = totalCostInfo,
                    Message = "Entrega registrada com sucesso."
                });
            }
            catch (DbUpdateException ex)
            {
                return ServiceResult<RentalDeliveryResponse>.Failed("Erro ao salvar os dados no banco de dados: " + ex.Message);
            }
        }

        public async Task<IEnumerable<Rental>> GetAll()
        {
            return await _rentalRepository.GetAll();
        }

        public async Task<Rental?> GetById(int id)
        {
            return await _rentalRepository.GetById(id);
        }

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

        public TotalCostModel GetTotalCost(Rental rental, RentalPlan rentalPlan, DateTime endDate)
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

