using Motto.Models;

namespace Motto.Services.Interfaces;

public interface IAuthService
{
    Task<LoginModelResponse?> AuthenticateUserAsync(string username, string password);
    string GenerateJwtToken(User user);
}