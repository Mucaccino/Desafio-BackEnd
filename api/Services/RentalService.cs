using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Motto.Controllers;
using Motto.Entities;
using Motto.Models;
using Motto.Repositories;
using Namotion.Reflection;

namespace Motto.Services
{
    public interface IRentalService
    {
        Task<ServiceResult<Rental>> RegisterRental(int userId, RentalRegisterModel registerModel);
        Task<ServiceResult<RentalDeliveryResult>> DeliverMotorcycle(int userId, int rentalId, DateTime endDate);
        Task<IEnumerable<Rental>> GetAllRentals();
        Task<Rental> GetRentalById(int id);
        Task<ServiceResult<RentalTotalCostModel>> GetTotalCostById(int id, DateTime endDate);
        RentalTotalCostModel GetTotalCost(Rental rental, RentalPlan rentalPlan, DateTime endDate);
    }

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

        public async Task<ServiceResult<Rental>> RegisterRental(int userId, RentalRegisterModel registerModel)
        {
            var deliveryDriver = await _deliveryDriverRepository.GetByIdAsync(userId);
            if (deliveryDriver == null)
            {
                return ServiceResult<Rental>.Failed("O entregador não existe.");
            }

            if (!deliveryDriver.DriverLicenseType.Contains("A"))
            {
                return ServiceResult<Rental>.Failed("O entregador não está habilitado na categoria A.");
            }

            var rentalPlan = await _rentalPlanRepository.GetByIdAsync(registerModel.RentalPlanId);
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
                await _rentalRepository.AddRentalAsync(rental);
                return ServiceResult<Rental>.Successed(rental);
            }
            catch (DbUpdateException ex)
            {
                return ServiceResult<Rental>.Failed("Erro ao salvar os dados no banco de dados: " + ex.Message);
            }
        }

        public async Task<ServiceResult<RentalDeliveryResult>> DeliverMotorcycle(int userId, int rentalId, DateTime endDate)
        {
            var rental = await _rentalRepository.GetRentalByIdAsync(rentalId);
            if (rental == null)
            {
                return ServiceResult<RentalDeliveryResult>.Failed("Aluguel não encontrado.");
            }

            if (rental.DeliveryDriverId != userId)
            {
                return ServiceResult<RentalDeliveryResult>.Failed("Você não está autorizado a entregar esta moto.");
            }

            if (!rental.EndDate.HasValidNullability())
            {
                return ServiceResult<RentalDeliveryResult>.Failed("A moto já foi entregue.");
            }

            if (endDate < rental.StartDate)
            {
                return ServiceResult<RentalDeliveryResult>.Failed("A data de entrega não pode ser anterior à data de retirada.");
            }

            var rentalPlan = await _rentalPlanRepository.GetByIdAsync(rental.RentalPlanId);
            if (rentalPlan == null)
            {
                return ServiceResult<RentalDeliveryResult>.Failed("Plano de aluguel inválido.");
            }

            rental.EndDate = endDate;
            var totalCostInfo = GetTotalCost(rental, rentalPlan, endDate);

            try
            {
                await _rentalRepository.SaveChangesAsync();
                return ServiceResult<RentalDeliveryResult>.Successed(new RentalDeliveryResult
                {
                    Cost = totalCostInfo,
                    Message = "Entrega registrada com sucesso."
                });
            }
            catch (DbUpdateException ex)
            {
                return ServiceResult<RentalDeliveryResult>.Failed("Erro ao salvar os dados no banco de dados: " + ex.Message);
            }
        }

        public async Task<IEnumerable<Rental>> GetAllRentals()
        {
            return await _rentalRepository.GetAllRentalsAsync();
        }

        public async Task<Rental> GetRentalById(int id)
        {
            return await _rentalRepository.GetRentalByIdAsync(id);
        }

        public async Task<ServiceResult<RentalTotalCostModel>> GetTotalCostById(int id, DateTime endDate)
        {
            var rental = await _rentalRepository.GetRentalByIdAsync(id);
            if (rental == null)
            {
                return ServiceResult<RentalTotalCostModel>.Failed("Aluguel não encontrado.");
            }

            if (endDate < rental.StartDate)
            {
                return ServiceResult<RentalTotalCostModel>.Failed("A data de entrega não pode ser anterior à data de retirada.");
            }

            var totalCostInfo = GetTotalCost(rental, rental.RentalPlan, endDate);

            return ServiceResult<RentalTotalCostModel>.Successed(totalCostInfo);
        }

        public RentalTotalCostModel GetTotalCost(Rental rental, RentalPlan rentalPlan, DateTime endDate)
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

            return new RentalTotalCostModel
            {
                BaseCost = rental.TotalCost - rental.PenaltyCost,
                PenaltyCost = rental.PenaltyCost,
                TotalCost = rental.TotalCost
            };
        }

    }
}

