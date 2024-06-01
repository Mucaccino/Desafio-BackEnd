using Motto.Data.Entities;

namespace Motto.Domain.Services.Interfaces;

public interface IRentalPlanService
{
    Task<IEnumerable<RentalPlan>> GetAll();
}
