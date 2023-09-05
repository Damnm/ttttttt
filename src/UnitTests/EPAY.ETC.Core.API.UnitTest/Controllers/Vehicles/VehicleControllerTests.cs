using EPAY.ETC.Core.API.Controllers;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Vehicles;
using EPAY.ETC.Core.API.Core.Models.Common;
using EPAY.ETC.Core.API.Core.Validation;
using EPAY.ETC.Core.API.UnitTest.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Moq;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using EPAY.ETC.Core.API.Core.Models.Vehicle;

namespace EPAY.ETC.Core.API.UnitTest.Controllers.Vehicles
{
    public class VehicleControllerTests : ControllerBase
    {
        private Mock<IVehicleService> _vehicleServiceMock = new Mock<IVehicleService>();
        private Mock<ILogger<VehicleController>> _loggerMock = new Mock<ILogger<VehicleController>>();
        private VehicleModel requestMock = new VehicleModel()
        {
            PlateNumber = "Some Plate number",
            PlateColor = "Some Plate colour",
            RFID = "Some RFID",
            Make = "Some make",
            Seat = 10,
            VehicleType = "Loại 2",
            Weight = 7000,
        };
        private ValidationResult<VehicleModel> responseMock = new ValidationResult<VehicleModel>(new VehicleModel()
        {
            Id = new Guid(),
            CreatedDate = DateTime.Now,
            PlateNumber = "Some Plate number",
            PlateColor = "Some Plate colour",
            RFID = "Some RFID",
            Make = "Some make",
            Seat = 10,
            VehicleType = "Loại 2",
            Weight = 7000,
        });
        #region AddAsync
        // Happy case 200/201
        [Fact]
        public void GiveValidRequest_WhenApiAddAsyncIsCalled_ThenReturnGoodValidation()
        {
            //arrange

            //act
            var actualResult = ValidateModelTest.ValidateModel(responseMock);

            //assert 
            actualResult.Should().NotBeNull();
            Assert.True(actualResult.Count() == 0);
        }

        [Fact]
        public async Task GivenValidRequest_WhenApiAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _vehicleServiceMock.Setup(x => x.AddAsync(It.IsAny<VehicleModel>())).ReturnsAsync(responseMock);
            var request = new VehicleModel();

