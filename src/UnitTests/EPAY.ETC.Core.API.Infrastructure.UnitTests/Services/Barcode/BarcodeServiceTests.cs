using EPAY.ETC.Core.API.Core.Models.Barcode;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Barcode;
using EPAY.ETC.Core.API.Infrastructure.Services.Barcode;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Common;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Helpers;
using EPAY.ETC.Core.Models.Request;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Services.Barcode
{
    public class BarcodeServiceTests: AutoMapperTestBase
    {
        #region Init mock data
        private readonly Exception _exception = null!;
        private readonly Mock<ILogger<BarcodeService>> _loggerMock = new();
        private readonly Mock<IBarcodeRepository> _barcodeRepositoryMock = new();
        private static Guid id = Guid.NewGuid();
        private BarcodeAddRequestModel addRequest = new BarcodeAddRequestModel()
        {
            Id = id,
            ActionCode = "111",
            ActionDesc = "Open Barrier",
            EmployeeId = "23232"
        };
        private BarcodeUpdateRequestModel updateRequest = new BarcodeUpdateRequestModel()
        {
            ActionCode = "111",
            ActionDesc = "Open Barrier",
            EmployeeId = "23232"
        };
        private BarcodeModel? barcode = new BarcodeModel()
        {
            Id = id,
            ActionCode = "111",
            ActionDesc = "Open Barrier",
            EmployeeId = "23232"
        };
        #endregion

        #region AddAsync
        [Fact]
        public async Task GivenValidRequest_WhenAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _barcodeRepositoryMock.Setup(x => x.AddAsync(It.IsNotNull<BarcodeModel>())).ReturnsAsync(barcode!);

            // Act
            var service = new BarcodeService(_loggerMock.Object, _barcodeRepositoryMock.Object, _mapper);
            var result = await service.AddAsync(addRequest);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            _barcodeRepositoryMock.Verify(x => x.AddAsync(It.IsNotNull<BarcodeModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.AddAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.AddAsync)} method", Times.Never, _exception);
        }
        [Fact]
        public async Task GivenValidRequestAndRepositoryIsDown_WhenAddAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            _barcodeRepositoryMock.Setup(x => x.AddAsync(It.IsNotNull<BarcodeModel>())).ThrowsAsync(new Exception());

            // Act
            var service = new BarcodeService(_loggerMock.Object, _barcodeRepositoryMock.Object, _mapper);
            Func<Task> func = () => service.AddAsync(addRequest);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            _barcodeRepositoryMock.Verify(x => x.AddAsync(It.IsNotNull<BarcodeModel>()), Times.Once);
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
            _barcodeRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(barcode);
            _barcodeRepositoryMock.Setup(x => x.UpdateAsync(It.IsNotNull<BarcodeModel>())).Callback<object>(k => callbackObject = k);

            // Act
            var service = new BarcodeService(_loggerMock.Object, _barcodeRepositoryMock.Object, _mapper);
            var result = await service.UpdateAsync(id, updateRequest);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            _barcodeRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _barcodeRepositoryMock.Verify(x => x.UpdateAsync(It.IsNotNull<BarcodeModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.UpdateAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.UpdateAsync)} method", Times.Never, _exception);
        }
        [Fact]
        public async Task GivenValidRequestAndNonExistingGuid_WhenUpdateAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            object callbackObject;
            _barcodeRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(barcode = null);
            _barcodeRepositoryMock.Setup(x => x.UpdateAsync(It.IsNotNull<BarcodeModel>())).Callback<object>(k => callbackObject = k);

            // Act
            var service = new BarcodeService(_loggerMock.Object, _barcodeRepositoryMock.Object, _mapper);
            var result = await service.UpdateAsync(id, updateRequest);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Count(x => x.Code == 404).Should().Be(1);
            _barcodeRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _barcodeRepositoryMock.Verify(x => x.UpdateAsync(It.IsNotNull<BarcodeModel>()), Times.Never);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.UpdateAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.UpdateAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndRepositoryIsDown_WhenUpdateAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            _barcodeRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(barcode);
            _barcodeRepositoryMock.Setup(x => x.UpdateAsync(It.IsNotNull<BarcodeModel>())).ThrowsAsync(new Exception());

            // Act
            var service = new BarcodeService(_loggerMock.Object, _barcodeRepositoryMock.Object, _mapper);
            Func<Task> func = () => service.UpdateAsync(id, updateRequest);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            _barcodeRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _barcodeRepositoryMock.Verify(x => x.UpdateAsync(It.IsNotNull<BarcodeModel>()), Times.Once);
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
            _barcodeRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(barcode);
            _barcodeRepositoryMock.Setup(x => x.RemoveAsync(It.IsNotNull<BarcodeModel>())).Callback<object>(k => callbackObject = k);

            // Act
            var service = new BarcodeService(_loggerMock.Object, _barcodeRepositoryMock.Object, _mapper);
            var result = await service.RemoveAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeTrue();
            _barcodeRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _barcodeRepositoryMock.Verify(x => x.RemoveAsync(It.IsNotNull<BarcodeModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.RemoveAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.RemoveAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndNonExistingGuid_WhenRemoveAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            _barcodeRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(barcode = null);

            // Act
            var service = new BarcodeService(_loggerMock.Object, _barcodeRepositoryMock.Object, _mapper);
            var result = await service.RemoveAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Count(x => x.Code == 404).Should().Be(1);
            _barcodeRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _barcodeRepositoryMock.Verify(x => x.RemoveAsync(It.IsNotNull<BarcodeModel>()), Times.Never);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.RemoveAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.RemoveAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndRepositoryIsDown_WhenRemoveAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            _barcodeRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ThrowsAsync(new Exception());

            // Act
            var service = new BarcodeService(_loggerMock.Object, _barcodeRepositoryMock.Object, _mapper);
            Func<Task> func = () => service.RemoveAsync(id);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            _barcodeRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _barcodeRepositoryMock.Verify(x => x.RemoveAsync(It.IsNotNull<BarcodeModel>()), Times.Never);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.RemoveAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.RemoveAsync)} method", Times.Once, _exception);
        }
        #endregion

        #region GetByIdAsync
        [Fact]
        public async Task GivenValidRequest_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _barcodeRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(barcode);

            // Act
            var service = new BarcodeService(_loggerMock.Object, _barcodeRepositoryMock.Object, _mapper);
            var result = await service.GetByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            _barcodeRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.GetByIdAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestButNotFound_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _barcodeRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync((BarcodeModel)null!);

            // Act
            var service = new BarcodeService(_loggerMock.Object, _barcodeRepositoryMock.Object, _mapper);
            var result = await service.GetByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
            _barcodeRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.GetByIdAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndRepositoryIsDown_WhenGetByIdAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            _barcodeRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ThrowsAsync(new Exception());

            // Act
            var service = new BarcodeService(_loggerMock.Object, _barcodeRepositoryMock.Object, _mapper);
            Func<Task> func = () => service.GetByIdAsync(id);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            _barcodeRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.GetByIdAsync)} method", Times.Once, _exception);
        }
        #endregion
    }
}
