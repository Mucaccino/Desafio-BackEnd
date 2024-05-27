namespace Motto.DTOs
{
    /// <summary>
    /// Login request
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// User name
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        public required string Password { get; set; }
    }
}
