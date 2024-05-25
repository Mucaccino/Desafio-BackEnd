using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Motto.Entities;
using Motto.Models;
using Motto.Repositories.Interfaces;

namespace Motto.Repositories
{
    public class RentalPlanRepository : IRentalPlanRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RentalPlanRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RentalPlan?> GetById(int id)
        {
            return await _dbContext.RentalPlans.FirstOrDefaultAsync(rp => rp.Id == id);
        }

        public async Task<IEnumerable<RentalPlan>> GetAll()
        {
            return await _dbContext.RentalPlans.ToListAsync();
        }

    }
}
