using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Motto.Entities;
using Motto.Models;

namespace Motto.Repositories
{
    public class DeliveryDriverRepository : IDeliveryDriverRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DeliveryDriverRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DeliveryDriver> GetByIdAsync(int id)
        {
            return await _dbContext.DeliveryDrivers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<DeliveryDriver>> GetAllAsync()
        {
            return await _dbContext.DeliveryDrivers.ToListAsync();
        }

        public async Task AddAsync(DeliveryDriver driver)
        {
            await _dbContext.DeliveryDrivers.AddAsync(driver);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(DeliveryDriver driver)
        {
            _dbContext.DeliveryDrivers.Update(driver);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
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
