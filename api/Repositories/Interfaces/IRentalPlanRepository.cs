using Motto.Models;

namespace Motto.Repositories.Interfaces
{
    public interface IRentalPlanRepository
    {
        Task<IEnumerable<RentalPlan>> GetAll();
        Task<RentalPlan?> GetById(int id);
    }
}