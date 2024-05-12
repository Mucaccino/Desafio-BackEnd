using Microsoft.AspNetCore.Mvc;
using Motto.Models;
using Motto.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Motto.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public UserController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("register/admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> RegisterAdmin(RegisterAdminModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                Username = registerModel.Username,
                Name = registerModel.Name,
                Type = UserType.Admin
            };

            user.SetPassword(registerModel.Password);

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return Ok("Administrador criado com sucesso");
        }

        [HttpPost("register/deliveryDriver")]
        public async Task<ActionResult<User>> RegisterDeliveryDriver(RegisterDeliveryDriverModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new DeliveryDriver
            {
                Username = registerModel.Username,
                Name = registerModel.Name,
                CNPJ = registerModel.CNPJ,
                DateOfBirth = registerModel.DateOfBirth,
                DriverLicenseNumber = registerModel.DriverLicenseNumber,
                DriverLicenseType = registerModel.DriverLicenseType,
                Type = UserType.DeliveryDriver // Define o tipo de usuário como DeliveryDriver
            };

            user.SetPassword(registerModel.Password);
            
            try
            {
                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();

                return Ok("Entregador criado com sucesso");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Erro ao salvar os dados no banco de dados: " + ex.Message);
            }
        }
    }

    public class RegisterAdminModel
    {
        public required string Name { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public class RegisterDeliveryDriverModel
    {
        public required string Name { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string CNPJ { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public required string DriverLicenseNumber { get; set; }
        public required string DriverLicenseType { get; set; }
        public string? DriverLicenseImage { get; set; }
    }
}



