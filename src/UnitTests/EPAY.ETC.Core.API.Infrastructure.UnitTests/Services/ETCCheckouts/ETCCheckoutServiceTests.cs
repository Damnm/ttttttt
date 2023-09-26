using EPAY.ETC.Core.API.Core.Models.ETCCheckOuts;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ETCCheckouts;
using EPAY.ETC.Core.API.Infrastructure.Services.ETCCheckouts;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Common;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Helpers;
using EPAY.ETC.Core.Models.Request;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Services.ETCCheckouts
{
    public class ETCCheckoutServiceTests : AutoMapperTestBase
    {
        #region Init mock instance
        private readonly Mock<ILogger<ETCCheckoutService>> _loggerMock = new();
        private readonly Mock<IETCCheckoutRepository> _etcCheckOutRepositoryMock = new();
        #endregion

        #region Init test data
        private ETCCheckoutAddUpdateRequestModel request = new ETCCheckoutAddUpdateRequestModel()
        {
            TransactionId = "Some Transaction",
            PaymentId = Guid.Parse("d87a071c-596b-4bc2-9205-240b9f6b03ca"),
            PlateNumber = "Some Plate",
            Amount = 7000,
            RFID = "Some RFID",
            ServiceProvider = Models.Enums.ETCServiceProviderEnum.VETC,
            TransactionStatus = Models.Enums.TransactionStatusEnum.CheckOut
        };

        private ETCCheckoutDataModel etcCheckOutModel = new ETCCheckoutDataModel()
        {
            CreatedDate = new DateTime(2023, 9, 15, 3, 6, 8),
            Id = Guid.NewGuid(),
            TransactionId = "Some Transaction",
            PaymentId = Guid.Parse("d87a071c-596b-4bc2-9205-240b9f6b03ca"),
            PlateNumber = "Some Plate",
            Amount = 7000,
            RFID = "Some RFID",
            ServiceProvider = Models.Enums.ETCServiceProviderEnum.VETC,
            TransactionStatus = Models.Enums.TransactionStatusEnum.CheckOut
        };
        private Exception _exception = null!;
        #endregion

        #region AddAsync
        [Fact]
        public async Task GivenValidRequest_WhenAddAsyncIsCalled_ThenAddedRecordSuccessfulAndReturnCorrectResult()
        {
            // Arrange
            _etcCheckOutRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<ETCCheckoutDataModel, bool>>>())).ReturnsAsync(new List<ETCCheckoutDataModel>());
            _etcCheckOutRepositoryMock.Setup(x => x.AddAsync(It.IsAny<ETCCheckoutDataModel>())).ReturnsAsync(etcCheckOutModel);

            // Act
            var service = new ETCCheckoutService(_loggerMock.Object, _etcCheckOutRepositoryMock.Object, _mapper);
            var result = await service.AddAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data?.Amount.Should().Be(request.Amount);
            result.Data?.PaymentId.Should().Be(request.PaymentId);
            result.Data?.PlateNumber.Should().Be(request.PlateNumber);
            result.Data?.Amount.Should().Be(request.Amount);
            result.Data?.RFID.Should().Be(request.RFID);
            result.Data?.ServiceProvider.Should().Be(request.ServiceProvider);
            result.Data?.TransactionStatus.Should().Be(request.TransactionStatus);

            _etcCheckOutRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<ETCCheckoutDataModel, bool>>>()), Times.Once);
            _etcCheckOutRepositoryMock.Verify(x => x.AddAsync(It.IsAny<ETCCheckoutDataModel>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.AddAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.AddAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndObjectIdIsExists_WhenAddAsyncIsCalled_ThenReturnConflict()
        {
            // Arrange
            _etcCheckOutRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<ETCCheckoutDataModel, bool>>>())).ReturnsAsync(new List<ETCCheckoutDataModel>() { new ETCCheckoutDataModel() });

            // Act
            var service = new ETCCheckoutService(_loggerMock.Object, _etcCheckOutRepositoryMock.Object, _mapper);
            var result = await service.AddAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Errors.Count(x => x.Code.Equals(StatusCodes.Status409Conflict)).Should().BeGreaterThan(0);

            _etcCheckOutRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<ETCCheckoutDataModel, bool>>>()), Times.Once);
            _etcCheckOutRepositoryMock.Verify(x => x.AddAsync(It.IsAny<ETCCheckoutDataModel>()), Times.Never);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.AddAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.AddAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndETCCheckOutRepositoryIsDown_WhenAddAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var exception = new Exception("Some ex");
            _etcCheckOutRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<ETCCheckoutDataModel, bool>>>())).ThrowsAsync(exception);

            // Act
            var service = new ETCCheckoutService(_loggerMock.Object, _etcCheckOutRepositoryMock.Object, _mapper);
            Func<Task> func = async () => await service.AddAsync(request);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => func());
            ex.Message.Should().Be(exception.Message);

            _etcCheckOutRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<ETCCheckoutDataModel, bool>>>()), Times.Once);
            _etcCheckOutRepositoryMock.Verify(x => x.AddAsync(It.IsAny<ETCCheckoutDataModel>()), Times.Never);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.AddAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.AddAsync)} method", Times.Once, _exception);
        }
        #endregion

        #region UpdateAsync
        [Fact]
        public async Task GivenValidRequest_WhenUpdateAsyncIsCalled_ThenUpdatedRecordSuccessfulAndReturnCorrectResult()
        {
            // Arrange
            _etcCheckOutRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<ETCCheckoutDataModel, bool>>>())).ReturnsAsync(new List<ETCCheckoutDataModel>());
            _etcCheckOutRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new ETCCheckoutDataModel());
            _etcCheckOutRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<ETCCheckoutDataModel>())).Callback<object>(s => { });

            // Act
            var service = new ETCCheckoutService(_loggerMock.Object, _etcCheckOutRepositoryMock.Object, _mapper);
            var result = await service.UpdateAsync(etcCheckOutModel.Id, request);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data?.Amount.Should().Be(request.Amount);
            result.Data?.PaymentId.Should().Be(request.PaymentId);
            result.Data?.PlateNumber.Should().Be(request.PlateNumber);
            result.Data?.Amount.Should().Be(request.Amount);
            result.Data?.RFID.Should().Be(request.RFID);
            result.Data?.ServiceProvider.Should().Be(request.ServiceProvider);
            result.Data?.TransactionStatus.Should().Be(request.TransactionStatus);

            _etcCheckOutRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _etcCheckOutRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<ETCCheckoutDataModel, bool>>>()), Times.Once);
            _etcCheckOutRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<ETCCheckoutDataModel>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.UpdateAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.UpdateAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndObjectIdIsExists_WhenUpdateAsyncIsCalled_ThenReturnConflict()
        {
            // Arrange
            _etcCheckOutRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<ETCCheckoutDataModel, bool>>>())).ReturnsAsync(new List<ETCCheckoutDataModel>() { new ETCCheckoutDataModel() });
            _etcCheckOutRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new ETCCheckoutDataModel());

            // Act
            var service = new ETCCheckoutService(_loggerMock.Object, _etcCheckOutRepositoryMock.Object, _mapper);
            var result = await service.UpdateAsync(It.IsAny<Guid>(), request);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Errors.Count(x => x.Code.Equals(StatusCodes.Status409Conflict)).Should().BeGreaterThan(0);

            _etcCheckOutRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _etcCheckOutRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<ETCCheckoutDataModel, bool>>>()), Times.Once);
            _etcCheckOutRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<ETCCheckoutDataModel>()), Times.Never);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.UpdateAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.UpdateAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndETCCheckOutIdIsNotExists_WhenUpdateAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            _etcCheckOutRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()));

            // Act
            var service = new ETCCheckoutService(_loggerMock.Object, _etcCheckOutRepositoryMock.Object, _mapper);
            var result = await service.UpdateAsync(It.IsAny<Guid>(), request);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Errors.Count(x => x.Code.Equals(StatusCodes.Status404NotFound)).Should().BeGreaterThan(0);

            _etcCheckOutRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _etcCheckOutRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<ETCCheckoutDataModel, bool>>>()), Times.Never);
            _etcCheckOutRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<ETCCheckoutDataModel>()), Times.Never);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.UpdateAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.UpdateAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndETCCheckOutRepositoryIsDown_WhenUpdateAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var exception = new Exception("Some ex");
            _etcCheckOutRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(exception);

            // Act
            var service = new ETCCheckoutService(_loggerMock.Object, _etcCheckOutRepositoryMock.Object, _mapper);
            Func<Task> func = async () => await service.UpdateAsync(It.IsAny<Guid>(), request);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => func());
            ex.Message.Should().Be(exception.Message);

            _etcCheckOutRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _etcCheckOutRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<ETCCheckoutDataModel, bool>>>()), Times.Never);
            _etcCheckOutRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<ETCCheckoutDataModel>()), Times.Never);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.UpdateAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.UpdateAsync)} method", Times.Once, _exception);
        }
        #endregion

        #region RemoveAsync
        [Fact]
        public async Task GivenValidRequest_WhenRemoveAsyncIsCalled_ThenRemovedRecordSuccessfulAndReturnNull()
        {
            // Arrange
            _etcCheckOutRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new ETCCheckoutDataModel());
            _etcCheckOutRepositoryMock.Setup(x => x.RemoveAsync(It.IsAny<ETCCheckoutDataModel>())).Callback<object>(s => { });

            // Act
            var service = new ETCCheckoutService(_loggerMock.Object, _etcCheckOutRepositoryMock.Object, _mapper);
            var result = await service.RemoveAsync(It.IsAny<Guid>());

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeNull();

            _etcCheckOutRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _etcCheckOutRepositoryMock.Verify(x => x.RemoveAsync(It.IsAny<ETCCheckoutDataModel>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.RemoveAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.RemoveAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndETCCheckOutIdIsNotExists_WhenRemoveAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            _etcCheckOutRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()));

            // Act
            var service = new ETCCheckoutService(_loggerMock.Object, _etcCheckOutRepositoryMock.Object, _mapper);
            var result = await service.RemoveAsync(It.IsAny<Guid>());

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Errors.Count(x => x.Code.Equals(StatusCodes.Status404NotFound)).Should().BeGreaterThan(0);

            _etcCheckOutRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _etcCheckOutRepositoryMock.Verify(x => x.RemoveAsync(It.IsAny<ETCCheckoutDataModel>()), Times.Never);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.RemoveAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.RemoveAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndETCCheckOutRepositoryIsDown_WhenRemoveAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var exception = new Exception("Some ex");
            _etcCheckOutRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(exception);

            // Act
            var service = new ETCCheckoutService(_loggerMock.Object, _etcCheckOutRepositoryMock.Object, _mapper);
            Func<Task> func = async () => await service.RemoveAsync(It.IsAny<Guid>());

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => func());
            ex.Message.Should().Be(exception.Message);

            _etcCheckOutRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _etcCheckOutRepositoryMock.Verify(x => x.RemoveAsync(It.IsAny<ETCCheckoutDataModel>()), Times.Never);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.RemoveAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.RemoveAsync)} method", Times.Once, _exception);
        }
        #endregion

        #region GetByIdAsync
        [Fact]
        public async Task GivenValidRequest_WhenGetByIdAsyncIsCalled_ThenRemovedRecordSuccessfulAndReturnNull()
        {
            // Arrange
            _etcCheckOutRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(etcCheckOutModel);

            // Act
            var service = new ETCCheckoutService(_loggerMock.Object, _etcCheckOutRepositoryMock.Object, _mapper);
            var result = await service.GetByIdAsync(It.IsAny<Guid>());

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data?.Amount.Should().Be(request.Amount);
            result.Data?.PaymentId.Should().Be(request.PaymentId);
            result.Data?.PlateNumber.Should().Be(request.PlateNumber);
            result.Data?.Amount.Should().Be(request.Amount);
            result.Data?.RFID.Should().Be(request.RFID);
            result.Data?.ServiceProvider.Should().Be(request.ServiceProvider);
            result.Data?.TransactionStatus.Should().Be(request.TransactionStatus);

            _etcCheckOutRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetByIdAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.GetByIdAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndETCCheckOutIdIsNotExists_WhenGetByIdAsyncIsCalled_ThenReturnDataNull()
        {
            // Arrange
            _etcCheckOutRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()));

            // Act
            var service = new ETCCheckoutService(_loggerMock.Object, _etcCheckOutRepositoryMock.Object, _mapper);
            var result = await service.GetByIdAsync(It.IsAny<Guid>());

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeNull();

            _etcCheckOutRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetByIdAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.GetByIdAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndETCCheckOutRepositoryIsDown_WhenGetByIdAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var exception = new Exception("Some ex");
            _etcCheckOutRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(exception);

            // Act
            var service = new ETCCheckoutService(_loggerMock.Object, _etcCheckOutRepositoryMock.Object, _mapper);
            Func<Task> func = async () => await service.GetByIdAsync(It.IsAny<Guid>());

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => func());
            ex.Message.Should().Be(exception.Message);

            _etcCheckOutRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetByIdAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.GetByIdAsync)} method", Times.Once, _exception);
        }
        #endregion

        #region GetAllAsync
        [Fact]
        public async Task GivenValidRequest_WhenGetAllAsyncIsCalled_ThenRemovedRecordSuccessfulAndReturnNull()
        {
            // Arrange
            _etcCheckOutRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<ETCCheckoutDataModel, bool>>>())).ReturnsAsync(new List<ETCCheckoutDataModel>() { new ETCCheckoutDataModel() });

            // Act
            var service = new ETCCheckoutService(_loggerMock.Object, _etcCheckOutRepositoryMock.Object, _mapper);
            var result = await service.GetAllAsync(It.IsAny<Expression<Func<ETCCheckoutDataModel, bool>>>());

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull().And.HaveCount(1);

            _etcCheckOutRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<ETCCheckoutDataModel, bool>>>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetAllAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.GetAllAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndETCCheckOutRepositoryIsDown_WhenGetAllAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var exception = new Exception("Some ex");
            _etcCheckOutRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<ETCCheckoutDataModel, bool>>>())).ThrowsAsync(exception);

            // Act
            var service = new ETCCheckoutService(_loggerMock.Object, _etcCheckOutRepositoryMock.Object, _mapper);
            Func<Task> func = async () => await service.GetAllAsync(It.IsAny<Expression<Func<ETCCheckoutDataModel, bool>>>());

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => func());
            ex.Message.Should().Be(exception.Message);

            _etcCheckOutRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<ETCCheckoutDataModel, bool>>>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetAllAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.GetAllAsync)} method", Times.Once, _exception);
        }
        #endregion
    }
}
