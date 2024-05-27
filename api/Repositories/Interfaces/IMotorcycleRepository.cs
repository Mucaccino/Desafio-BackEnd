using Motto.Models;

namespace Motto.Repositories.Interfaces
{
    /// <summary>
    /// Represents a repository for <see cref="Motorcycle"/>
    /// </summary>
    public interface IMotorcycleRepository
    {
        /// <summary>
        /// Adds a <see cref="Motorcycle"/>
        /// </summary>
        /// <param name="motorcycle"></param>
        /// <returns></returns>
        Task Add(Motorcycle motorcycle);

        /// <summary>
        /// Removes a <see cref="Motorcycle"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Remove(int id);

        /// <summary>
        /// Updates a <see cref="Motorcycle"/>
        /// </summary>
        /// <param name="motorcycle"></param>
        /// <returns></returns>
        Task Update(Motorcycle motorcycle);

        /// <summary>
        /// Gets a <see cref="Motorcycle"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Motorcycle?> GetById(int id);

        /// <summary>
        /// Gets a <see cref="Motorcycle"/>
        /// </summary>
        /// <param name="plate"></param>
        /// <returns></returns>
        Task<Motorcycle?> GetByPlate(string plate);

        /// <summary>
        /// Gets all <see cref="Motorcycle"/>
        /// </summary>
        /// <param name="plateFilter"></param>
        /// <returns></returns>
        Task<IEnumerable<Motorcycle>> GetAll(string? plateFilter);

        /// <summary>
        /// Checks if a <see cref="Motorcycle"/> has any <see cref="Rental"/>
        /// </summary>
        /// <param name="motorcycleId"></param>
        /// <returns></returns>
        Task<bool> HasRentals(int motorcycleId);

        /// <summary>
        /// Checks if a <see cref="Motorcycle"/> has a plate on another id
        /// </summary>
        /// <param name="plate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> HasPlate(string plate, int id);
    }
}