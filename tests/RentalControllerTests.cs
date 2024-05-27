using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using Motto.Controllers;
using Motto.Entities;
using Motto.DTOs;
using Motto.Repositories;
using Motto.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Motto.Tests
{
    [TestClass]
    public class RentalControllerTests
    {
        private Mock<ApplicationDbContext> _mockDbContext;
        private RentalController _controller;
        private List<Rental> _rentalFakeRentalList = TestDataHelper.GetFakeRentalList();

        [TestInitialize]
        public void Initialize()
        {
            _mockDbContext = new Mock<ApplicationDbContext>();
            
            // Seed database
            _mockDbContext.Setup(x => x.RentalPlans)
                .ReturnsDbSet(TestDataHelper.GetFakeRentalPlanList());                
            _mockDbContext.Setup(x => x.DeliveryDrivers)
                .ReturnsDbSet(TestDataHelper.GetFakeDeliveryDriverList());                
            _mockDbContext.Setup(x => x.Users)
                .ReturnsDbSet(TestDataHelper.GetFakeUserList());                
            _mockDbContext.Setup(x => x.Rentals)
                .ReturnsDbSet(TestDataHelper.GetFakeRentalList());          
            _mockDbContext.Setup(x => x.Motorcycles)
                .ReturnsDbSet(TestDataHelper.GetFakeMotorcycleList());

            _mockDbContext.Setup(x => x.Add(It.IsAny<Rental>())).Callback<Rental>((rental) => {
                 _mockDbContext.Setup(x => x.Rentals)
                    .ReturnsDbSet(_rentalFakeRentalList = _rentalFakeRentalList.Append(rental).ToList());
            });
            
            _mockDbContext.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

            // setup auth user
            var httpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "2"),
                    new Claim(ClaimTypes.Role, "DeliveryDriver")
                }))
            };

            new Mock<IHttpContextAccessor>().Setup(_ => _.HttpContext).Returns(httpContext);

            _controller = new RentalController(new RentalService(
                new RentalRepository(_mockDbContext.Object),
                new DeliveryDriverRepository(_mockDbContext.Object),
                new RentalPlanRepository(_mockDbContext.Object)))
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };
        }

        [TestMethod]
        public async Task RentalRegister_ValidModel_ReturnsOk()
        {
            // Arrange
            var registerModel = new CreateRentalRequest
            {
                MotorcycleId = 1,
                RentalPlanId = 1
            };

            // Act
            var result = await _controller.RentalRegister(registerModel);

            // Assert
            var okResult = Xunit.Assert.IsType<OkObjectResult>(result.Result);
            Xunit.Assert.IsType<Rental>(okResult.Value);
        }

        [TestMethod]
        public async Task RentalRegister_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var registerModel = new CreateRentalRequest
            {
                MotorcycleId = 1,
                RentalPlanId = 1
            };

            _controller.ModelState.AddModelError("", "Invalid model");

            // Act
            var result = await _controller.RentalRegister(registerModel);

            // Assert
            var badRequestResult = Xunit.Assert.IsType<BadRequestObjectResult>(result.Result);
            Xunit.Assert.IsType<SerializableError>(badRequestResult.Value);
        }


        [TestMethod]
        public async Task RentalRegister_InvalidDriverLicense_ReturnsBadRequest()
        {
            // Arrange
            var model = new CreateRentalRequest
            {
                MotorcycleId = 1,
                RentalPlanId = 1
            };

            var deliveryDriver = _mockDbContext.Object.DeliveryDrivers.First(x => x.Id == 2);
            deliveryDriver.DriverLicenseType = "B"; // Invalid license type
            _mockDbContext.Object.SaveChanges();

            // Act
            var result = await _controller.RentalRegister(model);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.AreEqual("O entregador não está habilitado na categoria A.", badRequestResult?.Value);
        }

        [TestMethod]
        public async Task DeliverMotorcycle_ValidRequest_ReturnsOk()
        {
            // Arrange
            var endDate = DateTime.Today;
            var rental = new Rental
            {
                Id = 1,
                MotorcycleId = 1,
                DeliveryDriverId = 2,
                RentalPlanId = 3,
                StartDate = DateTime.Today,
                ExpectedEndDate = DateTime.Today.AddDays(7)
            };
            _mockDbContext.Object.Add(rental);

            // Act
            var result = await _controller.DeliverMotorcycle(1, endDate);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult?.Value);
        }
    }
}