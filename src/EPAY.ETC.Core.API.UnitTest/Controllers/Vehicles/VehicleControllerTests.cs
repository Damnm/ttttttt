using EPAY.ETC.Core.API.Controllers;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Vehicles;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Core.Validation;
using EPAY.ETC.Core.API.UnitTest.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace EPAY.ETC.Core.API.UnitTest.Controllers.Vehicles
{
    public class VehicleControllerTests: ControllerBase
    {
        private Mock<IVehicleService> _vehicleServiceMock = new Mock<IVehicleService>();
        private Mock<ILogger<VehicleController>> _loggerMock = new Mock<ILogger<VehicleController>>();
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
                , _vehicleServiceMock.Object );
            var actualResult = await vehicleController.AddAsync( request);
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
            var actualResult = await vehicleController.AddAsync( request);
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
        public void GiveInvalidRequest_WhenApiAddAsyncIsCalled_ThenReturnBadValidation()
        {
            //arrange
            VehicleModel mockRequest = new VehicleModel();

            //act
            var actualResult = ValidateModelTest.ValidateModel(mockRequest);

            //assert 
            actualResult.Should().NotBeNull();
            Assert.True(actualResult.Count() > 0);
        }

        [Fact]
        public async Task GivenValidRequestAndVehicleServiceIsDown_WhenApiAddAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            var someEx = new Exception("An error occurred when calling AddAsync method");
            _vehicleServiceMock.Setup(x => x.AddAsync(It.IsAny<VehicleModel>())).ThrowsAsync(someEx);

            // Act
            var vehicleController = new VehicleController(_loggerMock.Object
                , _vehicleServiceMock.Object);
            var actualResult = await vehicleController.AddAsync( It.IsAny<VehicleModel>());
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleController.AddAsync)}...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleController.AddAsync)} method", Times.Once, null);
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            actualResultRespone.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone.Errors.Count() > 0);
        }
        #endregion
        //#region UpdateAsync
        //// Happy case 200
        //[Fact]
        //public async Task GivenValidRequest_WhenApiUpdateAsyncIsCalled_ThenReturnCorrectResult()
        //{
        //    // Arrange
        //    _vehicleServiceMock.Setup(x => x.UpdateAsync(It.IsAny<VehicleModel>())).ReturnsAsync(responseMock);

        //    // Act
        //    var vehicleController = new VehicleController(_loggerMock.Object
        //        , _vehicleServiceMock.Object);
        //    var actualResult = await vehicleController.UpdateAsync( Guid.NewGuid().ToString(), new VehicleModel());
        //    var data = ((OkObjectResult)actualResult).Value as ValidationResult<VehicleModel>;

        //    // Assert
        //    _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleController.UpdateAsync)}...", Times.Once, null);
        //    _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleController.UpdateAsync)} method", Times.Never, null);
        //    actualResult.Should().BeOfType<OkObjectResult>();
        //    ((OkObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
        //    data.Succeeded.Should().BeTrue();
        //    data.Data.Should().NotBeNull();
        //}

        //[Fact]
        //public async Task GivenValidRequestAndNonExistingPriorityVehicleId_WhenApiUpdateAsyncIsCalled_ThenReturnNotFound()
        //{
        //    // Arrange
        //    var request = new PriorityVehicleModel()
        //    {
        //        Id = Guid.NewGuid()
        //    };
        //    responseMock = new ValidationResult<PriorityVehicleModel>(null, new List<ValidationError>()
        //    {
        //        ValidationError.NotFound
        //    });
        //    _priorityVehicleServiceMock.Setup(x => x.UpdateAsync(It.IsAny<PriorityVehicleModel>())).ReturnsAsync(responseMock);

        //    // Act
        //    var priorityVehicleController = new PriorityVehicleController(_loggerMock.Object
        //        , _priorityVehicleServiceMock.Object
        //        , _priorityVehicleImportService.Object
        //        , _vehicleServiceMock.Object
        //        , _vehiclePostService.Object);
        //    var actualResult = await priorityVehicleController.UpdateAsync(stationId, Guid.NewGuid().ToString(), new PriorityVehicleModel());
        //    var data = actualResult as NotFoundObjectResult;
        //    var actualResultRespone = data!.Value as ValidationResult<PriorityVehicleModel>;

        //    // Assert
        //    _priorityVehicleServiceMock.Verify(x => x.UpdateAsync(It.IsAny<PriorityVehicleModel>()), Times.Once);
        //    _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(priorityVehicleController.UpdateAsync)}...", Times.Once, null);
        //    _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(priorityVehicleController.UpdateAsync)} method", Times.Never, null);
        //    ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status404NotFound);
        //    actualResultRespone.Succeeded.Should().BeFalse();
        //    Assert.True(actualResultRespone.Errors.Count(x => x.Code == StatusCodes.Status404NotFound) > 0);
        //}

        //// Unhappy case 400
        //[Fact]
        //public async Task GivenValidRequestAndPriorityVehicleServiceIsDown_WhenApiUpdateAsyncIsCalled_ThenReturnInternalServerError()
        //{
        //    // Arrange
        //    var request = new PriorityVehicleModel()
        //    {
        //        Id = Guid.NewGuid()
        //    };
        //    var someEx = new Exception("An error occurred when calling UpdateAsync method");
        //    _priorityVehicleServiceMock.Setup(x => x.UpdateAsync(It.IsAny<PriorityVehicleModel>())).ThrowsAsync(someEx);

        //    // Act
        //    var priorityVehicleController = new PriorityVehicleController(_loggerMock.Object
        //        , _priorityVehicleServiceMock.Object
        //        , _priorityVehicleImportService.Object
        //        , _vehicleServiceMock.Object
        //        , _vehiclePostService.Object);
        //    var actualResult = await priorityVehicleController.UpdateAsync(stationId, Guid.NewGuid().ToString(), It.IsAny<PriorityVehicleModel>());
        //    var data = ((ObjectResult)actualResult).Value as ValidationResult<SearchResponseModel>;
        //    var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

        //    // Assert
        //    _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(priorityVehicleController.UpdateAsync)}...", Times.Once, null);
        //    _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(priorityVehicleController.UpdateAsync)} method", Times.Once, null);
        //    ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        //    actualResultRespone.Succeeded.Should().BeFalse();
        //    Assert.True(actualResultRespone.Errors.Count() > 0);
        //}
        //#endregion
    }
}
