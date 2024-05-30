using Motto.Domain.Models;
using Motto.Domain.Services.Results;
using Motto.Entities;
using Motto.Services.Results;

namespace Motto.Services.Interfaces;

public interface IRentalService
{
    Task<ServiceResult<RentalDeliverResult>> Deliver(int userId, int rentalId, DateTime endDate);
    Task<IEnumerable<Rental>> GetAll();
    Task<Rental?> GetById(int id);
    TotalCostModel GetTotalCost(Rental rental, RentalPlan rentalPlan, DateTime endDate);
    Task<ServiceResult<TotalCostModel>> GetTotalCostById(int id, DateTime endDate);
    Task<ServiceResult<Rental>> Register(Rental rental);
}
