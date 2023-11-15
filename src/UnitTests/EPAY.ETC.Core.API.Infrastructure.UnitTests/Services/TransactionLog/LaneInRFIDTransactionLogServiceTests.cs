using EPAY.ETC.Core.API.Core.Models.ETCCheckOuts;
using EPAY.ETC.Core.API.Core.Models.TransactionLog;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.TransactionLog;
using EPAY.ETC.Core.API.Infrastructure.Services.TransactionLog;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Common;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Helpers;
using EPAY.ETC.Core.Models.Fees;
using EPAY.ETC.Core.Models.Utils;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Services.ETCCheckouts
{
    public class LaneInRFIDTransactionLogServiceTests : AutoMapperTestBase
    {
        #region Init mock instance
        private readonly Mock<ILogger<LaneInRFIDTransactionLogService>> _loggerMock = new();
        private readonly Mock<ILaneInRFIDTransactionLogRepository> _laneInRFIDTransactionLogServiceMock = new();
        #endregion

        #region Init test data
        private LaneInVehicleModel request = new LaneInVehicleModel()
        {
            Epoch = new DateTime(2023, 9, 11).ToUnixTime(),
            RFID = "0123456789",
            LaneInId = "1",
            Device = new ETC.Core.Models.Devices.DeviceModel()
            {
                IpAddr = "AAA:BBB:CCC",
                MacAddr = "127.0.0.1",
            },
            VehicleInfo = new ETC.Core.Models.VehicleInfoModel()
            {
                ConfidenceScore = 0.9,
                Make = "Some",
                Model = "Some",
                PlateColour = "Some",
                PlateNumber = "21A12345",
                PlateNumberPhotoUrl = "Some Url",
                PlateNumberRearPhotoUrl = "Some Url",
                RearPlateColour = "Some",
                RearPlateNumber = "21A12345",
                Seat = 7,
                VehiclePhotoUrl = "Some Url",
                VehicleRearPhotoUrl = "Some Url",
                VehicleType = "Some"
            }
        };

        private LaneInRFIDTransactionLog laneInRFIDTransactionLog = new LaneInRFIDTransactionLog()
        {
            Id = Guid.Parse("5eac112b-3e80-487a-a736-8ff584e8b722"),
            CreatedDate = new DateTime(2023, 9, 11),
            ConfidenceScore = 0.9,
            Epoch = new DateTime(2023, 9, 11).ToUnixTime(),
            LaneInId = "1",
            Make = "Some",
            Model = "Some",
            PlateColour = "Some",
            PlateNumber = "21A12345",
            PlateNumberPhotoUrl = "Some Url",
            PlateNumberRearPhotoUrl = "Some Url",
            RearPlateColour = "Some",
            RearPlateNumber = "21A12345",
            RFID = "0123456789",
            RFIDReaderIPAddr = "AAA:BBB:CCC",
            RFIDReaderMacAddr = "127.0.0.1",
            Seat = 7,
            VehiclePhotoUrl = "Some Url",
            VehicleRearPhotoUrl = "Some Url",
            VehicleType = "Some"
,
        };
        private Exception _exception = null!;
        #endregion

        #region AddAsync
        [Fact]
        public async Task GivenValidRequest_WhenAddOrUpdateAsyncIsCalled_ThenAddedRecordSuccessfulAndReturnCorrectResult()
        {
            // Arrange
            _laneInRFIDTransactionLogServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((LaneInRFIDTransactionLog)null!);
            _laneInRFIDTransactionLogServiceMock.Setup(x => x.UpdateAsync(It.IsAny<LaneInRFIDTransactionLog>()));
            _laneInRFIDTransactionLogServiceMock.Setup(x => x.AddAsync(It.IsAny<LaneInRFIDTransactionLog>())).ReturnsAsync(laneInRFIDTransactionLog);

            // Act
            var service = new LaneInRFIDTransactionLogService(_loggerMock.Object, _laneInRFIDTransactionLogServiceMock.Object, _mapper);
            var result = await service.AddOrUpdateAsync(laneInRFIDTransactionLog.Id, request);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeTrue();

            _laneInRFIDTransactionLogServiceMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _laneInRFIDTransactionLogServiceMock.Verify(x => x.UpdateAsync(It.IsAny<LaneInRFIDTransactionLog>()), Times.Never);
            _laneInRFIDTransactionLogServiceMock.Verify(x => x.AddAsync(It.IsAny<LaneInRFIDTransactionLog>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.AddOrUpdateAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.AddOrUpdateAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndIdIsExists_WhenAddAsyncIsCalled_ThenReturnBadRequest()
        {
            // Arrange
            _laneInRFIDTransactionLogServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(laneInRFIDTransactionLog);
            _laneInRFIDTransactionLogServiceMock.Setup(x => x.UpdateAsync(It.IsAny<LaneInRFIDTransactionLog>()));
            _laneInRFIDTransactionLogServiceMock.Setup(x => x.AddAsync(It.IsAny<LaneInRFIDTransactionLog>())).ReturnsAsync(laneInRFIDTransactionLog);

            // Act
            var service = new LaneInRFIDTransactionLogService(_loggerMock.Object, _laneInRFIDTransactionLogServiceMock.Object, _mapper);
            var result = await service.AddOrUpdateAsync(laneInRFIDTransactionLog.Id, request);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Data.Should().BeTrue();

            _laneInRFIDTransactionLogServiceMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _laneInRFIDTransactionLogServiceMock.Verify(x => x.UpdateAsync(It.IsAny<LaneInRFIDTransactionLog>()), Times.Once);
            _laneInRFIDTransactionLogServiceMock.Verify(x => x.AddAsync(It.IsAny<LaneInRFIDTransactionLog>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.AddOrUpdateAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.AddOrUpdateAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndPaymentRepositoryIsDown_WhenAddAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var exception = new Exception("Some ex");
            _laneInRFIDTransactionLogServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(exception);

            // Act
            var service = new LaneInRFIDTransactionLogService(_loggerMock.Object, _laneInRFIDTransactionLogServiceMock.Object, _mapper);
            Func<Task> func = async () => await service.AddOrUpdateAsync(laneInRFIDTransactionLog.Id, request);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => func());
            ex.Message.Should().Be(exception.Message);

            _laneInRFIDTransactionLogServiceMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _laneInRFIDTransactionLogServiceMock.Verify(x => x.UpdateAsync(It.IsAny<LaneInRFIDTransactionLog>()), Times.Never);
            _laneInRFIDTransactionLogServiceMock.Verify(x => x.AddAsync(It.IsAny<LaneInRFIDTransactionLog>()), Times.Never);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.AddOrUpdateAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.AddOrUpdateAsync)} method", Times.Once, _exception);
        }
        #endregion
    }
}
