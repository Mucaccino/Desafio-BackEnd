using Microsoft.EntityFrameworkCore;
using Motto.Entities;
using Motto.Models;
using System.Threading.Tasks;

namespace Motto.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsernameAsync(string username);
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
    }
}
