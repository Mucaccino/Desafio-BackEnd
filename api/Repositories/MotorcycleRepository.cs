using Motto.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Motto.Entities;
using Motto.Models;

namespace Motto.Repositories
{
    /// <summary>
    /// Represents a repository for managing motorcycles.
    /// </summary>
    public class MotorcycleRepository : IMotorcycleRepository
    {
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="MotorcycleRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The application database context.</param>
        public MotorcycleRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves a motorcycle by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the motorcycle.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the motorcycle if found, or null if not found.</returns>
        public async Task<Motorcycle?> GetById(int id)
        {
            return await _dbContext.Motorcycles.FirstOrDefaultAsync(m => m.Id == id);
        }

        /// <summary>
        /// Asynchronously retrieves a motorcycle by its plate.
        /// </summary>
        /// <param name="plate">The plate of the motorcycle to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the motorcycle with the specified plate, or null if no motorcycle is found.</returns>
        public async Task<Motorcycle?> GetByPlate(string plate)
        {
            return await _dbContext.Motorcycles.FirstOrDefaultAsync(m => m.Plate == plate);
        }

        /// <summary>
        /// Checks if a plate is already in use by another motorcycle.
        /// </summary>
        /// <param name="plate">The plate to check.</param>
        /// <param name="id">The ID of the motorcycle to exclude from the check.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the plate is already in use.</returns>
        public async Task<bool> HasPlate(string plate, int id)
        {
            var motorcycle = await _dbContext.Motorcycles.FirstOrDefaultAsync(m => m.Plate == plate && m.Id != id);

            return motorcycle != null;
        }

        /// <summary>
        /// Asynchronously retrieves all motorcycles from the database, optionally filtered by plate.
        /// </summary>
        /// <param name="plateFilter">An optional filter to apply to the motorcycles' plates.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable of Motorcycle objects.</returns>
        public async Task<IEnumerable<Motorcycle>> GetAll(string? plateFilter)
        {
            IQueryable<Motorcycle> query = _dbContext.Motorcycles;

            if (!string.IsNullOrEmpty(plateFilter))
            {
                query = query.Where(m => m.Plate.Contains(plateFilter));
            }

            return await query.ToListAsync();
        }

        /// <summary>
        /// Checks if a motorcycle has any rentals.
        /// </summary>
        /// <param name="motorcycleId">The ID of the motorcycle to check.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the motorcycle has any rentals.</returns>
        public async Task<bool> HasRentals(int motorcycleId)
        {
            return await _dbContext.Rentals.AnyAsync(r => r.MotorcycleId == motorcycleId);
        }

        /// <summary>
        /// Adds a motorcycle.
        /// </summary>
        /// <param name="motorcycle">The motorcycle to be added.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Add(Motorcycle motorcycle)
        {
            await _dbContext.Motorcycles.AddAsync(motorcycle);
        }

        /// <summary>
        /// Updates a motorcycle with the given ID.
        /// </summary>
        /// <param name="id">The ID of the motorcycle to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Update(int id)
        {
            var motorcycle = await _dbContext.Motorcycles.FindAsync(id);
            if (motorcycle != null)
            {
                _dbContext.Motorcycles.Update(motorcycle);
                await _dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Removes a motorcycle with the specified ID from the database.
        /// </summary>
        /// <param name="id">The ID of the motorcycle to be removed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Remove(int id)
        {
            var motorcycle = await _dbContext.Motorcycles.FindAsync(id);
            if (motorcycle != null)
            {
                _dbContext.Motorcycles.Remove(motorcycle);
                await _dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Saves changes to the database.
        /// </summary>
        public async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
