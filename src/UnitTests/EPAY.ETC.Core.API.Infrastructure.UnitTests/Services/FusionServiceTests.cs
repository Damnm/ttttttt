using Azure.Core;
using EPAY.ETC.Core.API.Core.Models.Common;
using EPAY.ETC.Core.API.Core.Models.Fusion;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Fusion;
using EPAY.ETC.Core.API.Infrastructure.Services.Fusion;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Common;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Services
{
    public class FusionServiceTests : AutoMapperTestBase
    {
        #region Init mock data
        private readonly Mock<ILogger<FusionService>> _loggerMock = new();
        private readonly Mock<IFusionRepository> _fusionRepositoryMock = new();
        private static Guid id = Guid.NewGuid();
        private FusionRequestModel request = new FusionRequestModel()
        {
            Epoch = 100,           
            Loop1 = true,
            RFID = false,
            Cam1 = "12A12345",
            Loop2 = true,
            Cam2 = "12A12345",
            Loop3 = true,
            ReversedLoop1 = true,
            ReversedLoop2 = true
        };
        private FusionModel fusion = new FusionModel()
        {
            Id = id,
            CreatedDate = DateTime.Now,
            Epoch = 100,
            Loop1 = true,
            RFID = false,
            Cam1 = "12A12345",
            Loop2 = true,
            Cam2 = "12A12345",
            Loop3 = true,
            ReversedLoop1 = true,
            ReversedLoop2 = true
        };
        #endregion
        #region AddAsync
        [Fact]
        public async Task GivenValidRequest_WhenAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _fusionRepositoryMock.Setup(x => x.AddAsync(It.IsNotNull<FusionModel>())).ReturnsAsync(fusion);

            // Act
            var service = new FusionService(_loggerMock.Object, _fusionRepositoryMock.Object, _mapper);
            var result = await service.AddAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            _fusionRepositoryMock.Verify(x => x.AddAsync(It.IsNotNull<FusionModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.AddAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.AddAsync)} method", Times.Never, null);
        }

        [Fact(Skip = "Skip")]
        public async Task GivenValidRequestAndExistingFusion_WhenAddAsyncIsCalled_ThenReturnConflict()
        {
            // Arrange

            _fusionRepositoryMock.Setup(x => x.AddAsync(It.IsNotNull<FusionModel>())).ReturnsAsync(fusion);

            // Act
            var service = new FusionService(_loggerMock.Object, _fusionRepositoryMock.Object, _mapper);
            var result = await service.AddAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Count(x => x.Code == StatusCodes.Status409Conflict).Should().Be(1);
            _fusionRepositoryMock.Verify(x => x.AddAsync(It.IsNotNull<FusionModel>()), Times.Never);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.AddAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.AddAsync)} method", Times.Never, null);
        }

        [Fact]
        public async Task GivenValidRequestAndRepositoryIsDown_WhenAddAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            _fusionRepositoryMock.Setup(x => x.AddAsync(It.IsNotNull<FusionModel>())).ThrowsAsync(new Exception());

            // Act
            var service = new FusionService(_loggerMock.Object, _fusionRepositoryMock.Object, _mapper);
            Func<Task> func = () => service.AddAsync(request);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            _fusionRepositoryMock.Verify(x => x.AddAsync(It.IsNotNull<FusionModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.AddAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.AddAsync)} method", Times.Once, null);
        }
        #endregion

        #region UpdateAsync
        [Fact]
        public async Task GivenValidRequest_WhenUpdateAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            Object callbackObject;
            _fusionRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(fusion);
            _fusionRepositoryMock.Setup(x => x.UpdateAsync(It.IsNotNull<FusionModel>())).Callback<object>(k => callbackObject = k);

            // Act
            var service = new FusionService(_loggerMock.Object, _fusionRepositoryMock.Object, _mapper);
            var result = await service.UpdateAsync(id, request);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            _fusionRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _fusionRepositoryMock.Verify(x => x.UpdateAsync(It.IsNotNull<FusionModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.UpdateAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.UpdateAsync)} method", Times.Never, null);
        }
        [Fact]
        public async Task GivenValidRequestAndNonExistingGuid_WhenUpdateAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            Object callbackObject;
            _fusionRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(fusion = null);
            _fusionRepositoryMock.Setup(x => x.UpdateAsync(It.IsNotNull<FusionModel>())).Callback<object>(k => callbackObject = k);

            // Act
            var service = new FusionService(_loggerMock.Object, _fusionRepositoryMock.Object, _mapper);
            var result = await service.UpdateAsync(id, request);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Count(x => x.Code == 404).Should().Be(1);
            _fusionRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _fusionRepositoryMock.Verify(x => x.UpdateAsync(It.IsNotNull<FusionModel>()), Times.Never);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.UpdateAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.UpdateAsync)} method", Times.Never, null);
        }

        [Fact]
        public async Task GivenValidRequestAndRepositoryIsDown_WhenUpdateAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            _fusionRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(fusion);
            _fusionRepositoryMock.Setup(x => x.UpdateAsync(It.IsNotNull<FusionModel>())).ThrowsAsync(new Exception());

            // Act
            var service = new FusionService(_loggerMock.Object, _fusionRepositoryMock.Object, _mapper);
            Func<Task> func = () => service.UpdateAsync(id, request);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            _fusionRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _fusionRepositoryMock.Verify(x => x.UpdateAsync(It.IsNotNull<FusionModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.UpdateAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.UpdateAsync)} method", Times.Once, null);
        }
        #endregion

        #region RemoveAsync
        [Fact]
        public async Task GivenValidRequest_WhenRemoveAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            Object callbackObject;
            _fusionRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(fusion);
            _fusionRepositoryMock.Setup(x => x.RemoveAsync(It.IsNotNull<FusionModel>())).Callback<object>(k => callbackObject = k);

            // Act
            var service = new FusionService(_loggerMock.Object, _fusionRepositoryMock.Object, _mapper);
            var result = await service.RemoveAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeTrue();
            _fusionRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _fusionRepositoryMock.Verify(x => x.RemoveAsync(It.IsNotNull<FusionModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.RemoveAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.RemoveAsync)} method", Times.Never, null);
        }

        [Fact]
        public async Task GivenValidRequestAndNonExistingGuid_WhenRemoveAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            _fusionRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(fusion = null);

            // Act
            var service = new FusionService(_loggerMock.Object, _fusionRepositoryMock.Object, _mapper);
            var result = await service.RemoveAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Count(x => x.Code == 404).Should().Be(1);
            _fusionRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _fusionRepositoryMock.Verify(x => x.RemoveAsync(It.IsNotNull<FusionModel>()), Times.Never);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.RemoveAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.RemoveAsync)} method", Times.Never, null);
        }

        [Fact]
        public async Task GivenValidRequestAndRepositoryIsDown_WhenRemoveAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            _fusionRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ThrowsAsync(new Exception());

            // Act
            var service = new FusionService(_loggerMock.Object, _fusionRepositoryMock.Object, _mapper);
            Func<Task> func = () => service.RemoveAsync(id);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            _fusionRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _fusionRepositoryMock.Verify(x => x.RemoveAsync(It.IsNotNull<FusionModel>()), Times.Never);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.RemoveAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.RemoveAsync)} method", Times.Once, null);
        }
        #endregion

        #region GetByIdAsync
        [Fact]
        public async Task GivenValidRequest_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _fusionRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(fusion);

            // Act
            var service = new FusionService(_loggerMock.Object, _fusionRepositoryMock.Object, _mapper);
            var result = await service.GetByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            _fusionRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetByIdAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.GetByIdAsync)} method", Times.Never, null);
        }

        [Fact]
        public async Task GivenValidRequestButNotFound_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _fusionRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync((FusionModel)null);

            // Act
            var service = new FusionService(_loggerMock.Object, _fusionRepositoryMock.Object, _mapper);
            var result = await service.GetByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
            _fusionRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetByIdAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.GetByIdAsync)} method", Times.Never, null);
        }

        [Fact]
        public async Task GivenValidRequestAndRepositoryIsDown_WhenGetByIdAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            _fusionRepositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ThrowsAsync(new Exception());

            // Act
            var service = new FusionService(_loggerMock.Object, _fusionRepositoryMock.Object, _mapper);
            Func<Task> func = () => service.GetByIdAsync(id);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            _fusionRepositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetByIdAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.GetByIdAsync)} method", Times.Once, null);
        }
        #endregion
    }
}