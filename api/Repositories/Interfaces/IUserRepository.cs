using Motto.Models;

namespace Motto.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task Add(User user);
        Task<User?> GetByUsername(string username);
        Task<User?> GetById(int userId);
        Task SaveChanges();
        Task Update(User user);
    }
}