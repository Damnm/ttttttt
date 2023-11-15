using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Models.TransactionLog;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.TransactionLog;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Helpers;
using EPAY.ETC.Core.Models.Utils;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Repositories.ETCCheckouts
{
    public class LaneInRFIDTransactionLogRepositoryTests
    {
        #region Init mock data
        private Mock<CoreDbContext> _dbContextMock = new Mock<CoreDbContext>();
        private Mock<ILogger<LaneInRFIDTransactionLogRepository>> _loggerMock = new Mock<ILogger<LaneInRFIDTransactionLogRepository>>();
        private Mock<DbSet<LaneInRFIDTransactionLog>>? _dbSetMock;
        #endregion

        #region Init Data mock
        private List<LaneInRFIDTransactionLog> entities = new List<LaneInRFIDTransactionLog>
        {
            new LaneInRFIDTransactionLog()
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
,            }
        };
        private readonly Exception _exception = null!;
        #endregion

        #region GetAllAsync
        [Fact]
        public async void GivenRequestIsValid_WhenGetAllAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var data = entities.FirstOrDefault()!;
            _dbSetMock = EFTestHelper.GetMockDbSet(entities);
            _dbContextMock.Setup(x => x.LaneInRFIDTransactionLogs).Returns(_dbSetMock.Object);

            // Act
            var repository = new LaneInRFIDTransactionLogRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = await repository.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.First().Should().BeEquivalentTo(data);
            _dbContextMock.Verify(x => x.LaneInRFIDTransactionLogs, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.GetAllAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.GetAllAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async void GivenRequestIsValidAndExpressionIsExists_WhenGetAllAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var data = entities.FirstOrDefault()!;
            _dbSetMock = EFTestHelper.GetMockDbSet(entities);
            _dbContextMock.Setup(x => x.LaneInRFIDTransactionLogs).Returns(_dbSetMock.Object);

            Expression<Func<LaneInRFIDTransactionLog, bool>> expression = s => s.Id == data.Id;

            // Act
            var repository = new LaneInRFIDTransactionLogRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = await repository.GetAllAsync(expression);

            // Assert
            result.Should().NotBeNull();
            result.First().Should().BeEquivalentTo(data);
            _dbContextMock.Verify(x => x.LaneInRFIDTransactionLogs, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.GetAllAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.GetAllAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenRequestIsValidAndDBContextIsDown_WhenGetAllAsyncIsCalled_ThenThrowETCEPAYCoreAPIException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.LaneInRFIDTransactionLogs).Throws(someEx);

            // Act
            var repository = new LaneInRFIDTransactionLogRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await repository.GetAllAsync();

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.LaneInRFIDTransactionLogs, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.GetAllAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.GetAllAsync)} method", Times.Once, _exception);
        }
        #endregion

        #region GetByIdAsync
        [Fact]
        public async void GivenRequestIsValid_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var data = entities.FirstOrDefault()!;
            _dbSetMock = EFTestHelper.GetMockDbSet(entities);
            _dbContextMock.Setup(x => x.LaneInRFIDTransactionLogs).Returns(_dbSetMock.Object);

            // Act
            var repository = new LaneInRFIDTransactionLogRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = await repository.GetByIdAsync(data.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(data);
            _dbContextMock.Verify(x => x.LaneInRFIDTransactionLogs, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.GetByIdAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async void GivenRequestIsValidAndIdIsNotExists_WhenGetByIdAsyncIsCalled_ThenReturnNull()
        {
            // Arrange
            var data = entities.FirstOrDefault()!;
            _dbSetMock = EFTestHelper.GetMockDbSet(new List<LaneInRFIDTransactionLog>());
            _dbContextMock.Setup(x => x.LaneInRFIDTransactionLogs).Returns(_dbSetMock.Object);

            // Act
            var repository = new LaneInRFIDTransactionLogRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = await repository.GetByIdAsync(data.Id);

            // Assert
            result.Should().BeNull();
            _dbContextMock.Verify(x => x.LaneInRFIDTransactionLogs, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.GetByIdAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenRequestIsValidAndDBContextIsDown_WhenGetByIdAsyncIsCalled_ThenThrowETCEPAYCoreAPIException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.LaneInRFIDTransactionLogs).Throws(someEx);

            // Act
            var repository = new LaneInRFIDTransactionLogRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await repository.GetByIdAsync(It.IsNotNull<Guid>());

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.LaneInRFIDTransactionLogs, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.GetByIdAsync)} method", Times.Once, _exception);
        }
        #endregion

        #region AddAsync
        [Fact]
        public async void GivenRequestIsValid_WhenAddAsyncIsCalled_ThenRecordAddedSuccessfulAndReturnCorrectResult()
        {
            // Arrange
            var data = entities.FirstOrDefault()!;
            _dbSetMock = EFTestHelper.GetMockDbSet(entities);
            _dbContextMock.Setup(x => x.LaneInRFIDTransactionLogs).Returns(_dbSetMock.Object);

            // Act
            var repository = new LaneInRFIDTransactionLogRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = await repository.AddAsync(data);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(data);
            _dbContextMock.Verify(x => x.LaneInRFIDTransactionLogs, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.AddAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.AddAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenRequestIsValidAndDBContextIsDown_WhenAddAsyncIsCalled_ThenThrowETCEPAYCoreAPIException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.LaneInRFIDTransactionLogs).Throws(someEx);

            // Act
            var repository = new LaneInRFIDTransactionLogRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await repository.AddAsync(It.IsNotNull<LaneInRFIDTransactionLog>());

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.LaneInRFIDTransactionLogs, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.AddAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.AddAsync)} method", Times.Once, _exception);
        }
        #endregion

        #region UpdateAsync
        [Fact]
        public async void GivenRequestIsValid_WhenUpdateAsyncIsCalled_ThenRecordUpdatedSuccessful()
        {
            // Arrange
            var data = entities.FirstOrDefault()!;
            _dbSetMock = EFTestHelper.GetMockDbSet(entities);
            _dbContextMock.Setup(x => x.LaneInRFIDTransactionLogs).Returns(_dbSetMock.Object);

            // Act
            var repository = new LaneInRFIDTransactionLogRepository(_loggerMock.Object, _dbContextMock.Object);
            await repository.UpdateAsync(data);

            // Assert
            _dbContextMock.Verify(x => x.LaneInRFIDTransactionLogs, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.UpdateAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.UpdateAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenRequestIsValidAndDBContextIsDown_WhenUpdateAsyncIsCalled_ThenThrowETCEPAYCoreAPIException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.LaneInRFIDTransactionLogs).Throws(someEx);

            // Act
            var repository = new LaneInRFIDTransactionLogRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await repository.UpdateAsync(It.IsNotNull<LaneInRFIDTransactionLog>());

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.LaneInRFIDTransactionLogs, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.UpdateAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.UpdateAsync)} method", Times.Once, _exception);
        }
        #endregion
    }
}
