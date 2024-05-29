using Microsoft.AspNetCore.Mvc;
using Motto.DTOs;
using Motto.Entities;
using Microsoft.AspNetCore.Authorization;
using Motto.Services.Interfaces;

namespace Motto.Controllers
{
    /// <summary>
    /// The UserController class is responsible for handling user registration requests.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Registers a new admin user.
        /// </summary>
        /// <param name="registerModel">The model containing the admin registration details.</param>
        /// <returns>The created admin user if successful, otherwise a bad request with the error message.</returns>
        [HttpPost("register/admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> RegisterAdmin(UserCreateRequest registerModel)
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

        /// <summary>
        /// Registers a new delivery driver user.
        /// </summary>
        /// <param name="registerModel">The model containing the delivery driver registration details.</param>
        /// <returns>The created delivery driver user if successful, otherwise a bad request with the error message or a 500 status code with the error message.</returns>
        [HttpPost("register/deliveryDriver")]
        public async Task<ActionResult<User>> RegisterDeliveryDriver(DeliveryDriverCreateRequest registerModel)
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
