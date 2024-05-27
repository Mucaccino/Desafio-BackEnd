namespace Motto.DTOs
{
    /// <summary>
    /// Represents a request to create an admin.
    /// </summary>
    public class UserCreateRequest
    {
        /// <summary>
        /// Gets or sets the name of the admin.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the username of the admin.
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the admin.
        /// </summary>
        public required string Password { get; set; }
    }
}
