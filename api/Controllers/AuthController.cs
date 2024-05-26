using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using Motto.Models;
using Motto.Services;
using Motto.Services.Interfaces;

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
        /// <param name="loginModel">The login credentials of the user.</param>
        /// <returns>The login response containing the authentication token and user information.</returns>
        [HttpPost("login")]
        public async Task<ActionResult<LoginModelResponse>> AuthenticateUser(LoginModel loginModel)
        {
            try
            {
                var response = await _authService.AuthenticateUserAsync(loginModel.Username, loginModel.Password);
                return Ok(response);
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            catch (IncorrectPasswordException ex)
            {
                _logger.LogError(ex.Message);
                return Unauthorized(ex.Message);
            }
        }
    }
}
