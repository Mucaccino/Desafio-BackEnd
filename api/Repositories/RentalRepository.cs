using Microsoft.EntityFrameworkCore;
using Motto.Entities;
using Motto.Models;

namespace Motto.Repositories
{

    public interface IRentalRepository
    {
        Task<Rental?> GetRentalByIdAsync(int id);
        Task<IEnumerable<Rental>> GetAllRentalsAsync();
        Task AddRentalAsync(Rental rental);
        Task SaveChangesAsync();
    }

    public class RentalRepository : IRentalRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RentalRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Rental?> GetRentalByIdAsync(int id)
        {
            return await _dbContext.Rentals
                .Include(r => r.Motorcycle)
                .Include(r => r.DeliveryDriver)
                .Include(r => r.RentalPlan)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Rental>> GetAllRentalsAsync()
        {
            return await _dbContext.Rentals.ToListAsync();
        }

        public async Task AddRentalAsync(Rental rental)
        {
            await _dbContext.Rentals.AddAsync(rental);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
