using System.Collections.Generic;
using System.Threading.Tasks;
using Motto.Repositories.Interfaces;
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

        public async Task<DeliveryDriver?> GetById(int id)
        {
            return await _dbContext.DeliveryDrivers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<DeliveryDriver>> GetAll()
        {
            return await _dbContext.DeliveryDrivers.ToListAsync();
        }

        public async Task Add(DeliveryDriver driver)
        {
            await _dbContext.DeliveryDrivers.AddAsync(driver);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(DeliveryDriver driver)
        {
            _dbContext.DeliveryDrivers.Update(driver);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
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
