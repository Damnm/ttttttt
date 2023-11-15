using EPAY.ETC.Core.API.Controllers.TransactionLog;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Transaction;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.UnitTests.Helpers;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace EPAY.ETC.Core.API.UnitTests.Controllers.TransactionLog
{
    public class LaneInCameraTransactionLogControllerTests : ControllerBase
    {
        private readonly Exception _exception = null!;
        private Mock<ILaneInCameraTransactionLogService> _laneInCameraTransactionLogServiceMock = new Mock<ILaneInCameraTransactionLogService>();
        private Mock<ILogger<LaneInCameraTransactionLogController>> _loggerMock = new Mock<ILogger<LaneInCameraTransactionLogController>>();
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

        #region UpdateInsertAsync
       
        [Fact]
        public async Task GivenValidRequest_WhenApiUpdateInsertAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _laneInCameraTransactionLogServiceMock.Setup(x => x.UpdateInsertAsync(It.IsAny<LaneInCameraTransactionLogRequest>())).ReturnsAsync(ValidationResult.Success(true));
            var request = new VehicleModel();

            // Act
            var vehicleController = new LaneInCameraTransactionLogController(_loggerMock.Object
                , _laneInCameraTransactionLogServiceMock.Object);
            var actualResult = await vehicleController.UpdateInsertAsync(It.IsAny<LaneInCameraTransactionLogRequest>());
            var data = ((ObjectResult)actualResult).Value as ValidationResult<VehicleModel>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleController.UpdateInsertAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleController.UpdateInsertAsync)} method", Times.Never, _exception);
            actualResult.Should().BeOfType<ObjectResult>();
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status201Created);
            data?.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async Task GivenValidRequestAndVehicleServiceIsDown_WhenApiUpdateInsertAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            var someEx = new Exception("An error occurred when calling AddAsync method");
            _laneInCameraTransactionLogServiceMock.Setup(x => x.UpdateInsertAsync(It.IsAny<LaneInCameraTransactionLogRequest>())).ThrowsAsync(someEx);

            // Act
            var vehicleController = new LaneInCameraTransactionLogController(_loggerMock.Object
                , _laneInCameraTransactionLogServiceMock.Object);
            var actualResult = await vehicleController.UpdateInsertAsync(It.IsAny<LaneInCameraTransactionLogRequest>());
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleController.UpdateInsertAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleController.UpdateInsertAsync)} method", Times.Once, _exception);
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            actualResultRespone!.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone.Errors.Count() > 0);
        }
        #endregion
    }
}
