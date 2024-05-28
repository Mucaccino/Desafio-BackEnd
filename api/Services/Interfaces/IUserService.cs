using Motto.DTOs;

namespace Motto.Services.Interfaces;

public interface IUserService
{
    Task<ServiceResult<string>> RegisterAdmin(UserCreateRequest registerModel);
    Task<ServiceResult<string>> RegisterDeliveryDriver(DeliveryDriverCreateRequest registerModel);
}