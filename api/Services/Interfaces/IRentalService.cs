using Motto.DTOs;
using Motto.Models;

namespace Motto.Services.Interfaces;

public interface IRentalService
{
    Task<ServiceResult<RentalDeliverResponse>> DeliverMotorcycle(int userId, int rentalId, DateTime endDate);
    Task<IEnumerable<Rental>> GetAll();
    Task<Rental?> GetById(int id);
    TotalCostResponse GetTotalCost(Rental rental, RentalPlan rentalPlan, DateTime endDate);
    Task<ServiceResult<TotalCostResponse>> GetTotalCostById(int id, DateTime endDate);
    Task<ServiceResult<Rental>> RegisterRental(int userId, RentalCreateRequest registerModel);
}
