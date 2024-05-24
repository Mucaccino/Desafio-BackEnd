using System.Collections.Generic;
using System.Threading.Tasks;
using Motto.Controllers;
using Motto.Entities;
using Motto.Models;
using Motto.Repositories;

namespace Motto.Services
{
    public class RentalPlanService : IRentalPlanService
    {
        private readonly IRentalPlanRepository _rentalPlanRepository;

        public RentalPlanService(IRentalPlanRepository rentalPlanRepository)
        {
            _rentalPlanRepository = rentalPlanRepository;
        }

        public async Task<IEnumerable<RentalPlan>> GetAllRentalPlans()
        {
            return await _rentalPlanRepository.GetAllRentalPlans();
        }
    }
}
