using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.PaymentStatus;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Common;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Helpers;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.Receipt.SessionReports;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentStatusModel = EPAY.ETC.Core.API.Core.Models.PaymentStatus.PaymentStatusModel;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Repositories.PaymentStatus
{
    public class PaymentStatusRepositoryTests : AutoMapperTestBase
    {
        #region Init mock data
        private readonly Exception _exception = null!;
        private Mock<CoreDbContext> _dbContextMock = new Mock<CoreDbContext>();
        private Mock<ILogger<PaymentStatusRepository>> _loggerMock = new Mock<ILogger<PaymentStatusRepository>>();
        private Mock<DbSet<PaymentStatusModel>>? _dbPaymentStatusSetMock;
        private List<PaymentStatusModel> paymentStatuses = new List<PaymentStatusModel>
        {
            new PaymentStatusModel()
            {
                Id = Guid.NewGuid(),
                PaymentId = Guid.NewGuid(),
                Amount = 300,
                Currency ="vnd",
                PaymentMethod = PaymentMethodEnum.RFID,

            }
        };
        #endregion
        #region AddAsync
        [Fact]
        public void GivenValidRequestAndData_WhenAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var data = paymentStatuses.FirstOrDefault();
            _dbPaymentStatusSetMock = EFTestHelper.GetMockDbSet(paymentStatuses);
            _dbContextMock.Setup(x => x.PaymentStatuses).Returns(_dbPaymentStatusSetMock.Object);

            // Act
            var paymentStatusRepository = new PaymentStatusRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = paymentStatusRepository.AddAsync(paymentStatuses.FirstOrDefault()!);

            // Assert
            result.Should().NotBeNull();
            _dbContextMock.Verify(x => x.PaymentStatuses, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(paymentStatusRepository.AddAsync)} method...", Times.Once, _exception);
        }

        [Fact]
        public async void GivenValidRequestButFailedToConnectDatabase_WhenAddAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some  exception");
            _dbContextMock.Setup(x => x.PaymentStatuses).Throws(someEx);

            // Act
            var paymentStatusRepository = new PaymentStatusRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await paymentStatusRepository.AddAsync(paymentStatuses.FirstOrDefault()!);

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.PaymentStatuses, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(paymentStatusRepository.AddAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(paymentStatusRepository.AddAsync)} method", Times.Once, _exception);
        }
        #endregion
        #region GetByIdAsync
        [Fact]
        public async void GivenValidRequest_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var data = paymentStatuses.FirstOrDefault()!;
            _dbPaymentStatusSetMock = EFTestHelper.GetMockDbSet(paymentStatuses);
            _dbContextMock.Setup(x => x.PaymentStatuses).Returns(_dbPaymentStatusSetMock.Object);

            // Act
            var paymentStatusRepository = new PaymentStatusRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = await paymentStatusRepository.GetByIdAsync(data.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(data);
            _dbContextMock.Verify(x => x.PaymentStatuses, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(paymentStatusRepository.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(paymentStatusRepository.GetByIdAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestButFailedToConnectDatabase_WhenGetByIdAsyncIsCalled_ThenThrowExceptionAsync()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.PaymentStatuses).Throws(someEx);

            // Act
            var paymentStatusRepository = new PaymentStatusRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await paymentStatusRepository.GetByIdAsync(It.IsNotNull<Guid>());

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.PaymentStatuses, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(paymentStatusRepository.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(paymentStatusRepository.GetByIdAsync)} method", Times.Once, _exception);
        }
        #endregion
        #region RemoveAsync
        [Fact]
        public async Task GivenValidRequest_WhenRemoveAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _dbPaymentStatusSetMock = EFTestHelper.GetMockDbSet(paymentStatuses);
            _dbContextMock.Setup(x => x.PaymentStatuses).Returns(_dbPaymentStatusSetMock.Object);

            // Act
            var paymentStatusRepository = new PaymentStatusRepository(_loggerMock.Object, _dbContextMock.Object);
            await paymentStatusRepository.RemoveAsync(paymentStatuses.FirstOrDefault()!);

            // Assert
            _dbContextMock.Verify(x => x.PaymentStatuses, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(paymentStatusRepository.RemoveAsync)} method...", Times.Once, _exception);
        }

        [Fact]
        public async void GivenValidRequestButFailedToConnectDatabase_WhenRemoveAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.PaymentStatuses).Throws(someEx);

            // Act
            var paymentStatusRepository = new PaymentStatusRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await paymentStatusRepository.RemoveAsync(It.IsAny<PaymentStatusModel>());

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.PaymentStatuses, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(paymentStatusRepository.RemoveAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(paymentStatusRepository.RemoveAsync)} method", Times.Once, _exception);
        }
        #endregion
        #region UpdateAsync
        [Fact]
        public async Task GivenValidRequest_WhenUpdateAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var data = paymentStatuses.FirstOrDefault();
            _dbPaymentStatusSetMock = EFTestHelper.GetMockDbSet(paymentStatuses);
            _dbContextMock.Setup(x => x.PaymentStatuses).Returns(_dbPaymentStatusSetMock.Object);

            // Act
            var paymentStatusRepository = new PaymentStatusRepository(_loggerMock.Object, _dbContextMock.Object);
            await paymentStatusRepository.UpdateAsync(data!);

            // Assert
            _dbContextMock.Verify(x => x.PaymentStatuses, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(paymentStatusRepository.UpdateAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(paymentStatusRepository.UpdateAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async void GivenValidRequestButFailedToConnectDatabase_WhenUpdateAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some ACVDb exception");
            _dbContextMock.Setup(x => x.PaymentStatuses).Throws(someEx);

            // Act
            var paymentStatusRepository = new PaymentStatusRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await paymentStatusRepository.UpdateAsync(paymentStatuses.FirstOrDefault()!);

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.PaymentStatuses, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(paymentStatusRepository.UpdateAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(paymentStatusRepository.UpdateAsync)} method", Times.Once, _exception);
        }
        #endregion
        #region GetAllWithNavigationAsync
        [Fact]
        public async void GivenValidRequest_WhenGetAllWithNavigationAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var data = paymentStatuses.FirstOrDefault()!;
            _dbPaymentStatusSetMock = EFTestHelper.GetMockDbSet(paymentStatuses);
            _dbContextMock.Setup(x => x.PaymentStatuses).Returns(_dbPaymentStatusSetMock.Object);

            // Act
            var paymentStatusRepository = new PaymentStatusRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = await paymentStatusRepository.GetAllWithNavigationAsync(It.IsAny<LaneSessionReportRequestModel>());

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(1);
            _dbContextMock.Verify(x => x.PaymentStatuses, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(paymentStatusRepository.GetAllWithNavigationAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(paymentStatusRepository.GetAllWithNavigationAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestButFailedToConnectDatabase_WhenGetAllWithNavigationAsyncIsCalled_ThenThrowExceptionAsync()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.PaymentStatuses).Throws(someEx);

            // Act
            var paymentStatusRepository = new PaymentStatusRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await paymentStatusRepository.GetAllWithNavigationAsync(It.IsAny<LaneSessionReportRequestModel>());

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.PaymentStatuses, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(paymentStatusRepository.GetAllWithNavigationAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(paymentStatusRepository.GetAllWithNavigationAsync)} method", Times.Once, _exception);
        }
        #endregion
    }
}
