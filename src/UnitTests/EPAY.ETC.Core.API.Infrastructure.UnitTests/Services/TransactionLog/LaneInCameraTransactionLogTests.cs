using EPAY.ETC.Core.API.Core.Models.TransactionLog;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.TransactionLog;
using EPAY.ETC.Core.API.Infrastructure.Services.TransactionLog;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Common;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Helpers;
using EPAY.ETC.Core.Models.Request;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Services.TransactionLog
{
    public class LaneInCameraTransactionLogTests : AutoMapperTestBase
    {
        #region Init mock data
        private readonly Exception _exception = null!;
        private readonly Mock<ILogger<LaneInCameraTransactionLogService>> _loggerMock = new();
        private readonly Mock<ILaneInCameraTransactionLogRepository> _repositoryMock = new();
        private List<LaneInCameraTransactionLogRequest> laneInCameraTransactionLog = new List<LaneInCameraTransactionLogRequest>();
       
        private LaneInCameraTransactionLogRequest request =
              new LaneInCameraTransactionLogRequest()
              {
                  PlateNumber = "Some Plate number",
                  RFID = "Some RFID",
                  Make = "Some make",
                  Seat = 10,
                  VehicleType = "Loại 2",
                  Weight = 7000,

              };

        #endregion

        #region UpdateInsertAsync
        [Fact]
        public async Task GivenValidRequest_WhenUpdateInsertAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            // Arrange
            Expression<Func<LaneInCameraTransactionLog, bool>> expression = s => s.Make == string.Empty;
            _repositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<LaneInCameraTransactionLog, bool>>>())).ReturnsAsync(new List<LaneInCameraTransactionLog>());
            _repositoryMock.Setup(x => x.AddAsync(It.IsNotNull<LaneInCameraTransactionLog>())).ReturnsAsync(new LaneInCameraTransactionLog() {
                PlateNumber = "Some Plate number",
                RFID = "Some RFID",
                Make = "Some make",
                Seat = 10,
                VehicleType = "Loại 2",
                Weight = 7000,
            });

            // Act
            var service = new LaneInCameraTransactionLogService(_loggerMock.Object, _repositoryMock.Object, _mapper);
            var result = await service.UpdateInsertAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeTrue();
            result.Succeeded.Should().BeTrue();
            _repositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<LaneInCameraTransactionLog, bool>>>()), Times.Once);
            _repositoryMock.Verify(x => x.AddAsync(It.IsNotNull<LaneInCameraTransactionLog>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.UpdateInsertAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.UpdateInsertAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndRepositoryIsDown_WhenAddAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            _repositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<LaneInCameraTransactionLog, bool>>>())).ThrowsAsync(new Exception());

            // Act
            var service = new LaneInCameraTransactionLogService(_loggerMock.Object, _repositoryMock.Object, _mapper);
            Func<Task> func = () => service.UpdateInsertAsync(request);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            _repositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<LaneInCameraTransactionLog, bool>>>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.UpdateInsertAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.UpdateInsertAsync)} method", Times.Once, _exception);
        }
        #endregion

    }
}
