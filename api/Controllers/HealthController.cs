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
        /// <summary>
        /// Checks the health of the application.
        /// </summary>
        /// <returns>An IActionResult with a 200 OK status code if the application is healthy.</returns>
        [HttpGet]
        public IActionResult CheckHealth()
        {
            // Here you can add logic to check the health of your application.
            return Ok("Healthy");
        }
    }
}