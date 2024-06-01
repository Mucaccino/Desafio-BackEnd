using Motto.Domain.Services.Results;
using OneOf;

namespace Motto.Domain.Services.Interfaces;

public interface IAuthService
{
    Task<AuthenticateUserResult> AuthenticateUser(string username, string password);
    Task<ServiceResult<OneOf<RefreshTokenResult, string>>> RefreshToken(string token, string refreshToken);
}