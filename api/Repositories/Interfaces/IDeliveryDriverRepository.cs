using Motto.Models;

namespace Motto.Repositories.Interfaces
{
    public interface IDeliveryDriverRepository
    {
        Task<DeliveryDriver?> GetById(int id);
        Task<IEnumerable<DeliveryDriver>> GetAll();
        Task Add(DeliveryDriver driver);
        Task Update(DeliveryDriver driver);
        Task Delete(int id);
    }
}
