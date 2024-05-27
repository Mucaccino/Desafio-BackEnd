using Motto.Models;

namespace Motto.Repositories.Interfaces
{
    /// <summary>
    /// Represents a repository for <see cref="User"/>
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Get a <see cref="User"/> by username.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>The user with the specified username, or null if not found.</returns>
        Task<User?> GetByUsername(string username);

        /// <summary>
        /// Get a <see cref="User"/> by user ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The user with the specified ID, or null if not found.</returns>
        Task<User?> GetById(int userId);

        /// <summary>
        /// Add a new <see cref="User"/>.
        /// </summary>
        /// <param name="user">The user object to add.</param>
        Task Add(User user);

        /// <summary>
        /// Update an existing <see cref="User"/>.
        /// </summary>
        /// <param name="user">The user object to update.</param>
        Task Update(User user);
    }
}