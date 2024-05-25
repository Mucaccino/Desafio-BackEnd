using Motto.Models;

namespace Motto.Services.Interfaces;

public interface IRentalPlanService
{
    Task<IEnumerable<RentalPlan>> GetAll();
}
