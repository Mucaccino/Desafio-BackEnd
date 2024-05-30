using Microsoft.AspNetCore.Mvc;
using Motto.Dtos;
using Motto.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Motto.Domain.Exceptions;
using Microsoft.VisualBasic;

namespace Motto.Controllers
{
    /// <summary>
    /// Represents a controller for authentication operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authService">The authentication service.</param>
        /// <param name="logger">The logger.</param>
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Authenticates a user with the provided login credentials.
        /// </summary>
        /// <param name="loginRequest">The login credentials of the user. <see cref="LoginRequest"/></param>
        /// <returns>The login response containing the authentication token and user information. <see cref="LoginResponse"/></returns>
        /// <remarks>statusCode: 200 - OK</remarks>
        /// <remarks>statusCode: 404 - User not found</remarks>
        /// <remarks>statusCode: 401 - Password is incorrect</remarks>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> AuthenticateUser([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Authenticating user with username: {Username}", loginRequest.Username);

            try
            {
                var response = await _authService.AuthenticateUser(loginRequest.Username, loginRequest.Password);

                _logger.LogInformation("User authentication successful. Returning response: {@Response}", response);

                return Ok(response);

            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError(ex, "User not found: {Message}", ex.Message);

                return NotFound(ex.Message);
            }
            catch (InvalidPasswordException ex)
            {
                _logger.LogError(ex, "Invalid password: {Message}", ex.Message);

                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error authenticating user: {Message}", ex.Message);

                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Refreshes the access token using the provided refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <returns>The action result containing the token response.</returns>  
        /// <remarks>statusCode: 200 - OK</remarks>
        /// <remarks>statusCode: 401 - Invalid token</remarks>
        /// <remarks>statusCode: 401 - Invalid refresh token</remarks>
        [HttpPost("token")]
        public async Task<ActionResult<TokenResponse>> RefreshToken(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                _logger.LogError("Refresh token is missing or empty.");
                return BadRequest("Refresh token is missing or empty.");
            }

            if (Request.Headers.Authorization.IsNullOrEmpty() || !Request.Headers.Authorization.Any())
            {
                _logger.LogError("Authorization header is missing.");
                return Unauthorized("Authorization header is missing.");
            }

            var accessToken = Request.Headers.Authorization.First();
            var authHeader = accessToken?.Replace("Bearer ", "");
            if (string.IsNullOrEmpty(authHeader))
            {
                _logger.LogError("Authorization header is missing or invalid.");
                return Unauthorized("Authorization header is missing or invalid.");
            }

            try
            {
                var response = await _authService.RefreshToken(authHeader, refreshToken);

                if(response.Success)
                {
                    _logger.LogInformation("Refresh token successful. Returning response: {@Response}", response);

                    return Ok(response);
                } else
                {
                    return StatusCode(response.StatusCode, response.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing token: {Message}", ex.Message);

                return StatusCode(500, "Internal server error");
            }
        }

    }
}