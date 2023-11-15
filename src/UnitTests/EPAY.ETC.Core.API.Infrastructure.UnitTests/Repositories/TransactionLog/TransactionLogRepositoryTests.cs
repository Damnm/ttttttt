using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Models.TransactionLog;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.TransactionLog;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Repositories.TransactionLog
{
    public class TransactionLogRepositoryTests
    {
        #region Init mock data
        private Mock<CoreDbContext> _dbContextMock = new Mock<CoreDbContext>();
        private Mock<ILogger<LaneInCameraTransactionLogRepository>> _loggerMock = new Mock<ILogger<LaneInCameraTransactionLogRepository>>();
        private Mock<DbSet<LaneInCameraTransactionLog>>? _dbSetMock;
        #endregion

        #region Init Data mock
        private List<LaneInCameraTransactionLog> entities = new List<LaneInCameraTransactionLog>
        {
            new LaneInCameraTransactionLog()
            {
                Id = Guid.NewGuid(),
                CreatedDate = new DateTime(2023,9,11)
            }
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
            _dbContextMock.Setup(x => x.LaneInCameraTransactionLogs).Returns(_dbSetMock.Object);

            // Act
            var repository = new LaneInCameraTransactionLogRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = await repository.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.First().Should().BeEquivalentTo(data);
            _dbContextMock.Verify(x => x.LaneInCameraTransactionLogs, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.GetAllAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.GetAllAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async void GivenRequestIsValidAndExpressionIsExists_WhenGetAllAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var data = entities.FirstOrDefault()!;
            _dbSetMock = EFTestHelper.GetMockDbSet(entities);
            _dbContextMock.Setup(x => x.LaneInCameraTransactionLogs).Returns(_dbSetMock.Object);

            Expression<Func<LaneInCameraTransactionLog, bool>> expression = s => s.Id == data.Id;

            // Act
            var repository = new LaneInCameraTransactionLogRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = await repository.GetAllAsync(expression);

            // Assert
            result.Should().NotBeNull();
            result.First().Should().BeEquivalentTo(data);
            _dbContextMock.Verify(x => x.LaneInCameraTransactionLogs, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.GetAllAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.GetAllAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenRequestIsValidAndDBContextIsDown_WhenGetAllAsyncIsCalled_ThenThrowETCEPAYCoreAPIException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.LaneInCameraTransactionLogs).Throws(someEx);

            // Act
            var repository = new LaneInCameraTransactionLogRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await repository.GetAllAsync();

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.LaneInCameraTransactionLogs, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.GetAllAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.GetAllAsync)} method", Times.Once, _exception);
        }
        #endregion

        #region AddAsync
        [Fact]
        public async void GivenRequestIsValid_WhenAddAsyncIsCalled_ThenRecordAddedSuccessfulAndReturnCorrectResult()
        {
            // Arrange
            var data = entities.FirstOrDefault()!;
            _dbSetMock = EFTestHelper.GetMockDbSet(entities);
            _dbContextMock.Setup(x => x.LaneInCameraTransactionLogs).Returns(_dbSetMock.Object);

            // Act
            var repository = new LaneInCameraTransactionLogRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = await repository.AddAsync(data);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(data);
            _dbContextMock.Verify(x => x.LaneInCameraTransactionLogs, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.AddAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.AddAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenRequestIsValidAndDBContextIsDown_WhenAddAsyncIsCalled_ThenThrowETCEPAYCoreAPIException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.LaneInCameraTransactionLogs).Throws(someEx);

            // Act
            var repository = new LaneInCameraTransactionLogRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await repository.AddAsync(It.IsNotNull<LaneInCameraTransactionLog>());

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.LaneInCameraTransactionLogs, Times.Once);
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
            _dbContextMock.Setup(x => x.LaneInCameraTransactionLogs).Returns(_dbSetMock.Object);

            // Act
            var repository = new LaneInCameraTransactionLogRepository(_loggerMock.Object, _dbContextMock.Object);
            await repository.UpdateAsync(data);

            // Assert
            _dbContextMock.Verify(x => x.LaneInCameraTransactionLogs, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.UpdateAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.UpdateAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenRequestIsValidAndDBContextIsDown_WhenUpdateAsyncIsCalled_ThenThrowETCEPAYCoreAPIException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.LaneInCameraTransactionLogs).Throws(someEx);

            // Act
            var repository = new LaneInCameraTransactionLogRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await repository.UpdateAsync(It.IsNotNull<LaneInCameraTransactionLog>());

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.LaneInCameraTransactionLogs, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.UpdateAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.UpdateAsync)} method", Times.Once, _exception);
        }
        #endregion

    }
}
