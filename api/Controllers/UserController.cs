using Microsoft.AspNetCore.Mvc;
using Motto.Models;
using Microsoft.AspNetCore.Authorization;
using Motto.Services.Interfaces;

namespace Motto.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register/admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> RegisterAdmin(CreateAdminRequest registerModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.RegisterAdmin(registerModel);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpPost("register/deliveryDriver")]
        public async Task<ActionResult<User>> RegisterDeliveryDriver(CreateDeliveryDriverRequest registerModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.RegisterDeliveryDriver(registerModel);

            if (!result.Success)
            {
                return StatusCode(500, result.Message);
            }

            return Ok(result.Message);
        }
    }
}
