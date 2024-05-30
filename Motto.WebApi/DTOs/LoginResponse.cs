
namespace Motto.Dtos;

/// <summary>
/// Login response
/// </summary>
public class LoginResponse
{
    /// <summary>
    /// Access token
    /// </summary>
    public required string AccessToken { get; set; }

    /// <summary>
    /// Refresh token
    /// </summary>
    public required string RefreshToken { get; set; }

    /// <summary>
    /// User ID
    /// </summary>
    public required int UserId { get; set; }

    /// <summary>
    /// Is admin
    /// </summary>
    public required bool IsAdmin { get; set; }

    /// <summary>
    /// User username
    /// </summary>
    public required string Username { get; set; }

    /// <summary>
    /// User name
    /// </summary>
    public required string Name { get; set; }
}