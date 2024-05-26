using Motto.Models;

namespace Motto.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task Add(User user);
        Task<User?> GetByUsername(string username);
        Task SaveChanges();
    }
}