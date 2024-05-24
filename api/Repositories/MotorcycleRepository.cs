using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Motto.Entities;
using Motto.Models;

namespace Motto.Repositories
{
    public interface IMotorcycleRepository
    {
        Task<Motorcycle?> GetMotorcycleById(int id);
        Task<Motorcycle?> GetMotorcycleByPlate(string plate);
        Task<Motorcycle?> GetMotorcycleByPlateAndDifferentId(string plate, int id);
        Task<IEnumerable<Motorcycle>> GetMotorcycles(string? plateFilter);
        Task<bool> HasRentals(int motorcycleId);
        Task AddMotorcycle(Motorcycle motorcycle);
        Task UpdateMotorcycle(int id);
        Task RemoveMotorcycle(int id);
        Task SaveChangesAsync();
    }

    public class MotorcycleRepository : IMotorcycleRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public MotorcycleRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Motorcycle?> GetMotorcycleById(int id)
        {
            return await _dbContext.Motorcycles.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Motorcycle?> GetMotorcycleByPlate(string plate)
        {
            return await _dbContext.Motorcycles.FirstOrDefaultAsync(m => m.Plate == plate);
        }

        public async Task<Motorcycle?> GetMotorcycleByPlateAndDifferentId(string plate, int id)
        {
            return await _dbContext.Motorcycles.FirstOrDefaultAsync(m => m.Plate == plate && m.Id != id);
        }

        public async Task<IEnumerable<Motorcycle>> GetMotorcycles(string? plateFilter)
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

        public async Task AddMotorcycle(Motorcycle motorcycle)
        {
            await _dbContext.Motorcycles.AddAsync(motorcycle);
        }

        public async Task UpdateMotorcycle(int id)
        {
            var motorcycle = await _dbContext.Motorcycles.FindAsync(id);
            if (motorcycle != null)
            {
                _dbContext.Motorcycles.Update(motorcycle);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task RemoveMotorcycle(int id)
        {
            var motorcycle = await _dbContext.Motorcycles.FindAsync(id);
            if (motorcycle != null)
            {
                _dbContext.Motorcycles.Remove(motorcycle);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
