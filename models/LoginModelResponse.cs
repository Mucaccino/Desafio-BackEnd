
namespace Motto.Models;

public class LoginModelResponse
{
    public required string Token { get; set; }
    public required int UserId { get; set; }
}