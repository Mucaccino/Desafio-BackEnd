using Motto.Data.Entities;
using Motto.Data.Enums;
using Motto.Domain.Services.Results;
using OneOf;

namespace Motto.Domain.Services.Interfaces;

public interface IUserService
{
    Task<ServiceResult<string>> RegisterAdmin(AdminUser user);
    Task<ServiceResult<string>> RegisterDeliveryDriver(DeliveryDriverUser deliveryDriverUser);
    Task<ServiceResult<List<User>>> GetAllUsers(UserType? type = null, string? filter = null);
    Task<ServiceResult<List<DeliveryDriverUser>>> GetAllDeliveryDriverUsers(string? filter = null);
}