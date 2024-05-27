using Motto.Models;

namespace Motto.Repositories.Interfaces
{
    /// <summary>
    /// Represents a repository for <see cref="DeliveryDriver"/> objects.
    /// </summary>
    public interface IDeliveryDriverRepository
    {
        /// <summary>
        /// Retrieves a <see cref="DeliveryDriver"/> by its ID.
        /// </summary>
        /// <param name="id">The ID of the <see cref="DeliveryDriver"/> to retrieve.</param>
        /// <returns>A <see cref="Task{DeliveryDriver}"/> representing the asynchronous operation. The task result contains the <see cref="DeliveryDriver"/> with the specified ID, or null if not found.</returns>
        Task<DeliveryDriver?> GetById(int id);

        /// <summary>
        /// Retrieves all <see cref="DeliveryDriver"/> objects.
        /// </summary>
        /// <returns>A <see cref="DeliveryDriver"/> representing the asynchronous operation. The task result contains a collection of all <see cref="DeliveryDriver"/> objects.</returns>
        Task<IEnumerable<DeliveryDriver>> GetAll();

        /// <summary>
        /// Adds a new <see cref="DeliveryDriver"/> to the repository.
        /// </summary>
        /// <param name="driver">The <see cref="DeliveryDriver"/> to add.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task Add(DeliveryDriver driver);

        /// <summary>
        /// Updates an existing <see cref="DeliveryDriver"/> in the repository.
        /// </summary>
        /// <param name="driver">The updated <see cref="DeliveryDriver"/>.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task Update(DeliveryDriver driver);

        /// <summary>
        /// Removes a <see cref="DeliveryDriver"/> from the repository.
        /// </summary>
        /// <param name="id">The ID of the <see cref="DeliveryDriver"/> to remove.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task Remove(int id);
    }
}
