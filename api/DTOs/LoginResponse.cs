
namespace Motto.Models;

public class LoginResponse
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    public required int UserId { get; set; }
    public required bool IsAdmin { get; set; }
    public required string Username { get; set; }
    public required string Name { get; set; }
}