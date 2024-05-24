using Microsoft.AspNetCore.Mvc;

namespace Motto.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult CheckHealth()
        {
            // Aqui você pode adicionar lógica para verificar a saúde da sua aplicação.
            // Por exemplo, você pode verificar conexões com bancos de dados, serviços externos, etc.

            // Se tudo estiver bem, você pode retornar um código 200 OK.
            return Ok("Healthy");
        }
    }
}