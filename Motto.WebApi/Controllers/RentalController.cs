using System.Security.Claims;
using Motto.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Motto.Dtos;
using Motto.Entities;
using AutoMapper;

namespace Motto.Controllers;

/// <summary>
/// Represents a controller for managing rentals.
/// </summary>
[ApiController]
[Route("api/rental")]
public class RentalController : ControllerBase
{
    private readonly IRentalService _rentalService;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="RentalController"/> class.
    /// </summary>
    /// <param name="rentalService">The rental service.</param>
    /// <param name="mapper"></param>
    public RentalController(IRentalService rentalService, IMapper mapper)
    {
        _rentalService = rentalService;
        _mapper = mapper;
    }

    /// <summary>
    /// Registers a rental.
    /// </summary>
    /// <param name="registerModel">The rental registration model.</param>
    /// <returns>The created rental.</returns>
    [Authorize(Roles = "DeliveryDriver")]
    [HttpPost("register")]
    public async Task<ActionResult<Rental>> RentalRegister(RentalCreateRequest registerModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = Int32.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int id) ? id : 0;
        var rental = _mapper.Map<Rental>(registerModel, opts => opts.AfterMap((src, dest) => dest.DeliveryDriverId = userId));
        var result = await _rentalService.Register(rental);

        if (!result.Success)
        {
            return BadRequest(result.Message);
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Delivers a motorcycle.
    /// </summary>
    /// <param name="id">The ID of the motorcycle to be delivered.</param>
    /// <param name="endDate">The end date of the delivery.</param>
    /// <returns>An asynchronous task that returns an ActionResult containing the delivery cost and message, or a BadRequest result if the delivery was not successful.</returns>
    [Authorize(Roles = "DeliveryDriver")]
    [HttpPost("{id}/deliver")]
    public async Task<ActionResult<object>> DeliverMotorcycle(int id, DateTime endDate)
    {
        var userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "");
        var result = await _rentalService.Deliver(userId, id, endDate);

        if (!result.Success)
        {
            return BadRequest(result.Message);
        }

        return Ok(new { result.Data?.Cost, result.Data?.Message });
    }

    /// <summary>
    /// Retrieves a list of all rentals.
    /// </summary>
    /// <returns>An asynchronous task that returns an ActionResult containing a list of Rental objects, or a BadRequest result if the rentals could not be retrieved.</returns>
    [Authorize(Roles = "Admin")]
    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<Rental>>> GetAll()
    {
        var rentals = await _rentalService.GetAll();
        return Ok(rentals);
    }

    /// <summary>
    /// Retrieves a rental by its ID.
    /// </summary>
    /// <param name="id">The ID of the rental.</param>
    /// <returns>An asynchronous task that returns an ActionResult containing the rental if found, or a NotFound result if the rental is not found.</returns>
    [Authorize(Roles = "Admin, DeliveryDriver")]
    [HttpGet("{id}")]
    public async Task<ActionResult<Rental>> GetById(int id)
    {
        var rental = await _rentalService.GetById(id);

        if (rental == null)
        {
            return NotFound();
        }

        return Ok(rental);
    }

    /// <summary>
    /// Retrieves the total cost of a rental by its ID and end date.
    /// </summary>
    /// <param name="id">The ID of the rental.</param>
    /// <param name="endDate">The end date of the rental.</param>
    /// <returns>An asynchronous task that returns an ActionResult containing the total cost as a string if successful, or a BadRequest result with an error message if the rental is not found or the end date is earlier than the start date.</returns>
    [Authorize(Roles = "Admin, DeliveryDriver")]
    [HttpGet("{id}/totalCost")]
    public async Task<ActionResult<string>> GetTotalCostById(int id, DateTime endDate)
    {
        var result = await _rentalService.GetTotalCostById(id, endDate);

        if (!result.Success)
        {
            return BadRequest(result.Message);
        }

        return Ok(result.Data);
    }
}
