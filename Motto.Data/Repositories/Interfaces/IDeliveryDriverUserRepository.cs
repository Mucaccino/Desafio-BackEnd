using Motto.Entities;

namespace Motto.Repositories.Interfaces
{
    /// <summary>
    /// Represents a repository for <see cref="DeliveryDriverUser"/> objects.
    /// </summary>
    public interface IDeliveryDriverUserRepository
    {
        /// <summary>
        /// Retrieves a <see cref="DeliveryDriverUser"/> by its ID.
        /// </summary>
        /// <param name="id">The ID of the <see cref="DeliveryDriverUser"/> to retrieve.</param>
        /// <returns>A <see cref="Task{DeliveryDriverUser}"/> representing the asynchronous operation. The task result contains the <see cref="DeliveryDriverUser"/> with the specified ID, or null if not found.</returns>
        Task<DeliveryDriverUser?> GetById(int id);

        /// <summary>
        /// Retrieves all <see cref="DeliveryDriverUser"/> objects.
        /// </summary>
        /// <returns>A <see cref="DeliveryDriverUser"/> representing the asynchronous operation. The task result contains a collection of all <see cref="DeliveryDriverUser"/> objects.</returns>
        Task<List<DeliveryDriverUser>> GetAll(string? filter = null);

        /// <summary>
        /// Adds a new <see cref="DeliveryDriverUser"/> to the repository.
        /// </summary>
        /// <param name="driver">The <see cref="DeliveryDriverUser"/> to add.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task Add(DeliveryDriverUser driver);

        /// <summary>
        /// Updates an existing <see cref="DeliveryDriverUser"/> in the repository.
        /// </summary>
        /// <param name="driver">The updated <see cref="DeliveryDriverUser"/>.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task Update(DeliveryDriverUser driver);

        /// <summary>
        /// Removes a <see cref="DeliveryDriverUser"/> from the repository.
        /// </summary>
        /// <param name="id">The ID of the <see cref="DeliveryDriverUser"/> to remove.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task Remove(int id);
    }
}
