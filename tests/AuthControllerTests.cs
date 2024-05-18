using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using Motto.Api;
using Motto.Entities;
using Motto.Models;

namespace Motto.Tests
{
    [TestClass]
    public class AuthControllerTests
    {

        public AuthControllerTests()
        {
// [Fact]
// public async Task GetEmployees_WhenCalled_ReturnsEmployeeListAsync()
// {
//     // Arrange
//     var employeeContextMock = new Mock<EmployeeDBContext>();
//     employeeContextMock.Setup<DbSet<Employee>>(x => x.Employees)
//         .ReturnsDbSet(TestDataHelper.GetFakeEmployeeList());
//     //Act
//     EmployeesController employeesController = new(employeeContextMock.Object);
//     var employees = (await employeesController.GetEmployees()).Value;
//     //Assert
//     Assert.NotNull(employees);
//     Assert.Equal(2, employees.Count());
// }
        }

        [TestMethod]
        public async Task AuthController_ReturnsOkResult_WhenCredentialsAreValid()
        {
            // Arrange
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup<DbSet<User>>(x => x.Users)
                .ReturnsDbSet(TestDataHelper.GetFakeUserList());

            // Act
            AuthController authController = new(dbContextMock.Object, Mock.Of<ILogger<AuthController>>(), TestDataHelper.GetJwtToken());
            var result = await authController.AuthenticateUser(
                new LoginModel { Username = "admin", Password = "123mudar" }); 

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var response = okResult.Value as LoginModelResponse;
            Assert.IsNotNull(response);
            StringAssert.Contains(response.Token, "Bearer ");
        }

        [TestMethod]
        public async Task AuthController_ReturnsNotFoundResult_WhenUserIsNotFound()
        {
            // Arrange
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup<DbSet<User>>(x => x.Users)
                .ReturnsDbSet(TestDataHelper.GetFakeUserList());
            var loginModel = new LoginModel { Username = "null", Password = "null" };

            // Act
            AuthController authController = new(dbContextMock.Object, Mock.Of<ILogger<AuthController>>(), TestDataHelper.GetJwtToken());
            var result = await authController.AuthenticateUser(loginModel);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
            var notFoundObjectResult = result.Result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundObjectResult);
            Assert.AreEqual("Usuário não encontrado", notFoundObjectResult.Value);
        }

        [TestMethod]
        public async Task AuthController_ReturnsUnauthorizedResult_WhenPasswordIsIncorrect()
        {
            // Arrange
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup<DbSet<User>>(x => x.Users)
                .ReturnsDbSet(TestDataHelper.GetFakeUserList());
            var loginModel = new LoginModel { Username = "admin", Password = "wrongpassword" };

            // Act
            AuthController authController = new(dbContextMock.Object, Mock.Of<ILogger<AuthController>>(), TestDataHelper.GetJwtToken());
            var result = await authController.AuthenticateUser(loginModel);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(UnauthorizedObjectResult));
            var unauthorizedResult = result.Result as UnauthorizedObjectResult;
            Assert.IsNotNull(unauthorizedResult);
            Assert.AreEqual("Senha incorreta", unauthorizedResult.Value);
        }
    }

}
