using Microsoft.AspNetCore.Mvc;
using Motto.Dtos;
using Motto.Entities;
using Microsoft.AspNetCore.Authorization;
using Motto.Services.Interfaces;
using AutoMapper;
using Motto.Enums;

namespace Motto.Controllers
{
    /// <summary>
    /// The UserController class is responsible for handling user registration requests.
    /// </summary>
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        /// <param name="mapper">The mapper.</param>
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
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

            AdminUser user = _mapper.Map<AdminUser>(registerModel);
            var result = await _userService.RegisterAdmin(user);

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
        [HttpPost("register/delivery-driver")]
        public async Task<ActionResult<User>> RegisterDeliveryDriver(DeliveryDriverCreateRequest registerModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DeliveryDriverUser deliveryDriverUser = _mapper.Map<DeliveryDriverUser>(registerModel);
            var result = await _userService.RegisterDeliveryDriver(deliveryDriverUser);

            if (!result.Success)
            {
                return StatusCode(500, result.Message);
            }

            return Ok(result.Message);
        }

    }
}
