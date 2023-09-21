using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Payment;
using EPAY.ETC.Core.API.Infrastructure.Services.Payment;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Common;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Helpers;
using EPAY.ETC.Core.Models.Request;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentModel = EPAY.ETC.Core.API.Core.Models.Payment.PaymentModel;
namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Services.Payment
{
    public class PaymentServiceTests : AutoMapperTestBase
    {
        #region Init mock data
        private readonly Exception _exception = null!;
        private readonly Mock<ILogger<PaymentService>> _loggerMock = new();
        private readonly Mock<IPaymentRepository> _paymentRepositoryMock = new();
        private static Guid id = Guid.NewGuid();
        private PaymentAddRequestModel addRequest = new PaymentAddRequestModel()
        {
            PaymentId = id,
            LaneInId = "1",
            LaneOutId = "1",
            RFID = "dfsdfdsfds",
            Make = "Toyota",
            Amount = 300
        };
        private PaymentUpdateRequestModel updateRequest = new PaymentUpdateRequestModel()
        {
            PaymentId = Guid.NewGuid(),
            LaneInId = "1",
            LaneOutId = "1",
            RFID = "dfsdfdsfds",
            Make = "Toyota",
            Amount = 300
        };
        private PaymentModel? paymentStatus = new PaymentModel()
        {
            Id = id,
            LaneInId = "1",
            LaneOutId = "1",
            RFID = "dfsdfdsfds",
            Make = "Toyota",
            Amount = 300
        };
        #endregion

        #region AddAsync
        [Fact]
        public async Task GivenValidRequest_WhenAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _paymentRepositoryMock.Setup(x => x.AddAsync(It.IsNotNull<PaymentModel>())).ReturnsAsync(paymentStatus!);

            // Act
            var service = new PaymentService(_loggerMock.Object, _paymentRepositoryMock.Object, _mapper);
            var result = await service.AddAsync(addRequest);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            _paymentRepositoryMock.Verify(x => x.AddAsync(It.IsNotNull<PaymentModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.AddAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.AddAsync)} method", Times.Never, _exception);
        }
        [Fact]
        public async Task GivenValidRequestAndRepositoryIsDown_WhenAddAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            _paymentRepositoryMock.Setup(x => x.AddAsync(It.IsNotNull<PaymentModel>())).ThrowsAsync(new Exception());

            // Act
            var service = new PaymentService(_loggerMock.Object, _paymentRepositoryMock.Object, _mapper);
            Func<Task> func = () => service.AddAsync(addRequest);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            _paymentRepositoryMock.Verify(x => x.AddAsync(It.IsNotNull<PaymentModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.AddAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.AddAsync)} method", Times.Once, _exception);
        }
        #endregion

        #region UpdateAsync
        [Fact]
        public async Task GivenValidRequest_WhenUpdateAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            object callbackObject;
            _paymentRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(paymentStatus);
            _paymentRepositoryMock.Setup(x => x.UpdateAsync(It.IsNotNull<PaymentModel>())).Callback<object>(k => callbackObject = k);

            // Act
            var service = new PaymentService(_loggerMock.Object, _paymentRepositoryMock.Object, _mapper);
            var result = await service.UpdateAsync(id, updateRequest);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            _paymentRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _paymentRepositoryMock.Verify(x => x.UpdateAsync(It.IsNotNull<PaymentModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.UpdateAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.UpdateAsync)} method", Times.Never, _exception);
        }
        [Fact]
        public async Task GivenValidRequestAndNonExistingGuid_WhenUpdateAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            object callbackObject;
            _paymentRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(paymentStatus = null);
            _paymentRepositoryMock.Setup(x => x.UpdateAsync(It.IsNotNull<PaymentModel>())).Callback<object>(k => callbackObject = k);

            // Act
            var service = new PaymentService(_loggerMock.Object, _paymentRepositoryMock.Object, _mapper);
            var result = await service.UpdateAsync(id, updateRequest);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Count(x => x.Code == 404).Should().Be(1);
            _paymentRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _paymentRepositoryMock.Verify(x => x.UpdateAsync(It.IsNotNull<PaymentModel>()), Times.Never);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.UpdateAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.UpdateAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndRepositoryIsDown_WhenUpdateAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            _paymentRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(paymentStatus);
            _paymentRepositoryMock.Setup(x => x.UpdateAsync(It.IsNotNull<PaymentModel>())).ThrowsAsync(new Exception());

            // Act
            var service = new PaymentService(_loggerMock.Object, _paymentRepositoryMock.Object, _mapper);
            Func<Task> func = () => service.UpdateAsync(id, updateRequest);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            _paymentRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _paymentRepositoryMock.Verify(x => x.UpdateAsync(It.IsNotNull<PaymentModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.UpdateAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.UpdateAsync)} method", Times.Once, _exception);
        }
        #endregion

        #region RemoveAsync
        [Fact]
        public async Task GivenValidRequest_WhenRemoveAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            object callbackObject;
            _paymentRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(paymentStatus);
            _paymentRepositoryMock.Setup(x => x.RemoveAsync(It.IsNotNull<PaymentModel>())).Callback<object>(k => callbackObject = k);

            // Act
            var service = new PaymentService(_loggerMock.Object, _paymentRepositoryMock.Object, _mapper);
            var result = await service.RemoveAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeTrue();
            _paymentRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _paymentRepositoryMock.Verify(x => x.RemoveAsync(It.IsNotNull<PaymentModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.RemoveAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.RemoveAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndNonExistingGuid_WhenRemoveAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            _paymentRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(paymentStatus = null);

            // Act
            var service = new PaymentService(_loggerMock.Object, _paymentRepositoryMock.Object, _mapper);
            var result = await service.RemoveAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Count(x => x.Code == 404).Should().Be(1);
            _paymentRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _paymentRepositoryMock.Verify(x => x.RemoveAsync(It.IsNotNull<PaymentModel>()), Times.Never);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.RemoveAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.RemoveAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndRepositoryIsDown_WhenRemoveAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            _paymentRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ThrowsAsync(new Exception());

            // Act
            var service = new PaymentService(_loggerMock.Object, _paymentRepositoryMock.Object, _mapper);
            Func<Task> func = () => service.RemoveAsync(id);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            _paymentRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _paymentRepositoryMock.Verify(x => x.RemoveAsync(It.IsNotNull<PaymentModel>()), Times.Never);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.RemoveAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.RemoveAsync)} method", Times.Once, _exception);
        }
        #endregion

        #region GetByIdAsync
        [Fact]
        public async Task GivenValidRequest_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _paymentRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(paymentStatus);

            // Act
            var service = new PaymentService(_loggerMock.Object, _paymentRepositoryMock.Object, _mapper);
            var result = await service.GetByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            _paymentRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.GetByIdAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestButNotFound_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _paymentRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync((PaymentModel)null!);

            // Act
            var service = new PaymentService(_loggerMock.Object, _paymentRepositoryMock.Object, _mapper);
            var result = await service.GetByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
            _paymentRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.GetByIdAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndRepositoryIsDown_WhenGetByIdAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            _paymentRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ThrowsAsync(new Exception());

            // Act
            var service = new PaymentService(_loggerMock.Object, _paymentRepositoryMock.Object, _mapper);
            Func<Task> func = () => service.GetByIdAsync(id);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            _paymentRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.GetByIdAsync)} method", Times.Once, _exception);
        }
        #endregion
    }
}