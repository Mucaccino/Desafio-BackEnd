namespace Motto.Domain.Services.Results;

/// <summary>
/// Represents the result of a refresh token.
/// </summary>
public  class RefreshTokenResult
{
    /// <summary>
    /// Gets or sets the access token.
    /// </summary>
    public required string AccessToken { get; set; }

    /// <summary>
    /// Gets or sets the refresh token.
    /// </summary>
    public required string RefreshToken { get; set; }
}
