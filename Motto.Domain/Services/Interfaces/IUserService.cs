using Motto.Domain.Services.Results;
using Motto.Entities;

namespace Motto.Services.Interfaces;

public interface IUserService
{
    Task<ServiceResult<string>> RegisterAdmin(AdminUser user);
    Task<ServiceResult<string>> RegisterDeliveryDriver(DeliveryDriverUser deliveryDriverUser);
}