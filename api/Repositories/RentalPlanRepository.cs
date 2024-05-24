using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Motto.Entities;
using Motto.Models;

namespace Motto.Repositories
{
    public interface IRentalPlanRepository
    {
        Task<IEnumerable<RentalPlan>> GetAllRentalPlans();
        Task<RentalPlan> GetByIdAsync(int id);
    }

    public class RentalPlanRepository : IRentalPlanRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RentalPlanRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RentalPlan> GetByIdAsync(int id)
        {
            return await _dbContext.RentalPlans.FirstOrDefaultAsync(rp => rp.Id == id);
        }

        public async Task<IEnumerable<RentalPlan>> GetAllRentalPlans()
        {
            return await _dbContext.RentalPlans.ToListAsync();
        }
    }
}
