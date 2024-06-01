using Motto.Domain.Services.Results;
using Motto.Entities;
using Motto.Enums;
using OneOf;

namespace Motto.Services.Interfaces;

public interface IUserService
{
    Task<ServiceResult<string>> RegisterAdmin(AdminUser user);
    Task<ServiceResult<string>> RegisterDeliveryDriver(DeliveryDriverUser deliveryDriverUser);
    Task<ServiceResult<List<User>>> GetAllUsers(UserType? type = null, string? filter = null);
    Task<ServiceResult<List<DeliveryDriverUser>>> GetAllDeliveryDriverUsers(string? filter = null);
}