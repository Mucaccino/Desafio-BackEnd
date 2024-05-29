using Microsoft.AspNetCore.Mvc;
using Motto.Controllers;
using Motto.Entities;
using Motto.Data;
using Moq;
using Moq.EntityFrameworkCore;
using Motto.Services;
using Motto.Repositories;
using Motto.DTOs;
using Motto.Enums;

namespace Motto.Tests
{
    [TestClass]
    public class UserControllerTests
    {
        private UserController _userController;
        private Mock<ApplicationDbContext> _mockDbContext;

        [TestInitialize]
        public void Initialize()
        {

            _mockDbContext = new Mock<ApplicationDbContext>();
            _mockDbContext.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);
            _mockDbContext.Setup(x => x.Users)
                .ReturnsDbSet(TestDataHelper.GetFakeUserList());
            var _service = new UserService(new UserRepository(_mockDbContext.Object));
            _userController = new UserController(_service);
        }


        [TestMethod]
        public async Task TestRegisterAdmin()
        {
            // Arrange
            var registerModel = new UserCreateRequest
            {
                Name = "Murillo Carmo",
                Username = "murillodocarmo@gmail.com",
                Password = "123mudar"
            };

            // Act
            var result = await _userController.RegisterAdmin(registerModel);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual("Administrador criado com sucesso", okResult?.Value);

            _mockDbContext.Verify(db => db.Users.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockDbContext.Verify(db => db.SaveChangesAsync(default), Times.Once);
        }

        [TestMethod]
        public async Task TestRegisterDeliveryDriver()
        {
            // Arrange
            var registerModel = new DeliveryDriverCreateRequest
            {
                Name = "Murillo Carmo",
                Username = "murillodocarmo@gmail.com",
                Password = "123mudar",
                CNPJ = "12345678901234",
                DateOfBirth = new DateTime(1985, 8, 11),
                DriverLicenseNumber = "123456789",
                DriverLicenseType = UserType.DeliveryDriver.ToString()
            };

            // Act
            var result = await _userController.RegisterDeliveryDriver(registerModel);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual("Entregador criado com sucesso", okResult?.Value);

            _mockDbContext.Verify(db => db.Users.AddAsync(It.IsAny<DeliveryDriverUser>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockDbContext.Verify(db => db.SaveChangesAsync(default), Times.Once);
        }
    }
}