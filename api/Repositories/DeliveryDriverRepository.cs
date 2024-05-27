using System.Collections.Generic;
using System.Threading.Tasks;
using Motto.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Motto.Entities;
using Motto.Models;

namespace Motto.Repositories
{
    /// <summary>
    /// Represents a repository for managing delivery drivers.
    /// </summary>
    public class DeliveryDriverRepository : IDeliveryDriverRepository
    {
        private readonly ApplicationDbContext _dbContext;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryDriverRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context used by the repository.</param>       
        public DeliveryDriverRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves a delivery driver by its ID.
        /// </summary>
        /// <param name="id">The ID of the delivery driver.</param>
        /// <returns>The delivery driver with the specified ID, or null if not found.</returns>
        public async Task<DeliveryDriver?> GetById(int id)
        {
            return await _dbContext.DeliveryDrivers.FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Retrieves all the delivery drivers.
        /// </summary>
        /// <returns>A collection of all the delivery drivers.</returns>
        public async Task<IEnumerable<DeliveryDriver>> GetAll()
        {
            return await _dbContext.DeliveryDrivers.ToListAsync();
        }

        /// <summary>
        /// Adds a new delivery driver.
        /// </summary>
        /// <param name="driver">The delivery driver to be added.</param>
        public async Task Add(DeliveryDriver driver)
        {
            await _dbContext.DeliveryDrivers.AddAsync(driver);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing delivery driver.
        /// </summary>
        /// <param name="driver">The delivery driver to be updated.</param>
        public async Task Update(DeliveryDriver driver)
        {
            _dbContext.DeliveryDrivers.Update(driver);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a delivery driver by its ID.
        /// </summary>
        /// <param name="id">The ID of the delivery driver to be deleted.</param>
        public async Task Remove(int id)
        {
            var driver = await _dbContext.DeliveryDrivers.FindAsync(id);
            if (driver != null)
            {
                _dbContext.DeliveryDrivers.Remove(driver);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
