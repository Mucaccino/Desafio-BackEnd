using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Motto.Entities;
using Motto.Models;

namespace Motto.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MotorcycleController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public MotorcycleController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] MotorcycleCreateModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            // Convertendo o objeto MotorcycleCreateModel para um objeto Motorcycle
            var motorcycle = new Motorcycle
            {
                Year = model.Year,
                Model = model.Model,
                Plate = model.Plate
            };

            _dbContext.Motorcycles.Add(motorcycle);
            await _dbContext.SaveChangesAsync();

            return Ok("Moto cadastrada com sucesso");
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message?.Contains("Plate") ?? false)
        {
            // Se a exceção indica uma violação de restrição única (placa duplicada)
            return Conflict("Já existe uma moto com essa placa");
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] MotorcycleCreateModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingMotorcycle = await _dbContext.Motorcycles.FirstOrDefaultAsync(m => m.Id == id);

        if (existingMotorcycle == null)
        {
            return NotFound();
        }

        // Verificar se a placa já existe em outra moto
        var existingPlateMotorcycle = await _dbContext.Motorcycles.FirstOrDefaultAsync(m => m.Plate == model.Plate && m.Id != id);
        if (existingPlateMotorcycle != null)
        {
            return Conflict("Já existe uma moto com essa placa");
        }

        existingMotorcycle.Year = model.Year;
        existingMotorcycle.Model = model.Model;
        existingMotorcycle.Plate = model.Plate;

        _dbContext.Motorcycles.Update(existingMotorcycle);
        await _dbContext.SaveChangesAsync();

        return Ok("Moto atualizada com sucesso");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var motorcycle = await _dbContext.Motorcycles.FindAsync(id);

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
        IQueryable<Motorcycle> query = _dbContext.Motorcycles;

        if (!string.IsNullOrEmpty(plateFilter))
        {
            // Filtra as motos com base na placa
            query = query.Where(m => m.Plate.Contains(plateFilter));
        }

        // Executa a consulta e retorna as motos
        return await query.ToListAsync();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("remove/{id}")]
    public async Task<IActionResult> RemoveMotorcycle(int id)
    {
        // Busca a moto pelo ID
        var motorcycle = await _dbContext.Motorcycles.FindAsync(id);

        // Se a moto não existe, retorna NotFound
        if (motorcycle == null)
        {
            return NotFound();
        }

        // Verifica se existem locações associadas à moto
        var hasRentals = _dbContext.Rentals.Any(r => r.MotorcycleId == id);

        // Se existirem locações, retorna BadRequest
        if (hasRentals)
        {
            return BadRequest("Não é possível remover a moto porque existem locações associadas a ela.");
        }

        // Remove a moto do contexto e salva as alterações
        _dbContext.Motorcycles.Remove(motorcycle);
        await _dbContext.SaveChangesAsync();

        return Ok("Moto removida com sucesso.");
    }
}

public class MotorcycleCreateModel
{
    public int Year { get; set; }
    public string Model { get; set; }
    public string Plate { get; set; }
}
