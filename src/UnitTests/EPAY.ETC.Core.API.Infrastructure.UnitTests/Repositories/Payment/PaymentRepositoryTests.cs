using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Payment;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Common;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Helpers;
using EPAY.ETC.Core.Models.Fees.PaidVehicleHistory;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentModel = EPAY.ETC.Core.API.Core.Models.Payment.PaymentModel;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Repositories.PaymentStatus
{
    public class PaymentRepositoryTests : AutoMapperTestBase
    {
        #region Init mock data
        private readonly Exception _exception = null!;
        private Mock<CoreDbContext> _dbContextMock = new Mock<CoreDbContext>();
        private Mock<ILogger<PaymentRepository>> _loggerMock = new Mock<ILogger<PaymentRepository>>();
        private Mock<DbSet<PaymentModel>>? _dbPaymentSetMock;
        private List<PaymentModel> payments = new List<PaymentModel>
        {
            new PaymentModel()
            {
                Id = Guid.NewGuid(),
                LaneInId = "1",
                LaneOutId = "1",
                RFID = "dfsdfdsfds",
                Make = "Toyota",
                Amount = 300

            }
        };
        #endregion

        #region AddAsync
        [Fact]
        public  void GivenValidRequestAndData_WhenAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var data = payments.FirstOrDefault();
            _dbPaymentSetMock = EFTestHelper.GetMockDbSet(payments);
            _dbContextMock.Setup(x => x.Payments).Returns(_dbPaymentSetMock.Object);

            // Act
            var PaymentRepository = new PaymentRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = PaymentRepository.AddAsync(payments.FirstOrDefault()!);

            // Assert
            result.Should().NotBeNull();
            _dbContextMock.Verify(x => x.Payments, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PaymentRepository.AddAsync)} method...", Times.Once, _exception);
        }

        [Fact]
        public async void GivenValidRequestButFailedToConnectDatabase_WhenAddAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some  exception");
            _dbContextMock.Setup(x => x.Payments).Throws(someEx);

            // Act
            var PaymentRepository = new PaymentRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await PaymentRepository.AddAsync(payments.FirstOrDefault()!);

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.Payments, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PaymentRepository.AddAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(PaymentRepository.AddAsync)} method", Times.Once, _exception);
        }
        #endregion

        #region GetByIdAsync
        [Fact]
        public async void GivenValidRequest_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var data = payments.FirstOrDefault()!;
            _dbPaymentSetMock = EFTestHelper.GetMockDbSet(payments);
            _dbContextMock.Setup(x => x.Payments).Returns(_dbPaymentSetMock.Object);

            // Act
            var PaymentRepository = new PaymentRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = await PaymentRepository.GetByIdAsync(data.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(data);
            _dbContextMock.Verify(x => x.Payments, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PaymentRepository.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(PaymentRepository.GetByIdAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestButFailedToConnectDatabase_WhenGetByIdAsyncIsCalled_ThenThrowExceptionAsync()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.Payments).Throws(someEx);

            // Act
            var PaymentRepository = new PaymentRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await PaymentRepository.GetByIdAsync(It.IsNotNull<Guid>());

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.Payments, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PaymentRepository.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(PaymentRepository.GetByIdAsync)} method", Times.Once, _exception);
        }
        #endregion

        #region RemoveAsync
        [Fact]
        public async Task GivenValidRequest_WhenRemoveAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _dbPaymentSetMock = EFTestHelper.GetMockDbSet(payments);
            _dbContextMock.Setup(x => x.Payments).Returns(_dbPaymentSetMock.Object);

            // Act
            var PaymentRepository = new PaymentRepository(_loggerMock.Object, _dbContextMock.Object);
            await PaymentRepository.RemoveAsync(payments.FirstOrDefault()!);

            // Assert
            _dbContextMock.Verify(x => x.Payments, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PaymentRepository.RemoveAsync)} method...", Times.Once, _exception);
        }

        [Fact]
        public async void GivenValidRequestButFailedToConnectDatabase_WhenRemoveAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.Payments).Throws(someEx);

            // Act
            var PaymentRepository = new PaymentRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await PaymentRepository.RemoveAsync(It.IsAny<PaymentModel>());

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.Payments, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PaymentRepository.RemoveAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(PaymentRepository.RemoveAsync)} method", Times.Once,_exception);
        }
        #endregion

        #region UpdateAsync
        [Fact]
        public async Task GivenValidRequest_WhenUpdateAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var data = payments.FirstOrDefault();
            _dbPaymentSetMock = EFTestHelper.GetMockDbSet(payments);
            _dbContextMock.Setup(x => x.Payments).Returns(_dbPaymentSetMock.Object);

            // Act
            var PaymentRepository = new PaymentRepository(_loggerMock.Object, _dbContextMock.Object);
            await PaymentRepository.UpdateAsync(data!);

            // Assert
            _dbContextMock.Verify(x => x.Payments, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PaymentRepository.UpdateAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(PaymentRepository.UpdateAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async void GivenValidRequestButFailedToConnectDatabase_WhenUpdateAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some ACVDb exception");
            _dbContextMock.Setup(x => x.Payments).Throws(someEx);

            // Act
            var PaymentRepository = new PaymentRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await PaymentRepository.UpdateAsync(payments.FirstOrDefault()!);

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.Payments, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PaymentRepository.UpdateAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(PaymentRepository.UpdateAsync)} method", Times.Once, _exception);
        }
        #endregion
    }
}
