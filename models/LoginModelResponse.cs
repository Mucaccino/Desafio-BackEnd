
namespace Motto.Models;

public class LoginModelResponse
{
    public required string Token { get; set; }
    public required int UserId { get; set; }
    public required bool IsAdmin { get; set; }
    public required string Username { get; set; }
    public required string Name { get; set; }
}