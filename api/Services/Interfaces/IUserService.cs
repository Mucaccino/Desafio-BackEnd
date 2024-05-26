using Motto.Models;

namespace Motto.Services.Interfaces;

public interface IUserService
{
    Task<ServiceResult<string>> RegisterAdmin(CreateAdminRequest registerModel);
    Task<ServiceResult<string>> RegisterDeliveryDriver(CreateDeliveryDriverRequest registerModel);
}