using Motto.Data.Entities;
using Motto.Domain.Models;
using Motto.Domain.Services.Results;

namespace Motto.Domain.Services.Interfaces;

public interface IRentalService
{
    Task<ServiceResult<RentalDeliverResult>> Deliver(int userId, int rentalId, DateTime endDate);
    Task<IEnumerable<Rental>> GetAll();
    Task<Rental?> GetById(int id);
    TotalCostModel GetTotalCost(Rental rental, RentalPlan rentalPlan, DateTime endDate);
    Task<ServiceResult<TotalCostModel>> GetTotalCostById(int id, DateTime endDate);
    Task<ServiceResult<Rental>> Register(Rental rental);
}
