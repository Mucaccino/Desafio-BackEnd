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

namespace Motto.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly string _jwtKey;

        public AuthController(ApplicationDbContext dbContext, string jwtKey)
        {
            _dbContext = dbContext;
            _jwtKey = jwtKey;
        }

        [HttpPost("login")] // Mudança do verbo HTTP de GET para POST para melhorar a segurança
        public async Task<ActionResult<User>> AuthenticateUser(LoginModel loginModel)
        {
            // Verifique se o modelo de login é válido (ex: campos obrigatórios preenchidos)
            if (loginModel == null || string.IsNullOrEmpty(loginModel.Username) || string.IsNullOrEmpty(loginModel.Password))
            {
                return BadRequest("Credenciais inválidas");
            }

            // Recupere o usuário do banco de dados com base no nome de usuário
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == loginModel.Username);
            if (user == null)
            {
                return NotFound("Usuário não encontrado");
            }

            // Verifique se a senha fornecida corresponde à senha armazenada no banco de dados
            var passwordValid = user.VerifyPassword(loginModel.Password);
            if (!passwordValid)
            {
                return Unauthorized("Senha incorreta");
            }

            var token = GenerateJwtToken(loginModel.Username);
            return Ok(new { Token = token });
        }
        
        private string GenerateJwtToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtKey); // Defina sua chave secreta

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username),
                    // Adicione quaisquer outras reivindicações necessárias
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Defina a expiração do token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }

    public class LoginModel
    {
        public string? Username { get; internal set; }
        public string? Password { get; internal set; }
    }
}



