using EPAY.ETC.Core.API.Core.Models.Fees;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Fees;
using EPAY.ETC.Core.API.Infrastructure.Services.Fees;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Common;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Helpers;
using EPAY.ETC.Core.Models.Fees;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using StackExchange.Redis;
using System.Linq.Expressions;
using CoreModel = EPAY.ETC.Core.Models.Fees;
using FeeModel = EPAY.ETC.Core.API.Core.Models.Fees.FeeModel;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Services.Fees
{
    public class FeeServiceTests : AutoMapperTestBase
    {
        #region Init mock instance
        private readonly Mock<ILogger<FeeService>> _loggerMock = new();
        private readonly Mock<IFeeRepository> _feeRepositoryMock = new();
        public readonly Mock<StackExchange.Redis.IDatabase> _redisDBMock = new();
        #endregion

        #region Init test data
        private CoreModel.FeeModel request = new CoreModel.FeeModel()
        {
            CreatedDate = new DateTime(2023, 9, 15, 3, 6, 8),
            EmployeeId = "Some employee",
            FeeId = Guid.Parse("a71717fb-cffd-4994-89d9-775a62d89399"),
            ObjectId = Guid.Parse("a71717fb-cffd-4994-89d9-775a62d89399"),
            ShiftId = Guid.Parse("a71717fb-cffd-4994-89d9-775a62d89399"),
            Payment = new CoreModel.PaymentModel()
            {
                Duration = 32541,
                Model = "Some model",
                PlateNumber = "Some plate number",
                Amount = 9000
            }
        };
        private FeeModel feeModel = new FeeModel()
        {
            CreatedDate = new DateTime(2023, 9, 15, 3, 6, 8),
            EmployeeId = "Some employee",
            Id = Guid.Parse("a71717fb-cffd-4994-89d9-775a62d89399"),
            ObjectId = Guid.Parse("a71717fb-cffd-4994-89d9-775a62d89399"),
            ShiftId = Guid.Parse("a71717fb-cffd-4994-89d9-775a62d89399"),
            Duration = 32541,
            Model = "Some model",
            PlateNumber = "Some plate number",
            Amount = 9000,
            LaneInDate = DateTimeOffset.FromUnixTimeSeconds(0).DateTime,
            LaneInEpoch = 0,
            LaneOutDate = DateTimeOffset.FromUnixTimeSeconds(0).DateTime,
            LaneOutEpoch = 0
        };
        private Exception _exception = null!;
        #endregion

        #region AddAsync
        [Fact]
        public async Task GivenValidRequest_WhenAddAsyncIsCalled_ThenAddedRecordSuccessfulAndReturnCorrectResult()
        {
            // Arrange
            _feeRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeModel, bool>>>())).ReturnsAsync(new List<FeeModel>());
            _feeRepositoryMock.Setup(x => x.AddAsync(It.IsAny<FeeModel>())).ReturnsAsync(feeModel);

            // Act
            var service = new FeeService(_loggerMock.Object, _feeRepositoryMock.Object, _mapper, _redisDBMock.Object);
            var result = await service.AddAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(feeModel);

            _feeRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeModel, bool>>>()), Times.Once);
            _feeRepositoryMock.Verify(x => x.AddAsync(It.IsAny<FeeModel>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.AddAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.AddAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndObjectIdIsExists_WhenAddAsyncIsCalled_ThenReturnConflict()
        {
            // Arrange
            _feeRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeModel, bool>>>())).ReturnsAsync(new List<FeeModel>() { new FeeModel() });

            // Act
            var service = new FeeService(_loggerMock.Object, _feeRepositoryMock.Object, _mapper, _redisDBMock.Object);
            var result = await service.AddAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Errors.Count(x => x.Code.Equals(StatusCodes.Status409Conflict)).Should().BeGreaterThan(0);

            _feeRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeModel, bool>>>()), Times.Once);
            _feeRepositoryMock.Verify(x => x.AddAsync(It.IsAny<FeeModel>()), Times.Never);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.AddAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.AddAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndFeeRepositoryIsDown_WhenAddAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var exception = new Exception("Some ex");
            _feeRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeModel, bool>>>())).ThrowsAsync(exception);

            // Act
            var service = new FeeService(_loggerMock.Object, _feeRepositoryMock.Object, _mapper, _redisDBMock.Object);
            Func<Task> func = async () => await service.AddAsync(request);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => func());
            ex.Message.Should().Be(exception.Message);

            _feeRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeModel, bool>>>()), Times.Once);
            _feeRepositoryMock.Verify(x => x.AddAsync(It.IsAny<FeeModel>()), Times.Never);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.AddAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.AddAsync)} method", Times.Once, _exception);
        }
        #endregion

        #region UpdateAsync
        [Fact]
        public async Task GivenValidRequest_WhenUpdateAsyncIsCalled_ThenUpdatedRecordSuccessfulAndReturnCorrectResult()
        {
            // Arrange
            _feeRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeModel, bool>>>())).ReturnsAsync(new List<FeeModel>());
            _feeRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new FeeModel());
            _feeRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<FeeModel>())).Callback<object>(s => { });

            // Act
            var service = new FeeService(_loggerMock.Object, _feeRepositoryMock.Object, _mapper, _redisDBMock.Object);
            var result = await service.UpdateAsync(feeModel.Id, request);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(feeModel);

            _feeRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _feeRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeModel, bool>>>()), Times.Once);
            _feeRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<FeeModel>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.UpdateAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.UpdateAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndObjectIdIsExists_WhenUpdateAsyncIsCalled_ThenReturnConflict()
        {
            // Arrange
            _feeRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeModel, bool>>>())).ReturnsAsync(new List<FeeModel>() { new FeeModel() });
            _feeRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new FeeModel());

            // Act
            var service = new FeeService(_loggerMock.Object, _feeRepositoryMock.Object, _mapper, _redisDBMock.Object);
            var result = await service.UpdateAsync(It.IsAny<Guid>(), request);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Errors.Count(x => x.Code.Equals(StatusCodes.Status409Conflict)).Should().BeGreaterThan(0);

            _feeRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _feeRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeModel, bool>>>()), Times.Once);
            _feeRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<FeeModel>()), Times.Never);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.UpdateAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.UpdateAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndFeeIdIsNotExists_WhenUpdateAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            _feeRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()));

            // Act
            var service = new FeeService(_loggerMock.Object, _feeRepositoryMock.Object, _mapper, _redisDBMock.Object);
            var result = await service.UpdateAsync(It.IsAny<Guid>(), request);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Errors.Count(x => x.Code.Equals(StatusCodes.Status404NotFound)).Should().BeGreaterThan(0);

            _feeRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _feeRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeModel, bool>>>()), Times.Never);
            _feeRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<FeeModel>()), Times.Never);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.UpdateAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.UpdateAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndFeeRepositoryIsDown_WhenUpdateAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var exception = new Exception("Some ex");
            _feeRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(exception);

            // Act
            var service = new FeeService(_loggerMock.Object, _feeRepositoryMock.Object, _mapper, _redisDBMock.Object);
            Func<Task> func = async () => await service.UpdateAsync(It.IsAny<Guid>(), request);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => func());
            ex.Message.Should().Be(exception.Message);

            _feeRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _feeRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeModel, bool>>>()), Times.Never);
            _feeRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<FeeModel>()), Times.Never);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.UpdateAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.UpdateAsync)} method", Times.Once, _exception);
        }
        #endregion

        #region RemoveAsync
        [Fact]
        public async Task GivenValidRequest_WhenRemoveAsyncIsCalled_ThenRemovedRecordSuccessfulAndReturnNull()
        {
            // Arrange
            _feeRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new FeeModel());
            _feeRepositoryMock.Setup(x => x.RemoveAsync(It.IsAny<FeeModel>())).Callback<object>(s => { });

            // Act
            var service = new FeeService(_loggerMock.Object, _feeRepositoryMock.Object, _mapper, _redisDBMock.Object);
            var result = await service.RemoveAsync(It.IsAny<Guid>());

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeNull();

            _feeRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _feeRepositoryMock.Verify(x => x.RemoveAsync(It.IsAny<FeeModel>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.RemoveAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.RemoveAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndFeeIdIsNotExists_WhenRemoveAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            _feeRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()));

            // Act
            var service = new FeeService(_loggerMock.Object, _feeRepositoryMock.Object, _mapper, _redisDBMock.Object);
            var result = await service.RemoveAsync(It.IsAny<Guid>());

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Errors.Count(x => x.Code.Equals(StatusCodes.Status404NotFound)).Should().BeGreaterThan(0);

            _feeRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _feeRepositoryMock.Verify(x => x.RemoveAsync(It.IsAny<FeeModel>()), Times.Never);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.RemoveAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.RemoveAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndFeeRepositoryIsDown_WhenRemoveAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var exception = new Exception("Some ex");
            _feeRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(exception);

            // Act
            var service = new FeeService(_loggerMock.Object, _feeRepositoryMock.Object, _mapper, _redisDBMock.Object);
            Func<Task> func = async () => await service.RemoveAsync(It.IsAny<Guid>());

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => func());
            ex.Message.Should().Be(exception.Message);

            _feeRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _feeRepositoryMock.Verify(x => x.RemoveAsync(It.IsAny<FeeModel>()), Times.Never);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.RemoveAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.RemoveAsync)} method", Times.Once, _exception);
        }
        #endregion

        #region GetByIdAsync
        [Fact]
        public async Task GivenValidRequest_WhenGetByIdAsyncIsCalled_ThenRemovedRecordSuccessfulAndReturnNull()
        {
            // Arrange
            _feeRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(feeModel);

            // Act
            var service = new FeeService(_loggerMock.Object, _feeRepositoryMock.Object, _mapper, _redisDBMock.Object);
            var result = await service.GetByIdAsync(It.IsAny<Guid>());

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(feeModel);

            _feeRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetByIdAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.GetByIdAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndFeeIdIsNotExists_WhenGetByIdAsyncIsCalled_ThenReturnDataNull()
        {
            // Arrange
            _feeRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()));

            // Act
            var service = new FeeService(_loggerMock.Object, _feeRepositoryMock.Object, _mapper, _redisDBMock.Object);
            var result = await service.GetByIdAsync(It.IsAny<Guid>());

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeNull();

            _feeRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetByIdAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.GetByIdAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndFeeRepositoryIsDown_WhenGetByIdAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var exception = new Exception("Some ex");
            _feeRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(exception);

            // Act
            var service = new FeeService(_loggerMock.Object, _feeRepositoryMock.Object, _mapper, _redisDBMock.Object);
            Func<Task> func = async () => await service.GetByIdAsync(It.IsAny<Guid>());

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => func());
            ex.Message.Should().Be(exception.Message);

            _feeRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetByIdAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.GetByIdAsync)} method", Times.Once, _exception);
        }
        #endregion

        #region GetAllAsync
        [Fact]
        public async Task GivenValidRequest_WhenGetAllAsyncIsCalled_ThenRemovedRecordSuccessfulAndReturnNull()
        {
            // Arrange
            _feeRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeModel, bool>>>())).ReturnsAsync(new List<FeeModel>() { new FeeModel() });

            // Act
            var service = new FeeService(_loggerMock.Object, _feeRepositoryMock.Object, _mapper, _redisDBMock.Object);
            var result = await service.GetAllAsync(It.IsAny<Expression<Func<FeeModel, bool>>>());

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull().And.HaveCount(1);

            _feeRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeModel, bool>>>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetAllAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.GetAllAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndFeeRepositoryIsDown_WhenGetAllAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var exception = new Exception("Some ex");
            _feeRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeModel, bool>>>())).ThrowsAsync(exception);

            // Act
            var service = new FeeService(_loggerMock.Object, _feeRepositoryMock.Object, _mapper, _redisDBMock.Object);
            Func<Task> func = async () => await service.GetAllAsync(It.IsAny<Expression<Func<FeeModel, bool>>>());

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => func());
            ex.Message.Should().Be(exception.Message);

            _feeRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeModel, bool>>>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetAllAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.GetAllAsync)} method", Times.Once, _exception);
        }
        #endregion

    }
}
