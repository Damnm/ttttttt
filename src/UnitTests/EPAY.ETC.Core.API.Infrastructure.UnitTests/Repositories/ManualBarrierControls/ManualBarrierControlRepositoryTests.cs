using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Models.ManualBarrierControl;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ManualBarrierControls;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Common;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Helpers;
using EPAY.ETC.Core.Models.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Repositories.ManualBarrierControls
{
    public class ManualBarrierControlRepositoryTests : AutoMapperTestBase
    {
        #region Init mock data
        private readonly Exception _exception = null!;
        private Mock<CoreDbContext> _dbContextMock = new Mock<CoreDbContext>();
        private Mock<ILogger<ManualBarrierControlRepository>> _loggerMock = new Mock<ILogger<ManualBarrierControlRepository>>();
        private Mock<DbSet<ManualBarrierControlModel>>? _dbManualBarrierControlSetMock;
        private List<ManualBarrierControlModel> listManualBarrierControl = new List<ManualBarrierControlModel>
        {
            new ManualBarrierControlModel()
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                EmployeeId = "650d28ea-4e1a-42e3-81a2-dd13a2a5d3c3",
                Action =BarrierActionEnum.Open,
                LaneOutId = "0011",
            }
        };
        #endregion
        #region AddAsync
        [Fact]
        public void GivenValidRequestAndData_WhenAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var data = listManualBarrierControl.FirstOrDefault();
            _dbManualBarrierControlSetMock = EFTestHelper.GetMockDbSet(listManualBarrierControl);
            _dbContextMock.Setup(x => x.ManualBarrierControls).Returns(_dbManualBarrierControlSetMock.Object);

            // Act
            var manualBarrierControlRepository = new ManualBarrierControlRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = manualBarrierControlRepository.AddAsync(listManualBarrierControl.FirstOrDefault()!);

            // Assert
            result.Should().NotBeNull();
            _dbContextMock.Verify(x => x.ManualBarrierControls, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(manualBarrierControlRepository.AddAsync)} method...", Times.Once, _exception);
        }

        [Fact]
        public async void GivenValidRequestButFailedToConnectDatabase_WhenAddAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some  exception");
            _dbContextMock.Setup(x => x.ManualBarrierControls).Throws(someEx);

            // Act
            var manualBarrierControlRepository = new ManualBarrierControlRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await manualBarrierControlRepository.AddAsync(listManualBarrierControl.FirstOrDefault()!);

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.ManualBarrierControls, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(manualBarrierControlRepository.AddAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(manualBarrierControlRepository.AddAsync)} method", Times.Once, _exception);
        }
        #endregion
        #region GetByIdAsync
        [Fact]
        public async void GivenValidRequest_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var data = listManualBarrierControl.FirstOrDefault()!;
            _dbManualBarrierControlSetMock = EFTestHelper.GetMockDbSet(listManualBarrierControl);
            _dbContextMock.Setup(x => x.ManualBarrierControls).Returns(_dbManualBarrierControlSetMock.Object);

            // Act
            var manualBarrierControlRepository = new ManualBarrierControlRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = await manualBarrierControlRepository.GetByIdAsync(data.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(data);
            _dbContextMock.Verify(x => x.ManualBarrierControls, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(manualBarrierControlRepository.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(manualBarrierControlRepository.GetByIdAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestButFailedToConnectDatabase_WhenGetByIdAsyncIsCalled_ThenThrowExceptionAsync()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.ManualBarrierControls).Throws(someEx);

            // Act
            var manualBarrierControlRepository = new ManualBarrierControlRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await manualBarrierControlRepository.GetByIdAsync(It.IsNotNull<Guid>());

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.ManualBarrierControls, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(manualBarrierControlRepository.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(manualBarrierControlRepository.GetByIdAsync)} method", Times.Once, _exception);
        }
        #endregion
        #region RemoveAsync
        [Fact]
        public async Task GivenValidRequest_WhenRemoveAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _dbManualBarrierControlSetMock = EFTestHelper.GetMockDbSet(listManualBarrierControl);
            _dbContextMock.Setup(x => x.ManualBarrierControls).Returns(_dbManualBarrierControlSetMock.Object);

            // Act
            var manualBarrierControlRepository = new ManualBarrierControlRepository(_loggerMock.Object, _dbContextMock.Object);
            await manualBarrierControlRepository.RemoveAsync(listManualBarrierControl.FirstOrDefault()!);

            // Assert
            _dbContextMock.Verify(x => x.ManualBarrierControls, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(manualBarrierControlRepository.RemoveAsync)} method...", Times.Once, _exception);
        }

        [Fact]
        public async void GivenValidRequestButFailedToConnectDatabase_WhenRemoveAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.ManualBarrierControls).Throws(someEx);

            // Act
            var manualBarrierControlRepository = new ManualBarrierControlRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await manualBarrierControlRepository.RemoveAsync(It.IsAny<ManualBarrierControlModel>());

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.ManualBarrierControls, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(manualBarrierControlRepository.RemoveAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(manualBarrierControlRepository.RemoveAsync)} method", Times.Once, _exception);
        }
        #endregion
        #region UpdateAsync
        [Fact]
        public async Task GivenValidRequest_WhenUpdateAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var data = listManualBarrierControl.FirstOrDefault();
            _dbManualBarrierControlSetMock = EFTestHelper.GetMockDbSet(listManualBarrierControl);
            _dbContextMock.Setup(x => x.ManualBarrierControls).Returns(_dbManualBarrierControlSetMock.Object);

            // Act
            var manualBarrierControlRepository = new ManualBarrierControlRepository(_loggerMock.Object, _dbContextMock.Object);
            await manualBarrierControlRepository.UpdateAsync(data!);

            // Assert
            _dbContextMock.Verify(x => x.ManualBarrierControls, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(manualBarrierControlRepository.UpdateAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(manualBarrierControlRepository.UpdateAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async void GivenValidRequestButFailedToConnectDatabase_WhenUpdateAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some ACVDb exception");
            _dbContextMock.Setup(x => x.ManualBarrierControls).Throws(someEx);

            // Act
            var manualBarrierControlRepository = new ManualBarrierControlRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await manualBarrierControlRepository.UpdateAsync(listManualBarrierControl.FirstOrDefault()!);

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.ManualBarrierControls, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(manualBarrierControlRepository.UpdateAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(manualBarrierControlRepository.UpdateAsync)} method", Times.Once, _exception);
        }
        #endregion
    }
}
