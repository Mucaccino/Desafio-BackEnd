using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Motto.Entities;
using Motto.Models;
using Motto.Repositories.Interfaces;

namespace Motto.Repositories
{
    /// <summary>
    /// Represents a repository for managing rental plans.
    /// </summary>
    public class RentalPlanRepository : IRentalPlanRepository
    {
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalPlanRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The application database context.</param>
        public RentalPlanRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves a rental plan by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the rental plan.</param>
        /// <returns>The rental plan with the specified ID, or null if it doesn't exist.</returns>
        public async Task<RentalPlan?> GetById(int id)
        {
            return await _dbContext.RentalPlans.FirstOrDefaultAsync(rp => rp.Id == id);
        }

        /// <summary>
        /// Retrieves all rental plans asynchronously.
        /// </summary>
        /// <returns>A collection of rental plans.</returns>
        public async Task<IEnumerable<RentalPlan>> GetAll()
        {
            return await _dbContext.RentalPlans.ToListAsync();
        }

    }
}
