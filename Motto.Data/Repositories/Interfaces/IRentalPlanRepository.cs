using Motto.Entities;

namespace Motto.Repositories.Interfaces
{
    /// <summary>
    /// Represents a repository for <see cref="RentalPlan"/>
    /// </summary>
    public interface IRentalPlanRepository
    {
        /// <summary>
        /// Gets all <see cref="RentalPlan"/>
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<RentalPlan>> GetAll();

        /// <summary>
        /// Gets a <see cref="RentalPlan"/> by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<RentalPlan?> GetById(int id);
    }
}