            // Act
            var vehicleController = new VehicleController(_loggerMock.Object
                , _vehicleServiceMock.Object);
            var actualResult = await vehicleController.AddAsync(request);
            var data = ((ObjectResult)actualResult).Value as ValidationResult<VehicleModel>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleController.AddAsync)}...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleController.AddAsync)} method", Times.Never, null);
            actualResult.Should().BeOfType<ObjectResult>();
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status201Created);
            data.Succeeded.Should().BeTrue();
            data.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task GivenValidRequestAndVehicleAlreadyExists_WhenApiAddAsyncIsCalled_ThenReturnConflict()
        {
            // Arrange
            responseMock = new ValidationResult<VehicleModel>(null, new List<ValidationError>()
            {
                ValidationError.Conflict
            });
            _vehicleServiceMock.Setup(x => x.AddAsync(It.IsAny<VehicleModel>())).ReturnsAsync(responseMock);
            var request = new VehicleModel();

            // Act
            var vehicleController = new VehicleController(_loggerMock.Object
                , _vehicleServiceMock.Object);
            var actualResult = await vehicleController.AddAsync(request);
            var data = actualResult as ConflictObjectResult;
            var actualResultRespone = data!.Value as ValidationResult<VehicleModel>;
            // Assert
            _vehicleServiceMock.Verify(x => x.AddAsync(It.IsAny<VehicleModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleController.AddAsync)}...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleController.AddAsync)} method", Times.Never, null);
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status409Conflict);
            actualResultRespone.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone.Errors.Count(x => x.Code == StatusCodes.Status409Conflict) > 0);
        }

        // Unhappy case 400
        [Fact]
        public async Task GivenValidRequestAndVehicleServiceIsDown_WhenApiAddAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            var someEx = new Exception("An error occurred when calling AddAsync method");
            _vehicleServiceMock.Setup(x => x.AddAsync(It.IsAny<VehicleModel>())).ThrowsAsync(someEx);

            // Act
            var vehicleController = new VehicleController(_loggerMock.Object
                , _vehicleServiceMock.Object);
            var actualResult = await vehicleController.AddAsync(It.IsAny<VehicleModel>());
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleController.AddAsync)}...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleController.AddAsync)} method", Times.Once, null);
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            actualResultRespone.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone.Errors.Count() > 0);
        }
        #endregion
        #region UpdateAsync
        // Happy case 200
        [Fact]
        public async Task GivenValidRequest_WhenUpdateAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _vehicleServiceMock.Setup(x => x.UpdateAsync(It.IsAny<VehicleModel>())).ReturnsAsync(responseMock);

            // Act
            var vehicleController = new VehicleController(_loggerMock.Object
                , _vehicleServiceMock.Object);
            var actualResult = await vehicleController.UpdateAsync(Guid.NewGuid().ToString(), new VehicleModel());
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<VehicleModel>;
            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleController.UpdateAsync)}...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleController.UpdateAsync)} method", Times.Never, null);
            actualResult.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data.Succeeded.Should().BeTrue();
            data.Data.Should().NotBeNull();
            data.Data.PlateNumber.Should().Be(responseMock.Data.PlateNumber);
            data.Data.PlateColor.Should().Be(responseMock.Data.PlateColor);
            data.Data.RFID.Should().Be(responseMock.Data.RFID);
            data.Data.Make.Should().Be(responseMock.Data.Make);
            data.Data.Seat.Should().Be(responseMock.Data.Seat);
            data.Data.VehicleType.Should().Be(responseMock.Data.VehicleType);
            data.Data.Weight.Should().Be(responseMock.Data.Weight);

        }
        [Fact]
        public async Task GivenValidRequestAndNonExistingVehicleId_WhenApiUpdateAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            var request = new VehicleModel()
            {
                Id = Guid.NewGuid()
            };
            responseMock = new ValidationResult<VehicleModel>(null, new List<ValidationError>()
            {
                ValidationError.NotFound
            });
            _vehicleServiceMock.Setup(x => x.UpdateAsync(It.IsAny<VehicleModel>())).ReturnsAsync(responseMock);

            // Act
            var vehicleController = new VehicleController(_loggerMock.Object
                , _vehicleServiceMock.Object);
            var actualResult = await vehicleController.UpdateAsync(Guid.NewGuid().ToString(), new VehicleModel());
            var data = actualResult as NotFoundObjectResult;
            var actualResultRespone = data!.Value as ValidationResult<VehicleModel>;

            // Assert
            _vehicleServiceMock.Verify(x => x.UpdateAsync(It.IsAny<VehicleModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleController.UpdateAsync)}...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleController.UpdateAsync)} method", Times.Never, null);
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status404NotFound);
            actualResultRespone.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone.Errors.Count(x => x.Code == StatusCodes.Status404NotFound) > 0);
        }
        // Unhappy case 400
        [Fact]
        public async Task GivenValidRequestAndVehicleServiceIsDown_WhenApiUpdateAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            var request = new VehicleModel()
            {
                Id = Guid.NewGuid()
            };
            var someEx = new Exception("An error occurred when calling UpdateAsync method");
            _vehicleServiceMock.Setup(x => x.UpdateAsync(It.IsAny<VehicleModel>())).ThrowsAsync(someEx);

            // Act
            var vehicleController = new VehicleController(_loggerMock.Object
                , _vehicleServiceMock.Object);
            var actualResult = await vehicleController.UpdateAsync(Guid.NewGuid().ToString(), It.IsAny<VehicleModel>());
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleController.UpdateAsync)}...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleController.UpdateAsync)} method", Times.Once, null);
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            actualResultRespone.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone.Errors.Count() > 0);
        }
        #endregion
        #region RemoveAsync
        // Happy case 200
        [Fact]
        public async Task GivenValidRequest_WhenApiRemoveAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var vehicleId = Guid.NewGuid();
            var responseMock = ValidationResult.Success(Guid.NewGuid());
            _vehicleServiceMock.Setup(x => x.RemoveAsync(vehicleId)).ReturnsAsync(responseMock);

            // Act
            var vehicleController = new VehicleController(_loggerMock.Object
                , _vehicleServiceMock.Object);
            var actualResult = await vehicleController.RemoveAsync(vehicleId.ToString());
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<Guid>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleController.RemoveAsync)}...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleController.RemoveAsync)} method", Times.Never, null);
            actualResult.Should().BeOfType<OkObjectResult>();
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data.Succeeded.Should().BeTrue();
            data.Data.Should().NotBeEmpty();
        }
        [Fact]
        public async Task GivenValidRequestAndNonExistingVehicleId_WhenApiRemoveAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            var vehicleId = Guid.NewGuid();
            var responseMock = new ValidationResult<Guid>(new List<ValidationError>()
            {
                ValidationError.NotFound
            });
            _vehicleServiceMock.Setup(x => x.RemoveAsync(vehicleId)).ReturnsAsync(responseMock);

            // Act
            var vehicleController = new VehicleController(_loggerMock.Object
                , _vehicleServiceMock.Object);
            var actualResult = await vehicleController.RemoveAsync(vehicleId.ToString());
            var data = actualResult as NotFoundObjectResult;
            var actualResultRespone = data!.Value as ValidationResult<Guid>;
            // Assert
            _vehicleServiceMock.Verify(x => x.RemoveAsync(It.IsNotNull<Guid>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleController.RemoveAsync)}...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleController.RemoveAsync)} method", Times.Never, null);
            ((NotFoundObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status404NotFound);
            actualResultRespone.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone.Errors.Count(x => x.Code == StatusCodes.Status404NotFound) > 0);
        }
        // Unhappy case 400
        [Fact]
        public async Task GivenValidRequestAndVehicleServiceIsDown_WhenApiRemoveAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            var priorityVehicleId = Guid.NewGuid();
            _vehicleServiceMock.Setup(x => x.RemoveAsync(priorityVehicleId)).ThrowsAsync(new Exception("Some exception"));

            // Act
            var vehicleController = new VehicleController(_loggerMock.Object
                , _vehicleServiceMock.Object);
            var actualResult = await vehicleController.RemoveAsync(priorityVehicleId.ToString());
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleController.RemoveAsync)}...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleController.RemoveAsync)} method", Times.Once, null);
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            actualResultRespone.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone.Errors.Count(x => x.Code == StatusCodes.Status500InternalServerError) > 0);
        }
        #endregion
        #region GetByIdAsync
        [Fact]
        public async Task GivenValidRequest_WhenApiGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _vehicleServiceMock.Setup(x => x.GetByIdAsync(new Guid())).ReturnsAsync(responseMock);

            // Act
            var vehicleController = new VehicleController(_loggerMock.Object
                , _vehicleServiceMock.Object);
            var actualResult = await vehicleController.GetByIdAsync((new Guid().ToString()));
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<VehicleModel>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleController.GetByIdAsync)}...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleController.GetByIdAsync)} method", Times.Never, null);
            actualResult.Should().BeOfType<OkObjectResult>();
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data.Succeeded.Should().BeTrue();
            data.Data.Should().NotBeNull();
            data.Data.PlateNumber.Should().Be(responseMock.Data.PlateNumber);
            data.Data.PlateColor.Should().Be(responseMock.Data.PlateColor);
            data.Data.RFID.Should().Be(responseMock.Data.RFID);
            data.Data.Make.Should().Be(responseMock.Data.Make);
            data.Data.Seat.Should().Be(responseMock.Data.Seat);
            data.Data.VehicleType.Should().Be(responseMock.Data.VehicleType);
            data.Data.Weight.Should().Be(responseMock.Data.Weight);
        }
        // Unhappy case 400
        [Fact]
        public async Task GivenValidRequestAndPriorityVehicleServiceIsDown_WhenApiGetByIdAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            var someEx = new Exception("An error occurred when calling GetByIdAsync method");
            _vehicleServiceMock.Setup(x => x.GetByIdAsync(new Guid())).ThrowsAsync(someEx);

            // Act
            var vehicleController = new VehicleController(_loggerMock.Object
                , _vehicleServiceMock.Object);
            var actualResult = await vehicleController.GetByIdAsync((new Guid().ToString()));
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleController.GetByIdAsync)}...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleController.GetByIdAsync)} method", Times.Once, null);
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            actualResultRespone.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone.Errors.Count() > 0);
        }
        #endregion
    }
}
