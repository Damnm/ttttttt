using EPAY.ETC.Core.API.Core.Models.PrintLog;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.PrintLog;
using EPAY.ETC.Core.API.Infrastructure.Services.PrintLog;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Common;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Helpers;
using EPAY.ETC.Core.Models.Request;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Services.PrintLog
{
    public class PrintLogServiceTests : AutoMapperTestBase
    {
        #region Init mock data
        private readonly Exception _exception = null!;
        private readonly Mock<ILogger<PrintLogService>> _loggerMock = new();
        private readonly Mock<IPrintLogRepository> _repositoryMock = new();
        private static Guid id = Guid.NewGuid();
        private PrintLogModel printLog = new PrintLogModel()
        {
            Id = id,
            CreatedDate = DateTime.Now,
            PlateNumber = "Some Plate number",
        };
        private PrintLogRequestModel request = new PrintLogRequestModel()
        {
            PlateNumber = "Some Plate number",
                   };

        #endregion

        #region AddAsync
        [Fact]
        public async Task GivenValidRequest_WhenAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _repositoryMock.Setup(x => x.AddAsync(It.IsNotNull<PrintLogModel>())).ReturnsAsync(printLog);

            // Act
            var service = new PrintLogService(_loggerMock.Object, _repositoryMock.Object, _mapper);
            var result = await service.AddAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            _repositoryMock.Verify(x => x.AddAsync(It.IsNotNull<PrintLogModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.AddAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.AddAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndRepositoryIsDown_WhenAddAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            _repositoryMock.Setup(x => x.AddAsync(It.IsNotNull<PrintLogModel>())).ThrowsAsync(new Exception());

            // Act
            var service = new PrintLogService(_loggerMock.Object, _repositoryMock.Object, _mapper);
            Func<Task> func = () => service.AddAsync(request);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            _repositoryMock.Verify(x => x.AddAsync(It.IsNotNull<PrintLogModel>()), Times.Once);
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
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(printLog);
            _repositoryMock.Setup(x => x.UpdateAsync(It.IsNotNull<PrintLogModel>())).Callback<object>(k => callbackObject = k);

            // Act
            var service = new PrintLogService(_loggerMock.Object, _repositoryMock.Object, _mapper);
            var result = await service.UpdateAsync(id, request);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            _repositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _repositoryMock.Verify(x => x.UpdateAsync(It.IsNotNull<PrintLogModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.UpdateAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.UpdateAsync)} method", Times.Never, _exception);
        }
        [Fact]
        public async Task GivenValidRequestAndNonExistingGuid_WhenUpdateAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            object callbackObject;
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync((PrintLogModel)null!);
            _repositoryMock.Setup(x => x.UpdateAsync(It.IsNotNull<PrintLogModel>())).Callback<object>(k => callbackObject = k);

            // Act
            var service = new PrintLogService(_loggerMock.Object, _repositoryMock.Object, _mapper);
            var result = await service.UpdateAsync(id, request);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Count(x => x.Code == 404).Should().Be(1);
            _repositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _repositoryMock.Verify(x => x.UpdateAsync(It.IsNotNull<PrintLogModel>()), Times.Never);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.UpdateAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.UpdateAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndRepositoryIsDown_WhenUpdateAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(printLog);
            _repositoryMock.Setup(x => x.UpdateAsync(It.IsNotNull<PrintLogModel>())).ThrowsAsync(new Exception());

            // Act
            var service = new PrintLogService(_loggerMock.Object, _repositoryMock.Object, _mapper);
            Func<Task> func = () => service.UpdateAsync(id, request);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            _repositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _repositoryMock.Verify(x => x.UpdateAsync(It.IsNotNull<PrintLogModel>()), Times.Once);
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
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(printLog);
            _repositoryMock.Setup(x => x.RemoveAsync(It.IsNotNull<PrintLogModel>())).Callback<object>(k => callbackObject = k);

            // Act
            var service = new PrintLogService(_loggerMock.Object, _repositoryMock.Object, _mapper);
            var result = await service.RemoveAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeTrue();
            _repositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _repositoryMock.Verify(x => x.RemoveAsync(It.IsNotNull<PrintLogModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.RemoveAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.RemoveAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndNonExistingGuid_WhenRemoveAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync((PrintLogModel)null!);

            // Act
            var service = new PrintLogService(_loggerMock.Object, _repositoryMock.Object, _mapper);
            var result = await service.RemoveAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Count(x => x.Code == 404).Should().Be(1);
            _repositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _repositoryMock.Verify(x => x.RemoveAsync(It.IsNotNull<PrintLogModel>()), Times.Never);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.RemoveAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.RemoveAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndRepositoryIsDown_WhenRemoveAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ThrowsAsync(new Exception());

            // Act
            var service = new PrintLogService(_loggerMock.Object, _repositoryMock.Object, _mapper);
            Func<Task> func = () => service.RemoveAsync(id);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            _repositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _repositoryMock.Verify(x => x.RemoveAsync(It.IsNotNull<PrintLogModel>()), Times.Never);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.RemoveAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.RemoveAsync)} method", Times.Once, _exception);
        }
        #endregion

        #region GetByIdAsync
        [Fact]
        public async Task GivenValidRequest_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(printLog);

            // Act
            var service = new PrintLogService(_loggerMock.Object, _repositoryMock.Object, _mapper);
            var result = await service.GetByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            _repositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.GetByIdAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestButNotFound_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync((PrintLogModel)null!);

            // Act
            var service = new PrintLogService(_loggerMock.Object, _repositoryMock.Object, _mapper);
            var result = await service.GetByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
            _repositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.GetByIdAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndRepositoryIsDown_WhenGetByIdAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ThrowsAsync(new Exception());

            // Act
            var service = new PrintLogService(_loggerMock.Object, _repositoryMock.Object, _mapper);
            Func<Task> func = () => service.GetByIdAsync(id);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            _repositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.GetByIdAsync)} method", Times.Once, _exception);
        }
        #endregion

    }
}
