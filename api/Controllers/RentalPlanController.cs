using Motto.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Motto.Entities;
using Motto.Models;

namespace Motto.Controllers
{

    /// <summary>
    /// Represents a controller for managing rental plans.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RentalPlanController : ControllerBase
    {
        private readonly IRentalPlanService _rentalPlanService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalPlanController"/> class.
        /// </summary>
        /// <param name="rentalPlanService">The rental plan service.</param>
        public RentalPlanController(IRentalPlanService rentalPlanService)
        {
            _rentalPlanService = rentalPlanService;
        }

        /// <summary>
        /// Gets all rental plans.
        /// </summary>
        /// <returns>The list of rental plans.</returns>
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
