using Motto.DTOs;
using Motto.Models;

namespace Motto.Services.Interfaces;

public interface IRentalService
{
    Task<ServiceResult<RentalDeliveryResponse>> DeliverMotorcycle(int userId, int rentalId, DateTime endDate);
    Task<IEnumerable<Rental>> GetAll();
    Task<Rental?> GetById(int id);
    TotalCostModel GetTotalCost(Rental rental, RentalPlan rentalPlan, DateTime endDate);
    Task<ServiceResult<TotalCostModel>> GetTotalCostById(int id, DateTime endDate);
    Task<ServiceResult<Rental>> RegisterRental(int userId, CreateRentalRequest registerModel);
}
