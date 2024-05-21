using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Motto.Entities;
using Motto.Models;
using Namotion.Reflection;

namespace Motto.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RentalController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public RentalController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
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
        var deliveryDriver = await _dbContext.DeliveryDrivers.FirstOrDefaultAsync(x => x.Id == userId);
        if (deliveryDriver == null)
        {
            return BadRequest("O entregador não existe.");
        }

        // Verificar se o entregador está habilitado na categoria A
        if (!deliveryDriver.DriverLicenseType.Contains("A"))
        {
            return BadRequest("O entregador não está habilitado na categoria A.");
        }

        // Verificar se o plano de aluguel é válido
        var rentalPlan = await _dbContext.RentalPlans.FirstOrDefaultAsync(x => x.Id == registerModel.RentalPlanId);
        if (rentalPlan == null)
        {
            return BadRequest("Plano de aluguel inválido.");
        }

        // Criar e salvar o objeto Rental
        var rental = new Rental
        {
            DeliveryDriverId = deliveryDriver.Id,
            MotorcycleId = registerModel.MotorcycleId,
            RentalPlanId = registerModel.RentalPlanId,
            StartDate = DateTime.Today.AddDays(1), // obrigatoriamente proximo dia
            ExpectedEndDate =  DateTime.Today.AddDays(rentalPlan.Days) // baseado no plano
        };

        try
        {
            _dbContext.Rentals.Add(rental);
            await _dbContext.SaveChangesAsync();

            return Ok(rental);
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, "Erro ao salvar os dados no banco de dados: " + ex.Message);
        }
    }

    [Authorize(Roles = "DeliveryDriver")]
    [HttpPost("{id}/deliver")]
    public async Task<ActionResult<object>> DeliverMotorcycle(int id, DateTime endDate)
    {
        // Encontrar o aluguel com o ID fornecido
        var rental = await _dbContext.Rentals
            .FirstOrDefaultAsync<Rental>(x => x.Id == id);

        if (rental == null)
        {
            return NotFound("Aluguel não encontrado.");
        }

        // Verificar se o entregador está associado a este aluguel
        if (rental.DeliveryDriverId.ToString() != User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
        {
            return Unauthorized("Você não está autorizado a entregar esta moto.");
        }

        // Verificar se a moto já foi entregue
        if (!rental.EndDate.HasValidNullability())
        {
            return BadRequest("A moto já foi entregue.");
        }

        if (endDate < rental.StartDate)
        {
            return BadRequest("A data de entrega não pode ser anterior à data de retirada.");
        }

        rental.EndDate = endDate;

        // Verificar se o plano de aluguel é válido
        var rentalPlan = await _dbContext.RentalPlans.FirstOrDefaultAsync<RentalPlan>(x => x.Id == rental.RentalPlanId);
        if (rentalPlan == null)
        {
            return BadRequest("Plano de aluguel inválido.");
        }

        var totalCostInfo = GetTotalCost(rental, rentalPlan, endDate);
        
        try
        {
            await _dbContext.SaveChangesAsync();
            return Ok(new { Cost = totalCostInfo, Message = "Entrega registrada com sucesso." });
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, "Erro ao salvar os dados no banco de dados: " + ex.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<Rental>>> GetAll()
    {
        IQueryable<Rental> query = _dbContext.Rentals;

        return await query.ToListAsync();
    }

    [Authorize(Roles = "Admin, DeliveryDriver")]
    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<Rental>>> GetById(int id)
    {
        var rental = await _dbContext.Rentals
            .Include(r => r.Motorcycle)
            .Include(r => r.DeliveryDriver)
            .Include(r => r.RentalPlan)
            .FirstOrDefaultAsync(r => r.Id == id);

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
        var rental = await _dbContext.Rentals
            .Include(r => r.Motorcycle)
            .Include(r => r.DeliveryDriver)
            .Include(r => r.RentalPlan)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (rental == null)
        {
            return NotFound();
        }
        
        if (endDate < rental.StartDate)
        {
            return BadRequest("A data de entrega não pode ser anterior à data de retirada.");
        }

        var totalCostInfo = GetTotalCost(rental, rental.RentalPlan, endDate);

        return Ok(totalCostInfo);
    }


    private RentalTotalCostModel GetTotalCost(Rental rental, RentalPlan rentalPlan, DateTime endDate) {
        // Se a data final for nula, marca como dia atual
        if (endDate == default(DateTime)) endDate = DateTime.Today;

        // Calcular a multa e o custo adicional, se aplicável
        if (endDate < rental.ExpectedEndDate)
        {
            int daysLate = (int)(rental.ExpectedEndDate - endDate).TotalDays;

            decimal lateFeePercentage = 0.0m;
            if (rentalPlan.Days == 7)
            {
                lateFeePercentage = 0.2m;
            }
            else if (rentalPlan.Days == 15)
            {
                lateFeePercentage = 0.4m;
            }

            decimal lateFee = rentalPlan.DailyCost * daysLate * lateFeePercentage;
            rental.PenaltyCost = lateFee;

            rental.TotalCost += lateFee;
        }
        else if (endDate > rental.ExpectedEndDate)
        {
            int additionalDays = (int)(endDate - rental.ExpectedEndDate).TotalDays;
            rental.PenaltyCost = 50 * additionalDays;
            
            rental.TotalCost += rental.PenaltyCost;
        }

        return new RentalTotalCostModel() { 
            BaseCost = rental.TotalCost - rental.PenaltyCost, 
            PenaltyCost = rental.PenaltyCost, 
            TotalCost = rental.TotalCost};
    }
    
}

public class RentalRegisterModel
{ 
    public int MotorcycleId { get; set; }
    public int RentalPlanId { get; set; }
}

public class RentalTotalCostModel
{ 
    public decimal BaseCost { get; set; }
    public decimal PenaltyCost { get; set; }
    public decimal TotalCost { get; set; }
}
