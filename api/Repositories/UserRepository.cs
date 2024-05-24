using Microsoft.EntityFrameworkCore;
using Motto.Entities;
using Motto.Models;
using System.Threading.Tasks;

namespace Motto.Repositories
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<User?> GetUserByUsernameAsync(string username);
        Task SaveChangesAsync();
    }

    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
        public async Task AddUserAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}

