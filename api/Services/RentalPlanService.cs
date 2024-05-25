using Motto.Services.Interfaces;
using Motto.Models;
using Motto.Repositories.Interfaces;

namespace Motto.Services
{
    public class RentalPlanService : IRentalPlanService
    {
        private readonly IRentalPlanRepository _rentalPlanRepository;

        public RentalPlanService(IRentalPlanRepository rentalPlanRepository)
        {
            _rentalPlanRepository = rentalPlanRepository;
        }

        public async Task<IEnumerable<RentalPlan>> GetAll()
        {
            return await _rentalPlanRepository.GetAll();
        }
    }
}
