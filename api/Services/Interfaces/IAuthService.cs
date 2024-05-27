using Motto.DTOs;
using Motto.DTOs;

namespace Motto.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponse?> AuthenticateUserAsync(string username, string password);
    Task<TokenResponse> RefreshTokenAsync(string token, string refreshToken);
}