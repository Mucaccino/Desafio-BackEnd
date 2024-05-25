using Motto.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Motto.Entities;
using Motto.Models;

namespace Motto.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class RentalPlanController : ControllerBase
    {
        private readonly IRentalPlanService _rentalPlanService;

        public RentalPlanController(IRentalPlanService rentalPlanService)
        {
            _rentalPlanService = rentalPlanService;
        }

        [Authorize(Roles = "Admin, DeliveryDriver")]
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<RentalPlan>>> GetAll()
        {
            var result = await _rentalPlanService.GetAll();

            if (result == null)
            {
                return NotFound("Nenhum plano de aluguel encontrado.");
            }

            return Ok(result);
        }
    }
}
