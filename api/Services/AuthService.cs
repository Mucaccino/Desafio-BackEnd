using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Motto.Models;
using Motto.Entities;
using Newtonsoft.Json;
using Serilog;
using Motto.Repositories.Interfaces;
using Motto.Services.Interfaces;

namespace Motto.Services
{

    public class AuthService : IAuthService
    {
        private readonly string _jwtKey;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUserRepository userRepository, string jwtKey, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _jwtKey = jwtKey;
            _logger = logger;
        }

        public async Task<LoginModelResponse?> AuthenticateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsername(username);

            if (user == null)
            {
                _logger.LogError("Usuário não encontrado");
                throw new UserNotFoundException("Usuário não encontrado");
            }

            if (!user.VerifyPassword(password))
            {
                _logger.LogError("Senha incorreta");
                throw new IncorrectPasswordException("Senha incorreta");
            }

            var token = GenerateJwtToken(user);

            var response = new LoginModelResponse
            {
                Token = $"Bearer {token}",
                UserId = user.Id,
                Username = user.Username,
                Name = user.Name,
                IsAdmin = user.Type == UserType.Admin
            };

            _logger.LogInformation(JsonConvert.SerializeObject(response));
            return response;
        }

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Name, user.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim(ClaimTypes.Role, user.Type.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    [Serializable]
    internal class IncorrectPasswordException : Exception
    {
        public IncorrectPasswordException()
        {
        }

        public IncorrectPasswordException(string? message) : base(message)
        {
        }

        public IncorrectPasswordException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }

    [Serializable]
    internal class UserNotFoundException : Exception
    {
        public UserNotFoundException()
        {
        }

        public UserNotFoundException(string? message) : base(message)
        {
        }

        public UserNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
