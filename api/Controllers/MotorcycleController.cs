using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Motto.Models;
using Motto.Services;
using Motto.Services.EventProducers;

namespace Motto.Controllers
{
    public interface IMotorcycleService
    {
        Task<ServiceResult<string>> CreateMotorcycle(MotorcycleCreateModel model, MotorcycleEventProducer? motorcycleEventProducer);
        Task<ServiceResult<string>> UpdateMotorcycle(int id, MotorcycleCreateModel model);
        Task<Motorcycle?> GetMotorcycleById(int id);
        Task<IEnumerable<Motorcycle>> GetMotorcycles(string? plateFilter);
        Task<ServiceResult<string>> RemoveMotorcycle(int id);
    }

    [ApiController]
    [Route("api/[controller]")]
    public class MotorcycleController : ControllerBase
    {
        private readonly MotorcycleService _motorcycleService;

        public MotorcycleController(MotorcycleService motorcycleService)
        {
            _motorcycleService = motorcycleService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<ActionResult<object>> Create([FromBody] MotorcycleCreateModel model, [FromServices] MotorcycleEventProducer? motorcycleEventProducer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _motorcycleService.CreateMotorcycle(model, motorcycleEventProducer);

            if (!result.Success)
            {
                return Conflict(result.Message);
            }

            return Ok(result.Message);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update/{id}")]
        public async Task<ActionResult<object>> Update(int id, [FromBody] MotorcycleCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _motorcycleService.UpdateMotorcycle(id, model);

            if (!result.Success)
            {
                return Conflict(result.Message);
            }

            return Ok(result.Message);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Motorcycle>> GetById(int id)
        {
            var motorcycle = await _motorcycleService.GetMotorcycleById(id);

            if (motorcycle == null)
            {
                return NotFound();
            }

            return Ok(motorcycle);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<Motorcycle>>> GetMotorcycles(string? plateFilter = null)
        {
            var motorcycles = await _motorcycleService.GetMotorcycles(plateFilter);
            return Ok(motorcycles);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("remove/{id}")]
        public async Task<ActionResult<string>> RemoveMotorcycle(int id)
        {
            var result = await _motorcycleService.RemoveMotorcycle(id);

            if (!result.Success)
            {
                return Conflict(result.Message);
            }

            return Ok(result.Message);
        }
    }
}
