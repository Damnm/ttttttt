using EPAY.ETC.Core.API.Core.Models.ManualBarrierControl;
using EPAY.ETC.Core.API.Infrastructure.Migrations;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ManualBarrierControls;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Payment;
using EPAY.ETC.Core.API.Infrastructure.Services.ManualBarrierControls;
using EPAY.ETC.Core.API.Infrastructure.Services.Payment;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Common;
using EPAY.ETC.Core.API.UnitTests.Helpers;
using EPAY.ETC.Core.Models.Request;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Services.ManualBarrierControls
{
    public class ManualBarrierControlsServiceTests : AutoMapperTestBase
    {
        #region Init mock data
        private readonly Exception _exception = null!;
        private readonly Mock<ILogger<ManualBarrierControlsService>> _loggerMock = new();
        private readonly Mock<IManualBarrierControlRepository> _manualBarrierControlRepositoryMock = new();
        private static Guid id = Guid.NewGuid();
        private ManualBarrierControlAddOrUpdateRequestModel addRequest = new ManualBarrierControlAddOrUpdateRequestModel()
        {
            EmployeeId = Guid.Parse("4512781e-4dc2-4ee3-acdd-a46becc08d6c"),
            Action = ActionEnum.Open,
            LaneOutId = "0103"
        };
        private ManualBarrierControlAddOrUpdateRequestModel updateRequest = new ManualBarrierControlAddOrUpdateRequestModel()
        {
            EmployeeId = Guid.Parse("4512781e-4dc2-4ee3-acdd-a46becc08d6c"),
            Action = ActionEnum.Open,
            LaneOutId = "0103"
        };
        private ManualBarrierControlModel? paymentStatus = new ManualBarrierControlModel()
        {
            Id = id,
            CreatedDate = DateTime.Now,
            EmployeeId = Guid.Parse("4512781e-4dc2-4ee3-acdd-a46becc08d6c"),
            Action = ActionEnum.Open,
            LaneOutId = "0103"
        };
        #endregion
        #region AddAsync
        [Fact]
        public async Task GivenValidRequest_WhenAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _manualBarrierControlRepositoryMock.Setup(x => x.AddAsync(It.IsNotNull<ManualBarrierControlModel>())).ReturnsAsync(paymentStatus!);

            // Act
            var service = new ManualBarrierControlsService(_loggerMock.Object, _manualBarrierControlRepositoryMock.Object, _mapper);
            var result = await service.AddAsync(addRequest);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            _manualBarrierControlRepositoryMock.Verify(x => x.AddAsync(It.IsNotNull<ManualBarrierControlModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.AddAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.AddAsync)} method", Times.Never, _exception);
        }
        [Fact]
        public async Task GivenValidRequestAndRepositoryIsDown_WhenAddAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            _manualBarrierControlRepositoryMock.Setup(x => x.AddAsync(It.IsNotNull<ManualBarrierControlModel>())).ThrowsAsync(new Exception());

            // Act
            var service = new ManualBarrierControlsService(_loggerMock.Object, _manualBarrierControlRepositoryMock.Object, _mapper);
            Func<Task> func = () => service.AddAsync(addRequest);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            _manualBarrierControlRepositoryMock.Verify(x => x.AddAsync(It.IsNotNull<ManualBarrierControlModel>()), Times.Once);
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
            _manualBarrierControlRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(paymentStatus);
            _manualBarrierControlRepositoryMock.Setup(x => x.UpdateAsync(It.IsNotNull<ManualBarrierControlModel>())).Callback<object>(k => callbackObject = k);

            // Act
            var service = new ManualBarrierControlsService(_loggerMock.Object, _manualBarrierControlRepositoryMock.Object, _mapper);
            var result = await service.UpdateAsync(id, updateRequest);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            _manualBarrierControlRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _manualBarrierControlRepositoryMock.Verify(x => x.UpdateAsync(It.IsNotNull<ManualBarrierControlModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.UpdateAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.UpdateAsync)} method", Times.Never, _exception);
        }
        [Fact]
        public async Task GivenValidRequestAndNonExistingGuid_WhenUpdateAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            object callbackObject;
            _manualBarrierControlRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(paymentStatus = null);
            _manualBarrierControlRepositoryMock.Setup(x => x.UpdateAsync(It.IsNotNull<ManualBarrierControlModel>())).Callback<object>(k => callbackObject = k);

            // Act
            var service = new ManualBarrierControlsService(_loggerMock.Object, _manualBarrierControlRepositoryMock.Object, _mapper);
            var result = await service.UpdateAsync(id, updateRequest);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Count(x => x.Code == 404).Should().Be(1);
            _manualBarrierControlRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _manualBarrierControlRepositoryMock.Verify(x => x.UpdateAsync(It.IsNotNull<ManualBarrierControlModel>()), Times.Never);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.UpdateAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.UpdateAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndRepositoryIsDown_WhenUpdateAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            _manualBarrierControlRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(paymentStatus);
            _manualBarrierControlRepositoryMock.Setup(x => x.UpdateAsync(It.IsNotNull<ManualBarrierControlModel>())).ThrowsAsync(new Exception());

            // Act
            var service = new ManualBarrierControlsService(_loggerMock.Object, _manualBarrierControlRepositoryMock.Object, _mapper);
            Func<Task> func = () => service.UpdateAsync(id, updateRequest);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            _manualBarrierControlRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _manualBarrierControlRepositoryMock.Verify(x => x.UpdateAsync(It.IsNotNull<ManualBarrierControlModel>()), Times.Once);
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
            _manualBarrierControlRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(paymentStatus);
            _manualBarrierControlRepositoryMock.Setup(x => x.RemoveAsync(It.IsNotNull<ManualBarrierControlModel>())).Callback<object>(k => callbackObject = k);

            // Act
            var service = new ManualBarrierControlsService(_loggerMock.Object, _manualBarrierControlRepositoryMock.Object, _mapper);
            var result = await service.RemoveAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeTrue();
            _manualBarrierControlRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _manualBarrierControlRepositoryMock.Verify(x => x.RemoveAsync(It.IsNotNull<ManualBarrierControlModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.RemoveAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.RemoveAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndNonExistingGuid_WhenRemoveAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            _manualBarrierControlRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(paymentStatus = null);

            // Act
            var service = new ManualBarrierControlsService(_loggerMock.Object, _manualBarrierControlRepositoryMock.Object, _mapper);
            var result = await service.RemoveAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Count(x => x.Code == 404).Should().Be(1);
            _manualBarrierControlRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _manualBarrierControlRepositoryMock.Verify(x => x.RemoveAsync(It.IsNotNull<ManualBarrierControlModel>()), Times.Never);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.RemoveAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.RemoveAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndRepositoryIsDown_WhenRemoveAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            _manualBarrierControlRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ThrowsAsync(new Exception());

            // Act
            var service = new ManualBarrierControlsService(_loggerMock.Object, _manualBarrierControlRepositoryMock.Object, _mapper);
            Func<Task> func = () => service.RemoveAsync(id);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            _manualBarrierControlRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _manualBarrierControlRepositoryMock.Verify(x => x.RemoveAsync(It.IsNotNull<ManualBarrierControlModel>()), Times.Never);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.RemoveAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.RemoveAsync)} method", Times.Once, _exception);
        }
        #endregion
        #region GetByIdAsync
        [Fact]
        public async Task GivenValidRequest_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _manualBarrierControlRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(paymentStatus);

            // Act
            var service = new ManualBarrierControlsService(_loggerMock.Object, _manualBarrierControlRepositoryMock.Object, _mapper);
            var result = await service.GetByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            _manualBarrierControlRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.GetByIdAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestButNotFound_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _manualBarrierControlRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync((ManualBarrierControlModel)null!);

            // Act
            var service = new ManualBarrierControlsService(_loggerMock.Object, _manualBarrierControlRepositoryMock.Object, _mapper);
            var result = await service.GetByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
            _manualBarrierControlRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.GetByIdAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndRepositoryIsDown_WhenGetByIdAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            _manualBarrierControlRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ThrowsAsync(new Exception());

            // Act
            var service = new ManualBarrierControlsService(_loggerMock.Object, _manualBarrierControlRepositoryMock.Object, _mapper);
            Func<Task> func = () => service.GetByIdAsync(id);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            _manualBarrierControlRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.GetByIdAsync)} method", Times.Once, _exception);
        }
        #endregion
    }
}
