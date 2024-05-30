using Motto.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Motto.Entities;
using Motto.Data;

namespace Motto.Repositories
{
    /// <summary>
    /// Represents a repository for managing delivery drivers.
    /// </summary>
    public class DeliveryDriverUserRepository : IDeliveryDriverUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryDriverUserRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context used by the repository.</param>       
        public DeliveryDriverUserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves a delivery driver by its ID.
        /// </summary>
        /// <param name="id">The ID of the delivery driver.</param>
        /// <returns>The delivery driver with the specified ID, or null if not found.</returns>
        public async Task<DeliveryDriverUser?> GetById(int id)
        {
            return await _dbContext.DeliveryDriverUsers.FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Retrieves all the delivery drivers.
        /// </summary>
        /// <returns>A collection of all the delivery drivers.</returns>
        public async Task<IEnumerable<DeliveryDriverUser>> GetAll()
        {
            return await _dbContext.DeliveryDriverUsers.ToListAsync();
        }

        /// <summary>
        /// Adds a new delivery driver.
        /// </summary>
        /// <param name="driver">The delivery driver to be added.</param>
        public async Task Add(DeliveryDriverUser driver)
        {
            await _dbContext.DeliveryDriverUsers.AddAsync(driver);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing delivery driver.
        /// </summary>
        /// <param name="driver">The delivery driver to be updated.</param>
        public async Task Update(DeliveryDriverUser driver)
        {
            _dbContext.DeliveryDriverUsers.Update(driver);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a delivery driver by its ID.
        /// </summary>
        /// <param name="id">The ID of the delivery driver to be deleted.</param>
        public async Task Remove(int id)
        {
            var driver = await _dbContext.DeliveryDriverUsers.FindAsync(id);
            if (driver != null)
            {
                _dbContext.DeliveryDriverUsers.Remove(driver);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
