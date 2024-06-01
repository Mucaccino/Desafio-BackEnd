using Microsoft.EntityFrameworkCore;
using Motto.Data.Repositories.Interfaces;
using Motto.Data.Enums;
using Motto.Data.Entities;

namespace Motto.Data.Repositories
{

    /// <summary>
    /// Represents a repository for managing user operations.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context used by the repository.</param>        
        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves a user from the database by username.
        /// </summary>
        public async Task<User?> GetByUsername(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        /// <summary>
        /// Retrieves a user from the database by ID.
        /// </summary>
        public async Task<User?> GetById(int userId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        public async Task Add(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Updates a user in the database.
        /// </summary>
        public async Task Update(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<List<User>> GetAll(UserType? type = null, string? filter = null)
        {
            var users = _dbContext.Users
               .Where(u => type == null || u.Type == type)
               .Where(u => filter == null || u.Username.Contains(filter) || u.Name.Contains(filter));

            return await users.ToListAsync();
        }
    }
}

