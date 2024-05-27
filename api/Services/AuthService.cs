using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Motto.Models;
using Motto.Entities;
using Newtonsoft.Json;
using Serilog;
using Motto.Repositories.Interfaces;
using Motto.Services.Interfaces;
using System.Threading.Tasks;
using Motto.DTOs;
using Motto.Controllers;

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

        public async Task<LoginResponse?> AuthenticateUserAsync(string username, string password)
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

            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();

            var response = new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserId = user.Id,
                Username = user.Username,
                Name = user.Name,
                IsAdmin = user.Type == UserType.Admin
            };

            user.RefreshToken = refreshToken;
            await _userRepository.Update(user);

            _logger.LogInformation(JsonConvert.SerializeObject(response));
            return response;
        }

        public async Task<TokenResponse> RefreshTokenAsync(string token, string refreshToken)
        {
            var principal = GetPrincipalFromExpiredToken(token);
            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new InvalidTokenException("Invalid refresh token.");
            }

            var user = await _userRepository.GetById(userId);
            if (user == null || user.RefreshToken != refreshToken)
            {
                throw new InvalidTokenException("Invalid refresh token.");
            }

            var accessToken = GenerateAccessToken(user);
            var newRefreshToken = GenerateRefreshToken();

            // Atualizar o refresh token no banco de dados
            user.RefreshToken = newRefreshToken;
            await _userRepository.Update(user);

            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken
            };
        }

        private string GenerateAccessToken(User user)
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

        private string GenerateRefreshToken()
        {
            var refreshToken = Guid.NewGuid().ToString();
            // Você também pode criptografar ou fazer outras manipulações no refresh token, dependendo dos requisitos de segurança da sua aplicação
            return refreshToken;
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false // Esta opção permite a validação de tokens expirados
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}
