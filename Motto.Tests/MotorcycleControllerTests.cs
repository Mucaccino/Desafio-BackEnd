using Microsoft.AspNetCore.Mvc;
using Moq;
using Motto.Controllers;
using Motto.Entities;
using Motto.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Moq.EntityFrameworkCore;
using Motto.Repositories;
using Motto.Services;
using Motto.Dtos;
using AutoMapper;
using Motto.WebApi;

namespace Motto.Tests
{

    [TestClass]
    public class MotorcycleControllerTests
    {

        private Mock<ApplicationDbContext> _dbContext;
        private MotorcycleController _controller;

        private IList<Motorcycle> _motorcycleFakeList = TestDataHelper.GetFakeMotorcycleList();
        private IList<Rental> _rentalFakeRentalList = TestDataHelper.GetFakeRentalList();

        [TestInitialize]
        public void Initialize()
        {
            _dbContext = new Mock<ApplicationDbContext>();

            // Seed database
            _dbContext.Setup(x => x.RentalPlans)
                .ReturnsDbSet(TestDataHelper.GetFakeRentalPlanList());                
            _dbContext.Setup(x => x.DeliveryDriverUsers)
                .ReturnsDbSet(TestDataHelper.GetFakeDeliveryDriverList());                
            _dbContext.Setup(x => x.Users)
                .ReturnsDbSet(TestDataHelper.GetFakeUserList());                
            _dbContext.Setup(x => x.Rentals)
                .ReturnsDbSet(_rentalFakeRentalList);          
            _dbContext.Setup(x => x.Motorcycles)
                .ReturnsDbSet(_motorcycleFakeList);

            _dbContext.Setup(x => x.SaveChanges()).Returns(1);
            _dbContext.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

            _dbContext.Setup(x => x.Add(It.IsAny<Rental>())).Callback<Rental>((rental) => {
                 _dbContext.Setup(x => x.Rentals)
                    .ReturnsDbSet(_rentalFakeRentalList = _rentalFakeRentalList.Append(rental).ToList());
            });
            _dbContext.Setup(x => x.Add(It.IsAny<Motorcycle>())).Callback<Motorcycle>((motorcyle) => {
                 _dbContext.Setup(x => x.Motorcycles)
                    .ReturnsDbSet(_motorcycleFakeList = _motorcycleFakeList.Append(motorcyle).ToList());
            });

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

            // Mock do serviço
            var _serviceMock = new MotorcycleService(new MotorcycleRepository(_dbContext.Object));
            var _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile())));

            // Configure o contexto do controlador
            _controller = new MotorcycleController(_serviceMock, _mapper)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };
        }
        
        [TestMethod]
        public async Task Create_ValidModel_ReturnsOk()
        {
            // Arrange
            var model = new MotorcycleCreateRequest
            {
                Year = 2021,
                Model = "Model Y",
                Plate = "XYZ5678"
            };

            // Act
            var result = await _controller.Create(model, null);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual("Moto cadastrada com sucesso", okResult?.Value);
        }

        [TestMethod]
        public async Task Create_DuplicatePlate_ReturnsConflict()
        {
            // Arrange
            var existingMotorcycle = new Motorcycle
            {
                Id = 1,
                Year = 2021,
                Model = "Model Z",
                Plate = "XYZ5678"
            };
            _dbContext.Object.Add(existingMotorcycle);            
            _dbContext.Object.SaveChanges();

            // Arrange
            var model = new MotorcycleCreateRequest
            {
                Year = 2021,
                Model = "Model Y",
                Plate = "XYZ5678"
            };

            // Act
            var result = await _controller.Create(model, null);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(ConflictObjectResult));
            var conflictResult = result.Result as ConflictObjectResult;
            Assert.AreEqual("Já existe uma moto com essa placa", conflictResult?.Value);
        }

        [TestMethod]
        public async Task Update_ValidModel_ReturnsOk()
        {
            // Arrange
            var newMotorcycle = new Motorcycle
            {
                Id = 1,
                Year = 2021,
                Model = "Model Z",
                Plate = "XYZ5678"
            };
            _dbContext.Object.Add(newMotorcycle);
            _dbContext.Object.SaveChanges();

            var model = new MotorcycleCreateRequest
            {
                Year = 2021,
                Model = "Model Y",
                Plate = "XYZ5678"
            };

            // Act
            var result = await _controller.Update(1, model);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual("Moto atualizada com sucesso", okResult?.Value);
        }

        [TestMethod]
        public async Task Update_DuplicatePlate_ReturnsConflict()
        {
            // Arrange
            var existingMotorcycle = new Motorcycle
            {
                Id = 1,
                Year = 2021,
                Model = "Model Z",
                Plate = "XYZ5678"
            };
            _dbContext.Object.Add(existingMotorcycle);

            var updateMotorcycle = new Motorcycle
            {
                Id = 2,
                Year = 2021,
                Model = "Model X",
                Plate = "XXX5678"
            };
            _dbContext.Object.Add(updateMotorcycle);

            _dbContext.Object.SaveChanges();

            var model = new MotorcycleCreateRequest
            {
                Year = updateMotorcycle.Year,
                Model = updateMotorcycle.Model,
                Plate = "XYZ5678"
            };
            
            // Act
            var result = await _controller.Update(2, model);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(ConflictObjectResult));
            var conflictResult = result.Result as ConflictObjectResult;
            Assert.AreEqual("Já existe uma moto com essa placa", conflictResult?.Value);
        }

        [TestMethod]
        public async Task GetById_ExistingMotorcycle_ReturnsOk()
        {
            // Arrange
            var motorcycle = new Motorcycle
            {
                Id = 1,
                Year = 2020,
                Model = "Model X",
                Plate = "ABC1234"
            };
            _dbContext.Object.Add(motorcycle);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult?.Value);
        }

        [TestMethod]
        public async Task GetById_NonExistingMotorcycle_ReturnsNotFound()
        {
            // Act
            var result = await _controller.GetById(99);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task RemoveMotorcycle_NoRentals_ReturnsOk()
        {
            // Arrange
            var motorcycle = new Motorcycle
            {
                Id = 1,
                Year = 2020,
                Model = "Model X",
                Plate = "ABC1234"
            };
            _dbContext.Object.Add(motorcycle);

            // Act
            var result = await _controller.RemoveMotorcycle(1);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual("Moto removida com sucesso.", okResult?.Value);
        }

        [TestMethod]
        public async Task RemoveMotorcycle_WithRentals_ReturnsBadRequest()
        {
            // Arrange
            var motorcycle = new Motorcycle
            {
                Id = 1,
                Year = 2020,
                Model = "Model X",
                Plate = "ABC1234"
            };
            _dbContext.Object.Add(motorcycle);
                        
            var rental = new Rental
            {
                Id = 1,
                MotorcycleId = 1,
                DeliveryDriverId = 1,
                StartDate = DateTime.Today,
                ExpectedEndDate = DateTime.Today.AddDays(7)
            };
            _dbContext.Object.Add(rental);

            _dbContext.Object.SaveChanges();

            // Act
            var result = await _controller.RemoveMotorcycle(1);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(ConflictObjectResult));
            var badRequestResult = result.Result as ConflictObjectResult;
            Assert.AreEqual("Não é possível remover a moto porque existem locações associadas a ela.", badRequestResult?.Value);
        }
        
    }
}
