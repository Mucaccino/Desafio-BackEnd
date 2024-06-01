using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using Motto.WebApi.Dtos;
using Motto.Data;
using Motto.Data.Repositories;
using Motto.Domain.Services.Results;
using Motto.Domain.Services;
using Motto.WebApi.Controllers;

namespace Motto.Tests
{
    [TestClass]
    public class AuthControllerTests
    {
        private readonly Mock<ApplicationDbContext> _dbContextMock;
        private readonly AuthService _authServiceMock;
        private readonly AuthController _authController;

        public AuthControllerTests()
        {
            // Arrange
            _dbContextMock = new Mock<ApplicationDbContext>();
            _dbContextMock
                .Setup(x => x.Users).ReturnsDbSet(TestDataHelper.GetFakeUserList());
            _authServiceMock = new AuthService(new UserRepository(_dbContextMock.Object), TestDataHelper.GetJwtToken(), Mock.Of<ILogger<AuthService>>());
            _authController = new(_authServiceMock, Mock.Of<ILogger<AuthController>>());
        }

        [TestMethod]
        public async Task AuthController_ReturnsOkResult_WhenCredentialsAreValid()
        {
    
            // Act
            var result = await _authController.Authenticate(
                new LoginRequest { Username = "admin", Password = "123mudar" }); 

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var response = okResult.Value as AuthenticateUserResult;
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.AccessToken);
        }

        [TestMethod]
        public async Task AuthController_ReturnsNotFoundResult_WhenUserIsNotFound()
        {
            // Arrange
            var loginModel = new LoginRequest { Username = "null", Password = "null" };

            // Act
            var result = await _authController.Authenticate(loginModel);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
            var notFoundObjectResult = result.Result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundObjectResult);
        }

        [TestMethod]
        public async Task AuthController_ReturnsUnauthorizedResult_WhenPasswordIsIncorrect()
        {
            // Arrange
            var loginModel = new LoginRequest { Username = "admin", Password = "wrongpassword" };

            // Act
            var result = await _authController.Authenticate(loginModel);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(UnauthorizedObjectResult));
            var unauthorizedResult = result.Result as UnauthorizedObjectResult;
            Assert.IsNotNull(unauthorizedResult);
        }
    }

}
