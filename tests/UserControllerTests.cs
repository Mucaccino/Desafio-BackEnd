using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Motto.Api;
using Motto.Entities;
using Motto.Models;
using Moq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq.EntityFrameworkCore;

namespace Motto.Tests
{
    [TestClass]
    public class UserControllerTests
    {
        [TestMethod]
        public async Task TestRegisterAdmin()
        {
            // Arrange
            var registerModel = new RegisterAdminModel
            {
                Name = "Murillo Carmo",
                Username = "murillodocarmo@gmail.com",
                Password = "123mudar"
            };

            var mockDbContext = new Mock<ApplicationDbContext>();
            mockDbContext.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);
            mockDbContext.Setup<DbSet<User>>(x => x.Users)
                .ReturnsDbSet(TestDataHelper.GetFakeUserList());
            var userController = new UserController(mockDbContext.Object);

            // Act
            var result = await userController.RegisterAdmin(registerModel);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual("Administrador criado com sucesso", okResult?.Value);

            mockDbContext.Verify(db => db.Users.Add(It.IsAny<User>()), Times.Once);
            mockDbContext.Verify(db => db.SaveChangesAsync(default), Times.Once);
        }

        [TestMethod]
        public async Task TestRegisterDeliveryDriver()
        {
            // Arrange
            var registerModel = new RegisterDeliveryDriverModel
            {
                Name = "Murillo Carmo",
                Username = "murillodocarmo@gmail.com",
                Password = "123mudar",
                CNPJ = "12345678901234",
                DateOfBirth = new DateTime(1985, 8, 11),
                DriverLicenseNumber = "123456789",
                DriverLicenseType = UserType.DeliveryDriver.ToString()
            };

            var mockDbContext = new Mock<ApplicationDbContext>();
            mockDbContext.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);
            mockDbContext.Setup<DbSet<User>>(x => x.Users)
                .ReturnsDbSet(TestDataHelper.GetFakeUserList());
            var userController = new UserController(mockDbContext.Object);

            // Act
            var result = await userController.RegisterDeliveryDriver(registerModel);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual("Entregador criado com sucesso", okResult?.Value);

            mockDbContext.Verify(db => db.Users.Add(It.IsAny<DeliveryDriver>()), Times.Once);
            mockDbContext.Verify(db => db.SaveChangesAsync(default), Times.Once);
        }
    }
}