using Motto.Models;
using System.Threading.Tasks;

namespace Motto.Repositories.Interfaces
{
    /// <summary>
    /// Represents a repository for <see cref="Rental"/>
    /// </summary>
    public interface IRentalRepository
    {
        /// <summary>
        /// Get a <see cref="Rental"/> by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Rental?> GetById(int id);

        /// <summary>
        /// Get all <see cref="Rental"/>
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Rental>> GetAll();

        /// <summary>
        /// Add a <see cref="Rental"/>
        /// </summary>
        /// <param name="rental"></param>
        /// <returns></returns>
        Task Add(Rental rental);

        /// <summary>
        /// Update a <see cref="Rental"/>
        /// </summary>
        /// <param name="rental"></param>
        /// <returns></returns>
        Task Update(Rental rental);

        /// <summary>
        /// Remove a <see cref="Rental"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Remove(int id);
    }
}