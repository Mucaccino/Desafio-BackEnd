using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.EntityFrameworkCore;
using Motto.Data;
using Motto.Data.Repositories;
using Motto.Domain.Services;
using Motto.Data.Entities;
using Motto.WebApi.Controllers;
using Motto.Tests.Helpers;

namespace Motto.Tests.Unitaries
{
    [TestClass]
    public class RentalPlanControllerTests
    {
        private readonly Mock<ApplicationDbContext> _mockDbContext;
        private readonly RentalPlanController _controller;

        public RentalPlanControllerTests()
        {
            _mockDbContext = new Mock<ApplicationDbContext>();
            _mockDbContext.Setup(x => x.RentalPlans)
                .ReturnsDbSet(TestDataHelper.GetFakeRentalPlanList());
            var _service = new RentalPlanService(new RentalPlanRepository(_mockDbContext.Object));
            _controller = new RentalPlanController(_service);
        }

        [TestMethod]
        public async Task GetAll_ReturnsListOfRentalPlans()
        {
            // Arrange
            var rentalPlans = TestDataHelper.GetFakeRentalPlanList();

            // Act
            var result = await _controller.GetAll();

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            var actualRentalPlans = okResult?.Value as List<RentalPlan>;
            Assert.AreEqual(rentalPlans.Count, actualRentalPlans?.Count);
        }
    }
}