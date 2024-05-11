using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Motto.Models;
using Motto.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;

namespace Motto.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly string _jwtKey;

        public UserController(ApplicationDbContext dbContext, string jwtKey)
        {
            _dbContext = dbContext;
            _jwtKey = jwtKey;
        }

        [HttpPost("register/deliveryDriver")]
        public async Task<ActionResult<User>> RegisterDeliveryDriver(RegisterModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                Username = registerModel.Username,
                Name = registerModel.Name,
                Type = UserType.DeliveryDriver // Define o tipo de usuário como DeliveryDriver
            };

            user.SetPassword(registerModel.Password);

            // Hash da senha e outras lógicas de segurança podem ser aplicadas aqui, se necessário

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return Ok("Usuário criado com sucesso");
        }
    }

    public class RegisterModel
    {
        public required string Name { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}



