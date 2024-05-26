using Microsoft.EntityFrameworkCore;
using Motto.Entities;
using Motto.Models;
using Motto.Repositories.Interfaces;

namespace Motto.Repositories
{

    public class RentalRepository : IRentalRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RentalRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Rental?> GetById(int id)
        {
            return await _dbContext.Rentals
                .Include(r => r.Motorcycle)
                .Include(r => r.DeliveryDriver)
                .Include(r => r.RentalPlan)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Rental>> GetAll()
        {
            return await _dbContext.Rentals.ToListAsync();
        }

        public async Task Add(Rental rental)
        {
            await _dbContext.Rentals.AddAsync(rental);
        }

        public async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
