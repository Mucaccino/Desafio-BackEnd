using Motto.Models;

namespace Motto.Repositories.Interfaces
{
    public interface IMotorcycleRepository
    {
        Task Add(Motorcycle motorcycle);
        Task Remove(int id);
        Task Update(int id);
        Task<Motorcycle?> GetById(int id);
        Task<Motorcycle?> GetByPlate(string plate);
        Task<IEnumerable<Motorcycle>> GetAll(string? plateFilter);
        Task<bool> HasRentals(int motorcycleId);
        Task<bool> HasPlate(string plate, int id);
        Task SaveChanges();
    }
}