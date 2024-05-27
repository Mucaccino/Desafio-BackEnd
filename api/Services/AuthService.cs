using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Motto.DTOs;
using Newtonsoft.Json;
using Motto.Repositories.Interfaces;
using Motto.Services.Interfaces;
using Motto.Controllers;
using Motto.Models;

namespace Motto.Services
{
    /// <summary>
    /// Provides authentication-related services.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly string _jwtKey;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="jwtKey">The JWT key.</param>
        /// <param name="logger">The logger.</param>
        public AuthService(IUserRepository userRepository, string jwtKey, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _jwtKey = jwtKey;
            _logger = logger;
        }
        
        /// <summary>
        /// Authenticates a user with the provided username and password.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>A task that represents the asynchronous authentication operation. The task result contains a <see cref="LoginResponse"/> object if the authentication is successful, or null if the user is not found or the password is incorrect.</returns>
        /// <exception cref="UserNotFoundException">Thrown when the user is not found.</exception>
        /// <exception cref="IncorrectPasswordException">Thrown when the password is incorrect.</exception>
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

        /// <summary>
        /// Asynchronously refreshes the access token using the provided refresh token.
        /// </summary>
        /// <param name="token">The expired access token.</param>
        /// <param name="refreshToken">The refresh token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a TokenResponse object
        /// with the new access token and refresh token.</returns>
        /// <exception cref="InvalidTokenException">Thrown when the refresh token is invalid.</exception>
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

        /// <summary>
        /// Generates an access token for the given user.
        /// </summary>
        /// <param name="user">The user for whom the access token is generated.</param>
        /// <returns>The generated access token.</returns>
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

        /// <summary>
        /// Generates a new refresh token.
        /// </summary>
        /// <returns>A new refresh token as a string.</returns>
        private string GenerateRefreshToken()
        {
            var refreshToken = Guid.NewGuid().ToString();
            // You can customize the refresh token generation logic here if needed
            return refreshToken;
        }

        /// <summary>
        /// Retrieves a ClaimsPrincipal object from an expired JWT token.
        /// </summary>
        /// <param name="token">The expired JWT token.</param>
        /// <returns>A ClaimsPrincipal object representing the authenticated user.</returns>
        /// <exception cref="SecurityTokenException">Thrown when the token is invalid.</exception>
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
