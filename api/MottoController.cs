using Microsoft.AspNetCore.Mvc;

namespace MottoAPI
{
    [ApiController]
    [Route("api/motto")]
    public class MottoController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello Motto! Vrum-Vrum!");
        }
    }
}