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

namespace EPAY.ETC.Core.API.UnitTests.Controllers.TransactionLog
{
    public class LaneInRFIDTransactionLogControllerTests : ControllerBase
    {
        private readonly Exception _exception = null!;
        private Mock<ILaneInRFIDTransactionLogService> _laneInRFIDTransactionLogServiceMock = new Mock<ILaneInRFIDTransactionLogService>();
        private Mock<ILogger<LaneInRFIDTransactionLogController>> _loggerMock = new Mock<ILogger<LaneInRFIDTransactionLogController>>();
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

        #region AddOrUpdateAsync

        [Fact]
        public async Task GivenValidRequest_WhenApiAddOrUpdateAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _laneInRFIDTransactionLogServiceMock.Setup(x => x.AddOrUpdateAsync(It.IsAny<Guid>(), It.IsAny<LaneInRFIDTransactionLogRequestModel>())).ReturnsAsync(ValidationResult.Success(true));
            var request = new VehicleModel();

            // Act
            var vehicleController = new LaneInRFIDTransactionLogController(_loggerMock.Object
                , _laneInRFIDTransactionLogServiceMock.Object);
            var actualResult = await vehicleController.AddOrUpdateAsync(It.IsAny<LaneInRFIDTransactionLogRequestModel>());
            var data = ((ObjectResult)actualResult).Value as ValidationResult<VehicleModel>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleController.AddOrUpdateAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleController.AddOrUpdateAsync)} method", Times.Never, _exception);
            actualResult.Should().BeOfType<ObjectResult>();
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status201Created);
            data?.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async Task GivenValidRequestAndVehicleServiceIsDown_WhenApiAddOrUpdateAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            var someEx = new Exception("An error occurred when calling AddAsync method");
            _laneInRFIDTransactionLogServiceMock.Setup(x => x.AddOrUpdateAsync(It.IsAny<Guid>(), It.IsAny<LaneInRFIDTransactionLogRequestModel>())).ThrowsAsync(someEx);

            // Act
            var vehicleController = new LaneInRFIDTransactionLogController(_loggerMock.Object
                , _laneInRFIDTransactionLogServiceMock.Object);
            var actualResult = await vehicleController.AddOrUpdateAsync(It.IsAny<LaneInRFIDTransactionLogRequestModel>());
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleController.AddOrUpdateAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleController.AddOrUpdateAsync)} method", Times.Once, _exception);
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            actualResultRespone!.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone.Errors.Count() > 0);
        }
        #endregion
    }
}
