using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;
using Motto.Controllers;
using Motto.Entities;
using Motto.Models;
using Motto.Repositories;
using Motto.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Motto.Tests
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