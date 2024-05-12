using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Motto.Entities;
using Motto.Models;

namespace Motto.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RentalPlanController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public RentalPlanController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [Authorize(Roles = "Admin, DeliveryDriver")]
    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<RentalPlan>>> GetAll()
    {
        IQueryable<RentalPlan> query = _dbContext.RentalPlans;

        return await query.ToListAsync();
    }
}
