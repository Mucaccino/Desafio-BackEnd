using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Motto.Models;
using Motto.Services;

namespace Motto.Controllers;



[ApiController]
[Route("api/[controller]")]
public class RentalController : ControllerBase
{
    private readonly IRentalService _rentalService;

    public RentalController(IRentalService rentalService)
    {
        _rentalService = rentalService;
    }

    [Authorize(Roles = "DeliveryDriver")]
    [HttpPost("register")]
    public async Task<ActionResult<Rental>> RentalRegister(RentalRegisterModel registerModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = Int32.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int id) ? id : 0;
        var result = await _rentalService.RegisterRental(userId, registerModel);

        if (!result.Success)
        {
            return BadRequest(result.Message);
        }

        return Ok(result.Data);
    }

    [Authorize(Roles = "DeliveryDriver")]
    [HttpPost("{id}/deliver")]
    public async Task<ActionResult<object>> DeliverMotorcycle(int id, DateTime endDate)
    {
        var userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "");
        var result = await _rentalService.DeliverMotorcycle(userId, id, endDate);

        if (!result.Success)
        {
            return BadRequest(result.Message);
        }

        return Ok(new { result.Data?.Cost, result.Data?.Message });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<Rental>>> GetAll()
    {
        var rentals = await _rentalService.GetAllRentals();
        return Ok(rentals);
    }

    [Authorize(Roles = "Admin, DeliveryDriver")]
    [HttpGet("{id}")]
    public async Task<ActionResult<Rental>> GetById(int id)
    {
        var rental = await _rentalService.GetRentalById(id);

        if (rental == null)
        {
            return NotFound();
        }

        return Ok(rental);
    }

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
