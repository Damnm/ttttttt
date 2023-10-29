using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Models.Fusion;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Fusion;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Vehicle;
using EPAY.ETC.Core.API.Infrastructure.Services.Fusion;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Common;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Repositories.Fusion
{
    public class FusionRepositoryTests : AutoMapperTestBase
    {
        #region Init mock data
        private readonly Exception _exception = null!;
        private Mock<CoreDbContext> _dbContextMock = new Mock<CoreDbContext>();
        private Mock<ILogger<FusionRepository>> _loggerMock = new Mock<ILogger<FusionRepository>>();
        private Mock<DbSet<FusionModel>>? _dbFusionSetMock;
        private List<FusionModel> listFusion = new List<FusionModel>
        {
            new FusionModel()
            {
                Id = Guid.NewGuid(),
                Loop1 = true,
                RFID = "Some RFID",
                ANPRCam1 = "12A12345",
                Loop2 = true,
                CCTVCam2 = "12A12345",
                Loop3 = true,
                ReversedLoop1 = true,
                ReversedLoop2 = true,
            }
        };
        #endregion
        #region AddAsync
        [Fact]
        public  void GivenValidRequestAndData_WhenAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var data = listFusion.FirstOrDefault();
            _dbFusionSetMock = EFTestHelper.GetMockDbSet(listFusion);
            _dbContextMock.Setup(x => x.Fusions).Returns(_dbFusionSetMock.Object);

            // Act
            var fusionsRepository = new FusionRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = fusionsRepository.AddAsync(listFusion.FirstOrDefault()!);

            // Assert
            result.Should().NotBeNull();
            _dbContextMock.Verify(x => x.Fusions, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(fusionsRepository.AddAsync)} method...", Times.Once, _exception);
        }

        [Fact]
        public async void GivenValidRequestButFailedToConnectDatabase_WhenAddAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some  exception");
            _dbContextMock.Setup(x => x.Fusions).Throws(someEx);

            // Act
            var fusionsRepository = new FusionRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await fusionsRepository.AddAsync(listFusion.FirstOrDefault()!);

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.Fusions, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(fusionsRepository.AddAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(fusionsRepository.AddAsync)} method", Times.Once, _exception);
        }
        #endregion
        #region GetByIdAsync
        [Fact]
        public async void GivenValidRequest_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var data = listFusion.FirstOrDefault()!;
            _dbFusionSetMock = EFTestHelper.GetMockDbSet(listFusion);
            _dbContextMock.Setup(x => x.Fusions).Returns(_dbFusionSetMock.Object);

            // Act
            var fusionsRepository = new FusionRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = await fusionsRepository.GetByIdAsync(data.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(data);
            _dbContextMock.Verify(x => x.Fusions, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(fusionsRepository.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(fusionsRepository.GetByIdAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestButFailedToConnectDatabase_WhenGetByIdAsyncIsCalled_ThenThrowExceptionAsync()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.Fusions).Throws(someEx);

            // Act
            var fusionsRepository = new FusionRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await fusionsRepository.GetByIdAsync(It.IsNotNull<Guid>());

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.Fusions, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(fusionsRepository.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(fusionsRepository.GetByIdAsync)} method", Times.Once, _exception);
        }
        #endregion
        #region RemoveAsync
        [Fact]
        public async Task GivenValidRequest_WhenRemoveAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _dbFusionSetMock = EFTestHelper.GetMockDbSet(listFusion);
            _dbContextMock.Setup(x => x.Fusions).Returns(_dbFusionSetMock.Object);

            // Act
            var fusionsRepository = new FusionRepository(_loggerMock.Object, _dbContextMock.Object);
            await fusionsRepository.RemoveAsync(listFusion.FirstOrDefault()!);

            // Assert
            _dbContextMock.Verify(x => x.Fusions, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(fusionsRepository.RemoveAsync)} method...", Times.Once, _exception);
        }

        [Fact]
        public async void GivenValidRequestButFailedToConnectDatabase_WhenRemoveAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.Fusions).Throws(someEx);

            // Act
            var fusionsRepository = new FusionRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await fusionsRepository.RemoveAsync(It.IsAny<FusionModel>());

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.Fusions, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(fusionsRepository.RemoveAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(fusionsRepository.RemoveAsync)} method", Times.Once,_exception);
        }
        #endregion
        #region UpdateAsync
        [Fact]
        public async Task GivenValidRequest_WhenUpdateAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var data = listFusion.FirstOrDefault();
            _dbFusionSetMock = EFTestHelper.GetMockDbSet(listFusion);
            _dbContextMock.Setup(x => x.Fusions).Returns(_dbFusionSetMock.Object);

            // Act
            var fusionsRepository = new FusionRepository(_loggerMock.Object, _dbContextMock.Object);
            await fusionsRepository.UpdateAsync(data!);

            // Assert
            _dbContextMock.Verify(x => x.Fusions, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(fusionsRepository.UpdateAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(fusionsRepository.UpdateAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async void GivenValidRequestButFailedToConnectDatabase_WhenUpdateAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some ACVDb exception");
            _dbContextMock.Setup(x => x.Fusions).Throws(someEx);

            // Act
            var fusionsRepository = new FusionRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await fusionsRepository.UpdateAsync(listFusion.FirstOrDefault()!);

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.Fusions, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(fusionsRepository.UpdateAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(fusionsRepository.UpdateAsync)} method", Times.Once, _exception);
        }
        #endregion
    }
}
