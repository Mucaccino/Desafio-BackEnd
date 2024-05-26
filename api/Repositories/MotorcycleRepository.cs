using Motto.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Motto.Entities;
using Motto.Models;

namespace Motto.Repositories
{
    public class MotorcycleRepository : IMotorcycleRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public MotorcycleRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Motorcycle?> GetById(int id)
        {
            return await _dbContext.Motorcycles.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Motorcycle?> GetByPlate(string plate)
        {
            return await _dbContext.Motorcycles.FirstOrDefaultAsync(m => m.Plate == plate);
        }

        public async Task<bool> HasPlate(string plate, int id)
        {
            var motorcycle = await _dbContext.Motorcycles.FirstOrDefaultAsync(m => m.Plate == plate && m.Id != id);

            return motorcycle != null;
        }

        public async Task<IEnumerable<Motorcycle>> GetAll(string? plateFilter)
        {
            IQueryable<Motorcycle> query = _dbContext.Motorcycles;

            if (!string.IsNullOrEmpty(plateFilter))
            {
                query = query.Where(m => m.Plate.Contains(plateFilter));
            }

            return await query.ToListAsync();
        }

        public async Task<bool> HasRentals(int motorcycleId)
        {
            return await _dbContext.Rentals.AnyAsync(r => r.MotorcycleId == motorcycleId);
        }

        public async Task Add(Motorcycle motorcycle)
        {
            await _dbContext.Motorcycles.AddAsync(motorcycle);
        }

        public async Task Update(int id)
        {
            var motorcycle = await _dbContext.Motorcycles.FindAsync(id);
            if (motorcycle != null)
            {
                _dbContext.Motorcycles.Update(motorcycle);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task Remove(int id)
        {
            var motorcycle = await _dbContext.Motorcycles.FindAsync(id);
            if (motorcycle != null)
            {
                _dbContext.Motorcycles.Remove(motorcycle);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
