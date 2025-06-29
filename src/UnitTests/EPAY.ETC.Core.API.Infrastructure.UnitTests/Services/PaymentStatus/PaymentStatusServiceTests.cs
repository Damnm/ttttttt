﻿using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.PaymentStatus;
using EPAY.ETC.Core.API.Infrastructure.Services.PaymentStatus;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Common;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Helpers;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.Request;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using PaymentStatusModel = EPAY.ETC.Core.API.Core.Models.PaymentStatus.PaymentStatusModel;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Services.PaymentStatus
{
    public class PaymentStatusServiceTests : AutoMapperTestBase
    {
        #region Init mock data
        private readonly Exception _exception = null!;
        private readonly Mock<ILogger<PaymentStatusService>> _loggerMock = new();
        private readonly Mock<IPaymentStatusRepository> _paymentStatusRepositoryMock = new();
        private static Guid id = Guid.NewGuid();
        private PaymentStatusAddRequestModel addRequest = new PaymentStatusAddRequestModel()
        {
            PaymentId = Guid.NewGuid(),
            Amount = 300,
            Currency = "vnd",
            PaymentMethod = PaymentMethodEnum.RFID,
        };
        private PaymentStatusUpdateRequestModel updateRequest = new PaymentStatusUpdateRequestModel()
        {
            PaymentId = Guid.NewGuid(),
            Amount = 300,
            Currency = "vnd",
            PaymentMethod = PaymentMethodEnum.RFID,
        };
        private PaymentStatusModel? paymentStatus = new PaymentStatusModel()
        {
            Id = id,
            PaymentId = Guid.NewGuid(),
            Amount = 300,
            Currency = "vnd",
            PaymentMethod = PaymentMethodEnum.RFID,
        };

        private IQueryable<PaymentStatusModel> paymentStatuses = new List<PaymentStatusModel>()
        {
            new PaymentStatusModel()
            {
                PaymentDate = DateTime.Now,
                Status = PaymentStatusEnum.Failed,
                Reason = "dfd222fdsf",
                PaymentMethod = PaymentMethodEnum.RFID,
            },
            new PaymentStatusModel()
            {
                PaymentDate = DateTime.Now,
                Status = PaymentStatusEnum.Failed,
                Reason = "dfd111fdsf",
                PaymentMethod = PaymentMethodEnum.QRCode,
            },
            new PaymentStatusModel()
            {
                PaymentDate = DateTime.Now,
                Status = PaymentStatusEnum.Failed,
                Reason = "dfd66fdsf",
                PaymentMethod = PaymentMethodEnum.Cash,
            }
        }.AsQueryable();
        #endregion

        #region AddAsync
        [Fact]
        public async Task GivenValidRequest_WhenAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _paymentStatusRepositoryMock.Setup(x => x.AddAsync(It.IsNotNull<PaymentStatusModel>())).ReturnsAsync(paymentStatus!);

            // Act
            var service = new PaymentStatusService(_loggerMock.Object, _paymentStatusRepositoryMock.Object, _mapper);
            var result = await service.AddAsync(addRequest);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            _paymentStatusRepositoryMock.Verify(x => x.AddAsync(It.IsNotNull<PaymentStatusModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.AddAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.AddAsync)} method", Times.Never, _exception);
        }
        [Fact]
        public async Task GivenValidRequestAndRepositoryIsDown_WhenAddAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            _paymentStatusRepositoryMock.Setup(x => x.AddAsync(It.IsNotNull<PaymentStatusModel>())).ThrowsAsync(new Exception());

            // Act
            var service = new PaymentStatusService(_loggerMock.Object, _paymentStatusRepositoryMock.Object, _mapper);
            Func<Task> func = () => service.AddAsync(addRequest);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            _paymentStatusRepositoryMock.Verify(x => x.AddAsync(It.IsNotNull<PaymentStatusModel>()), Times.Once);
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
            _paymentStatusRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(paymentStatus);
            _paymentStatusRepositoryMock.Setup(x => x.UpdateAsync(It.IsNotNull<PaymentStatusModel>())).Callback<object>(k => callbackObject = k);

            // Act
            var service = new PaymentStatusService(_loggerMock.Object, _paymentStatusRepositoryMock.Object, _mapper);
            var result = await service.UpdateAsync(id, updateRequest);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            _paymentStatusRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _paymentStatusRepositoryMock.Verify(x => x.UpdateAsync(It.IsNotNull<PaymentStatusModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.UpdateAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.UpdateAsync)} method", Times.Never, _exception);
        }
        [Fact]
        public async Task GivenValidRequestAndNonExistingGuid_WhenUpdateAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            object callbackObject;
            _paymentStatusRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(paymentStatus = null);
            _paymentStatusRepositoryMock.Setup(x => x.UpdateAsync(It.IsNotNull<PaymentStatusModel>())).Callback<object>(k => callbackObject = k);

            // Act
            var service = new PaymentStatusService(_loggerMock.Object, _paymentStatusRepositoryMock.Object, _mapper);
            var result = await service.UpdateAsync(id, updateRequest);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Count(x => x.Code == 404).Should().Be(1);
            _paymentStatusRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _paymentStatusRepositoryMock.Verify(x => x.UpdateAsync(It.IsNotNull<PaymentStatusModel>()), Times.Never);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.UpdateAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.UpdateAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndRepositoryIsDown_WhenUpdateAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            _paymentStatusRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(paymentStatus);
            _paymentStatusRepositoryMock.Setup(x => x.UpdateAsync(It.IsNotNull<PaymentStatusModel>())).ThrowsAsync(new Exception());

            // Act
            var service = new PaymentStatusService(_loggerMock.Object, _paymentStatusRepositoryMock.Object, _mapper);
            Func<Task> func = () => service.UpdateAsync(id, updateRequest);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            _paymentStatusRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _paymentStatusRepositoryMock.Verify(x => x.UpdateAsync(It.IsNotNull<PaymentStatusModel>()), Times.Once);
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
            _paymentStatusRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(paymentStatus);
            _paymentStatusRepositoryMock.Setup(x => x.RemoveAsync(It.IsNotNull<PaymentStatusModel>())).Callback<object>(k => callbackObject = k);

            // Act
            var service = new PaymentStatusService(_loggerMock.Object, _paymentStatusRepositoryMock.Object, _mapper);
            var result = await service.RemoveAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeTrue();
            _paymentStatusRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _paymentStatusRepositoryMock.Verify(x => x.RemoveAsync(It.IsNotNull<PaymentStatusModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.RemoveAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.RemoveAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndNonExistingGuid_WhenRemoveAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            _paymentStatusRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(paymentStatus = null);

            // Act
            var service = new PaymentStatusService(_loggerMock.Object, _paymentStatusRepositoryMock.Object, _mapper);
            var result = await service.RemoveAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Count(x => x.Code == 404).Should().Be(1);
            _paymentStatusRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _paymentStatusRepositoryMock.Verify(x => x.RemoveAsync(It.IsNotNull<PaymentStatusModel>()), Times.Never);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.RemoveAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.RemoveAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndRepositoryIsDown_WhenRemoveAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            _paymentStatusRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ThrowsAsync(new Exception());

            // Act
            var service = new PaymentStatusService(_loggerMock.Object, _paymentStatusRepositoryMock.Object, _mapper);
            Func<Task> func = () => service.RemoveAsync(id);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            _paymentStatusRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _paymentStatusRepositoryMock.Verify(x => x.RemoveAsync(It.IsNotNull<PaymentStatusModel>()), Times.Never);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.RemoveAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.RemoveAsync)} method", Times.Once, _exception);
        }
        #endregion

        #region GetByIdAsync
        [Fact]
        public async Task GivenValidRequest_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _paymentStatusRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(paymentStatus);

            // Act
            var service = new PaymentStatusService(_loggerMock.Object, _paymentStatusRepositoryMock.Object, _mapper);
            var result = await service.GetByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            _paymentStatusRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.GetByIdAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestButNotFound_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _paymentStatusRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync((PaymentStatusModel)null!);

            // Act
            var service = new PaymentStatusService(_loggerMock.Object, _paymentStatusRepositoryMock.Object, _mapper);
            var result = await service.GetByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
            _paymentStatusRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.GetByIdAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndRepositoryIsDown_WhenGetByIdAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            _paymentStatusRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ThrowsAsync(new Exception());

            // Act
            var service = new PaymentStatusService(_loggerMock.Object, _paymentStatusRepositoryMock.Object, _mapper);
            Func<Task> func = () => service.GetByIdAsync(id);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            _paymentStatusRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.GetByIdAsync)} method", Times.Once, _exception);
        }
        #endregion

        #region GetPaymentStatusHistoryAsync
        [Fact]
        public async Task GivenValidRequest_WhenGetPaymentStatusHistoryAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _paymentStatusRepositoryMock.Setup(x => x.GetPaymentStatusHistoryAsync(It.IsNotNull<Expression<Func<PaymentStatusModel, bool>>>())).ReturnsAsync(paymentStatuses);

            // Act
            var service = new PaymentStatusService(_loggerMock.Object, _paymentStatusRepositoryMock.Object, _mapper);
            var result = await service.GetPaymentStatusHistoryAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data?.Count.Should().Be(3);
            _paymentStatusRepositoryMock.Verify(x => x.GetPaymentStatusHistoryAsync(It.IsNotNull<Expression<Func<PaymentStatusModel, bool>>>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetPaymentStatusHistoryAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.GetPaymentStatusHistoryAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestButNotFound_WhenGetPaymentStatusHistoryAsyncIsCalled_ThenReturnCorrectResult()
        {
            IQueryable<PaymentStatusModel> paymentStat = Enumerable.Empty<PaymentStatusModel>().AsQueryable();

            // Arrange
            _paymentStatusRepositoryMock.Setup(x => x.GetPaymentStatusHistoryAsync(It.IsNotNull<Expression<Func<PaymentStatusModel, bool>>>())).ReturnsAsync(paymentStat);

            // Act
            var service = new PaymentStatusService(_loggerMock.Object, _paymentStatusRepositoryMock.Object, _mapper);
            var result = await service.GetPaymentStatusHistoryAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
            _paymentStatusRepositoryMock.Verify(x => x.GetPaymentStatusHistoryAsync(It.IsNotNull<Expression<Func<PaymentStatusModel, bool>>>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetPaymentStatusHistoryAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.GetPaymentStatusHistoryAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndRepositoryIsDown_WhenGetPaymentStatusHistoryAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            _paymentStatusRepositoryMock.Setup(x => x.GetPaymentStatusHistoryAsync(It.IsNotNull<Expression<Func<PaymentStatusModel, bool>>>())).ThrowsAsync(new Exception());

            // Act
            var service = new PaymentStatusService(_loggerMock.Object, _paymentStatusRepositoryMock.Object, _mapper);
            Func<Task> func = () => service.GetPaymentStatusHistoryAsync(id);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            _paymentStatusRepositoryMock.Verify(x => x.GetPaymentStatusHistoryAsync(It.IsNotNull<Expression<Func<PaymentStatusModel, bool>>>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetPaymentStatusHistoryAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.GetPaymentStatusHistoryAsync)} method", Times.Once, _exception);
        }
        #endregion
    }
}