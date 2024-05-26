using Motto.Models;

namespace Motto.Repositories.Interfaces
{
    public interface IRentalRepository
    {
        Task Add(Rental rental);
        Task<IEnumerable<Rental>> GetAll();
        Task<Rental?> GetById(int id);
        Task SaveChanges();
    }
}