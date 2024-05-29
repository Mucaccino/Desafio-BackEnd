using Motto.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Motto.Entities;
using Motto.Services.EventProducers;
using Motto.DTOs;

namespace Motto.Controllers
{
    /// <summary>
    /// Represents a controller for motorcycles.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MotorcycleController : ControllerBase
    {
        private readonly MotorcycleService _motorcycleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MotorcycleController"/> class.
        /// </summary>
        /// <param name="motorcycleService">The motorcycle service used by the controller.</param>
        public MotorcycleController(MotorcycleService motorcycleService)
        {
            _motorcycleService = motorcycleService;
        }

        /// <summary>
        /// Creates a new motorcycle with the given request model and sends a motorcycle registered event.
        /// </summary>
        /// <param name="model">The request model containing the motorcycle's information.</param>
        /// <param name="motorcycleEventProducer">The motorcycle event producer used to send the registered event.</param>
        /// <returns>An asynchronous task that returns an ActionResult containing the result message.</returns>
        /// <response code="400">If the request model is invalid.</response>
        /// <response code="409">If a motorcycle with the same plate already exists.</response>
        /// <response code="200">If the motorcycle is successfully created.</response>
        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<ActionResult<object>> Create([FromBody] MotorcycleCreateRequest model, [FromServices] MotorcycleEventProducer? motorcycleEventProducer)
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

        /// <summary>
        /// Updates a motorcycle with the given ID using the provided request model.
        /// </summary>
        /// <param name="id">The ID of the motorcycle to update.</param>
        /// <param name="model">The request model containing the updated motorcycle information.</param>
        /// <returns>An asynchronous task that returns an ActionResult containing the result message.</returns>
        /// <response code="400">If the request model is invalid.</response>
        /// <response code="409">If a motorcycle with the same plate already exists.</response>
        /// <response code="200">If the motorcycle is successfully updated.</response>
        [Authorize(Roles = "Admin")]
        [HttpPut("update/{id}")]
        public async Task<ActionResult<object>> Update(int id, [FromBody] MotorcycleCreateRequest model)
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

        /// <summary>
        /// Retrieves a motorcycle by its ID.
        /// </summary>
        /// <param name="id">The ID of the motorcycle.</param>
        /// <returns>An asynchronous task that returns an ActionResult containing the motorcycle if found, or a NotFound result if the motorcycle is not found.</returns>
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

        /// <summary>
        /// Retrieves a list of motorcycles filtered by plate.
        /// </summary>
        /// <param name="plateFilter">The plate filter to apply to the motorcycles. Optional.</param>
        /// <returns>An asynchronous task that returns an ActionResult containing an IEnumerable of Motorcycle objects.</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<Motorcycle>>> GetMotorcycles(string? plateFilter = null)
        {
            var motorcycles = await _motorcycleService.GetMotorcycles(plateFilter);
            return Ok(motorcycles);
        }
 
        /// <summary>
        /// Removes a motorcycle with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the motorcycle to be removed.</param>
        /// <returns>An asynchronous task that returns an ActionResult containing a string message. If the removal is successful, the message is "Moto removida com sucesso." Otherwise, it returns a Conflict result with the error message.</returns>
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
