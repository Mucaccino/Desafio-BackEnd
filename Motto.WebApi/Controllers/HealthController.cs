using Microsoft.AspNetCore.Mvc;

namespace Motto.Controllers
{
    /// <summary>
    /// Controller for checking the health of the application.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly ILogger<HealthController> _logger;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="HealthController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance used for logging.</param>
        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Checks the health of the application.
        /// </summary>
        /// <returns>An IActionResult with a 200 OK status code if the application is healthy.</returns>
        [HttpGet]
        public IActionResult CheckHealth()
        {
            _logger.LogInformation("Health check request received");
            // Here you can add logic to check the health of your application.
            _logger.LogInformation("Application is healthy");
            return Ok("Healthy");
        }
    }
}