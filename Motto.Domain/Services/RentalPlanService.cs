using Motto.Services.Interfaces;
using Motto.Entities;
using Motto.Repositories.Interfaces;

namespace Motto.Services
{
    /// <summary>
    /// Represents a service for managing rental plans.
    /// </summary>
    public class RentalPlanService : IRentalPlanService
    {
        private readonly IRentalPlanRepository _rentalPlanRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalPlanService"/> class.
        /// </summary>
        /// <param name="rentalPlanRepository">The rental plan repository.</param>
        public RentalPlanService(IRentalPlanRepository rentalPlanRepository)
        {
            _rentalPlanRepository = rentalPlanRepository;
        }

        /// <summary>
        /// Gets all rental plans.
        /// </summary>
        /// <returns>The collection of rental plans.</returns>
        public async Task<IEnumerable<RentalPlan>> GetAll()
        {
            return await _rentalPlanRepository.GetAll();
        }
    }
}
