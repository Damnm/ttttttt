using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Models.Barcode;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Barcode;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Common;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Repositories.Barcode
{
    public class BarcodeRepositoryTests: AutoMapperTestBase
    {
        #region Init mock data
        private readonly Exception _exception = null!;
        private Mock<CoreDbContext> _dbContextMock = new Mock<CoreDbContext>();
        private Mock<ILogger<BarcodeRepository>> _loggerMock = new Mock<ILogger<BarcodeRepository>>();
        private Mock<DbSet<BarcodeModel>>? _dbBarcodeSetMock;
        private List<BarcodeModel> paymentStatuses = new List<BarcodeModel>
        {
            new BarcodeModel()
            {
                Id = Guid.NewGuid(),
                 ActionCode = "111",
                 ActionDesc = "Open Barrier",
                 EmployeeId = "23232"
            }
        };
        #endregion
        #region AddAsync
        [Fact]
        public void GivenValidRequestAndData_WhenAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var data = paymentStatuses.FirstOrDefault();
            _dbBarcodeSetMock = EFTestHelper.GetMockDbSet(paymentStatuses);
            _dbContextMock.Setup(x => x.Barcodes).Returns(_dbBarcodeSetMock.Object);

            // Act
            var barcodeRepository = new BarcodeRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = barcodeRepository.AddAsync(paymentStatuses.FirstOrDefault()!);

            // Assert
            result.Should().NotBeNull();
            _dbContextMock.Verify(x => x.Barcodes, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(BarcodeRepository.AddAsync)} method...", Times.Once, _exception);
        }

        [Fact]
        public async void GivenValidRequestButFailedToConnectDatabase_WhenAddAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some  exception");
            _dbContextMock.Setup(x => x.Barcodes).Throws(someEx);

            // Act
            var barcodeRepository = new BarcodeRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await barcodeRepository.AddAsync(paymentStatuses.FirstOrDefault()!);

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.Barcodes, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(BarcodeRepository.AddAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(BarcodeRepository.AddAsync)} method", Times.Once, _exception);
        }
        #endregion
        #region GetByIdAsync
        [Fact]
        public async void GivenValidRequest_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var data = paymentStatuses.FirstOrDefault()!;
            _dbBarcodeSetMock = EFTestHelper.GetMockDbSet(paymentStatuses);
            _dbContextMock.Setup(x => x.Barcodes).Returns(_dbBarcodeSetMock.Object);

            // Act
            var barcodeRepository = new BarcodeRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = await barcodeRepository.GetByIdAsync(data.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(data);
            _dbContextMock.Verify(x => x.Barcodes, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(BarcodeRepository.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(BarcodeRepository.GetByIdAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestButFailedToConnectDatabase_WhenGetByIdAsyncIsCalled_ThenThrowExceptionAsync()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.Barcodes).Throws(someEx);

            // Act
            var barcodeRepository = new BarcodeRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await barcodeRepository.GetByIdAsync(It.IsNotNull<Guid>());

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.Barcodes, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(BarcodeRepository.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(BarcodeRepository.GetByIdAsync)} method", Times.Once, _exception);
        }
        #endregion
        #region RemoveAsync
        [Fact]
        public async Task GivenValidRequest_WhenRemoveAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _dbBarcodeSetMock = EFTestHelper.GetMockDbSet(paymentStatuses);
            _dbContextMock.Setup(x => x.Barcodes).Returns(_dbBarcodeSetMock.Object);

            // Act
            var barcodeRepository = new BarcodeRepository(_loggerMock.Object, _dbContextMock.Object);
            await barcodeRepository.RemoveAsync(paymentStatuses.FirstOrDefault()!);

            // Assert
            _dbContextMock.Verify(x => x.Barcodes, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(BarcodeRepository.RemoveAsync)} method...", Times.Once, _exception);
        }

        [Fact]
        public async void GivenValidRequestButFailedToConnectDatabase_WhenRemoveAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.Barcodes).Throws(someEx);

            // Act
            var barcodeRepository = new BarcodeRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await barcodeRepository.RemoveAsync(It.IsAny<BarcodeModel>());

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.Barcodes, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(BarcodeRepository.RemoveAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(BarcodeRepository.RemoveAsync)} method", Times.Once, _exception);
        }
        #endregion
        #region UpdateAsync
        [Fact]
        public async Task GivenValidRequest_WhenUpdateAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var data = paymentStatuses.FirstOrDefault();
            _dbBarcodeSetMock = EFTestHelper.GetMockDbSet(paymentStatuses);
            _dbContextMock.Setup(x => x.Barcodes).Returns(_dbBarcodeSetMock.Object);

            // Act
            var barcodeRepository = new BarcodeRepository(_loggerMock.Object, _dbContextMock.Object);
            await barcodeRepository.UpdateAsync(data!);

            // Assert
            _dbContextMock.Verify(x => x.Barcodes, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(BarcodeRepository.UpdateAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(BarcodeRepository.UpdateAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async void GivenValidRequestButFailedToConnectDatabase_WhenUpdateAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some ACVDb exception");
            _dbContextMock.Setup(x => x.Barcodes).Throws(someEx);

            // Act
            var barcodeRepository = new BarcodeRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await barcodeRepository.UpdateAsync(paymentStatuses.FirstOrDefault()!);

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.Barcodes, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(BarcodeRepository.UpdateAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(BarcodeRepository.UpdateAsync)} method", Times.Once, _exception);
        }
        #endregion
       
    }
}
