using Microsoft.EntityFrameworkCore;
using Motto.Entities;
using Motto.Models;
using Motto.Repositories.Interfaces;

namespace Motto.Repositories
{

    /// <summary>
    /// Represents a repository for managing rentals.
    /// </summary>
    public class RentalRepository : IRentalRepository
    {
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public RentalRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves a rental by its ID.
        /// </summary>
        /// <param name="id">The ID of the rental.</param>
        /// <returns>The rental with the specified ID, or null if not found.</returns>
        public async Task<Rental?> GetById(int id)
        {
            return await _dbContext.Rentals
                .Include(r => r.Motorcycle)
                .Include(r => r.DeliveryDriver)
                .Include(r => r.RentalPlan)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <summary>
        /// Retrieves all rentals.
        /// </summary>
        /// <returns>A collection of rentals.</returns>
        public async Task<IEnumerable<Rental>> GetAll()
        {
            return await _dbContext.Rentals.ToListAsync();
        }

        /// <summary>
        /// Adds a new rental.
        /// </summary>
        /// <param name="rental">The rental to add.</param>
        public async Task Add(Rental rental)
        {
            await _dbContext.Rentals.AddAsync(rental);
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
