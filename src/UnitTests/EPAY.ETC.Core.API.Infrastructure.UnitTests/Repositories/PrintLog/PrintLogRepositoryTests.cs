using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Models.PrintLog;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.PrintLog;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Common;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Repositories.PrintLog
{
    public class PrintLogRepositoryTests : AutoMapperTestBase
    {
        #region Init mock data
        private readonly Exception _exception = null!;
        private Mock<CoreDbContext> _dbContextMock = new Mock<CoreDbContext>();
        private Mock<ILogger<PrintLogRepository>> _loggerMock = new Mock<ILogger<PrintLogRepository>>();
        private Mock<DbSet<PrintLogModel>>? _dbPrintLogSetMock;
        private List<PrintLogModel> _printLogs = new List<PrintLogModel>()
        {
            new PrintLogModel()
            {
                Id = Guid.NewGuid(),
                RFID = "1245asdasda",
                CreatedDate = DateTime.Now,
                PlateNumber = "Some Plate number",
               
            },
            new PrintLogModel()
            {
                Id = Guid.NewGuid(),
                RFID = "1245asdasda",
                CreatedDate = DateTime.Now,
                PlateNumber = "Some Plate number",
               
            },
        };
        #endregion

        #region GetByIdAsync
        [Fact]
        public async void GivenValidRequest_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var data = _printLogs.FirstOrDefault();
            _dbPrintLogSetMock = EFTestHelper.GetMockDbSet(_printLogs);
            _dbContextMock.Setup(x => x.PrintLogs).Returns(_dbPrintLogSetMock.Object);

            // Act
            var PrintLogRepository = new PrintLogRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = await PrintLogRepository.GetByIdAsync(data!.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(data);
            _dbContextMock.Verify(x => x.PrintLogs, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PrintLogRepository.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(PrintLogRepository.GetByIdAsync)} method", Times.Never, _exception);
        }
        [Fact]
        public async void GivenValidRequestAndPrintLogRepositoryIsDown_WhenGetByIdAsyncIsCalled_ThenThowETCEPAYCoreAPIException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.PrintLogs).Throws(someEx);

            // Act
            var PrintLogRepository = new PrintLogRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await PrintLogRepository.GetByIdAsync(It.IsNotNull<Guid>());

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.PrintLogs, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PrintLogRepository.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(PrintLogRepository.GetByIdAsync)} method", Times.Once, _exception);
        }
        #endregion        

        #region AddAsync
        [Fact]
        public async void GivenValidEntity_WhenAddAsyncIsCalled_ThenRecordAddedSuccessfullyAndReturnCorrectResult()
        {
            // Arrange
            var data = _printLogs.FirstOrDefault();
            _dbPrintLogSetMock = EFTestHelper.GetMockDbSet(_printLogs);
            _dbContextMock.Setup(x => x.PrintLogs).Returns(_dbPrintLogSetMock.Object);

            // Act
            var PrintLogRepository = new PrintLogRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = await PrintLogRepository.AddAsync(data!);

            // Assert
            result.Should().NotBeNull().And.BeEquivalentTo(data);
            _dbContextMock.Verify(x => x.PrintLogs, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PrintLogRepository.AddAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(PrintLogRepository.AddAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async void GivenValidEntityAndVehicleRepositotyIsDown_WhenAddAsyncIsCalled_ThenThrowETCEPAYCoreAPIException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.PrintLogs).Throws(someEx);

            // Act
            var PrintLogRepository = new PrintLogRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await PrintLogRepository.AddAsync(It.IsAny<PrintLogModel>());

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.PrintLogs, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PrintLogRepository.AddAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(PrintLogRepository.AddAsync)} method", Times.Once, _exception);
        }
        #endregion

        #region UpdateAsync
        [Fact]
        public async void GivenValidEntity_WhenUpdateAsyncIsCalled_ThenRecordUpdatedSuccessfully()
        {
            // Arrange
            var data = _printLogs.FirstOrDefault();
            _dbPrintLogSetMock = EFTestHelper.GetMockDbSet(_printLogs);
            _dbContextMock.Setup(x => x.PrintLogs).Returns(_dbPrintLogSetMock.Object);

            // Act
            var PrintLogRepository = new PrintLogRepository(_loggerMock.Object, _dbContextMock.Object);
            await PrintLogRepository.UpdateAsync(data!);

            // Assert
            _dbContextMock.Verify(x => x.PrintLogs, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PrintLogRepository.UpdateAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(PrintLogRepository.UpdateAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async void GivenValidEntityAndVehicleRepositotyIsDown_WhenUpdateAsyncIsCalled_ThenThowETCEPAYCoreAPIException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.PrintLogs).Throws(someEx);

            // Act
            var PrintLogRepository = new PrintLogRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await PrintLogRepository.UpdateAsync(It.IsAny<PrintLogModel>());

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.PrintLogs, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PrintLogRepository.UpdateAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(PrintLogRepository.UpdateAsync)} method", Times.Once, _exception);
        }
        #endregion

        #region RemoveAsync
        [Fact]
        public async void GivenValidEntity_WhenRemoveAsyncIsCalled_ThenRecordRemovedSuccessfully()
        {
            // Arrange
            var data = _printLogs.FirstOrDefault();
            _dbPrintLogSetMock = EFTestHelper.GetMockDbSet(_printLogs);
            _dbContextMock.Setup(x => x.PrintLogs).Returns(_dbPrintLogSetMock.Object);

            // Act
            var PrintLogRepository = new PrintLogRepository(_loggerMock.Object, _dbContextMock.Object);
            await PrintLogRepository.RemoveAsync(data!);

            // Assert
            _dbContextMock.Verify(x => x.PrintLogs, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PrintLogRepository.RemoveAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(PrintLogRepository.RemoveAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async void GivenValidEntityAndPriorityPrintLogRepositoryIsDown_WhenRemoveAsyncIsCalled_ThenThrowETCEPAYCoreAPIException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.PrintLogs).Throws(someEx);

            // Act
            var PrintLogRepository = new PrintLogRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await PrintLogRepository.RemoveAsync(It.IsAny<PrintLogModel>());

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.PrintLogs, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PrintLogRepository.RemoveAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(PrintLogRepository.RemoveAsync)} method", Times.Once, _exception);
        }
        #endregion
    }
}