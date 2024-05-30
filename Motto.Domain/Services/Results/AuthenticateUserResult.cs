using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motto.Domain.Services.Results
{
    /// <summary>
    /// Represents the result of a user authentication.
    /// </summary>
    public class AuthenticateUserResult
    {
        /// <summary>
        /// User ID
        /// </summary>
        public required int Id { get; set; }

        /// <summary>
        /// User username
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Is admin
        /// </summary>
        public required bool IsAdmin { get; set; }

        /// <summary>
        /// Access token
        /// </summary>
        public required string AccessToken { get; set; }

        /// <summary>
        /// Refresh token
        /// </summary>
        public required string RefreshToken { get; set; }
    }
}