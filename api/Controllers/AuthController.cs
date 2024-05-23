using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using Motto.Models;
using Motto.Services;

namespace Motto.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

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
