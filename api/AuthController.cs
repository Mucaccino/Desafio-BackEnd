using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Motto.Models;
using Motto.Entities;

namespace Motto.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly string _jwtKey;

        public AuthController(ApplicationDbContext dbContext, ILogger<AuthController> logger, string jwtKey)
        {
            _logger = logger;
            _dbContext = dbContext;
            _jwtKey = jwtKey;
        }

        [HttpGet("verify/admin"), Authorize(Roles = "Admin")]
        public string GetAdmin()
        {
            return "Admin Authorized";
        }

        [HttpGet("verify/deliveryDriver"), Authorize(Roles = "DeliveryDriver")]
        public string GetDeliveryDriver()
        {
            return "DeliveryDriver Authorized";
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginModelResponse>> AuthenticateUser(LoginModel loginModel)
        {
            // Verifique se o modelo de login é válido (ex: campos obrigatórios preenchidos)
            if (loginModel == null || string.IsNullOrEmpty(loginModel.Username) || string.IsNullOrEmpty(loginModel.Password))
            {
                _logger.LogError("Credenciais inválidas");
                return BadRequest("Credenciais inválidas");
            }

            // Recupere o usuário do banco de dados com base no nome de usuário
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == loginModel.Username);
            if (user == null)
            {
                _logger.LogError("Usuário não encontrado");
                return NotFound("Usuário não encontrado");
            }

            // Verifique se a senha fornecida corresponde à senha armazenada no banco de dados
            var passwordValid = user.VerifyPassword(loginModel.Password);
            if (!passwordValid)
            {
                _logger.LogError("Senha incorreta");
                return Unauthorized("Senha incorreta");
            }

            // Configure jwt token
            var token = GenerateJwtToken(user);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = new LoginModelResponse() { Token = $"Bearer {token}", UserId = user.Id };
            _logger.LogInformation(JsonConvert.SerializeObject(response));
            return Ok(response);
        }
        
        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtKey); // Defina sua chave secreta

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Name, user.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim(ClaimTypes.Role, user.Type.ToString())
                ]),
                Expires = DateTime.UtcNow.AddHours(1), // Defina a expiração do token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
       
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}



