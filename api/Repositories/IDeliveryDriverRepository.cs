using System.Collections.Generic;
using System.Threading.Tasks;
using Motto.Entities;
using Motto.Models;

namespace Motto.Repositories
{
    public interface IDeliveryDriverRepository
    {
        Task<DeliveryDriver> GetByIdAsync(int id);
        Task<IEnumerable<DeliveryDriver>> GetAllAsync();
        Task AddAsync(DeliveryDriver driver);
        Task UpdateAsync(DeliveryDriver driver);
        Task DeleteAsync(int id);
    }
}
