using AutoMapper;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Vehicle;
using EPAY.ETC.Core.API.Infrastructure.Services.Vehicles;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Common;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Services
{
    public class VehicleServiceTests : AutoMapperTestBase
    {
        #region Init mock data
        private readonly Mock<ILogger<VehicleService>> _loggerMock = new();
        private readonly Mock<IVehicleRepository> _repositoryMock = new();
        private static Guid id = Guid.NewGuid();
        private VehicleModel _request = new VehicleModel()
        {
            Id = id,
            CreatedDate = DateTime.Now,
            PlateNumber = "Some Plate number",
            PlateColor = "Some Plate colour",
            RFID = "Some RFID",
            Make = "Some make",
            Seat = 10,
            VehicleType = "Loại 2",
            Weight = 7000,
        };
        #endregion

        #region AddAsync
        [Fact]
        public async Task GivenValidRequest_WhenAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _repositoryMock.Setup(x => x.AddAsync(It.IsNotNull<VehicleModel>())).ReturnsAsync(_request);

            // Act
            var service = new VehicleService(_loggerMock.Object, _repositoryMock.Object, _mapper);
            var result = await service.AddAsync(_request);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            _repositoryMock.Verify(x => x.AddAsync(It.IsNotNull<VehicleModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.AddAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.AddAsync)} method", Times.Never, null);
        }

        [Fact]
        public async Task GivenValidRequestAndExistingVehicle_WhenAddAsyncIsCalled_ThenReturnConflict()
        {
            // Arrange
            _repositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<VehicleModel, bool>>>())).ReturnsAsync(new List<VehicleModel>() { new VehicleModel() });
            _repositoryMock.Setup(x => x.AddAsync(It.IsNotNull<VehicleModel>())).ReturnsAsync(_request);

            // Act
            var service = new VehicleService(_loggerMock.Object, _repositoryMock.Object, _mapper);
            var result = await service.AddAsync(_request);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Count(x => x.Code == StatusCodes.Status409Conflict).Should().Be(1);
            _repositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<VehicleModel, bool>>>()), Times.Once);
            _repositoryMock.Verify(x => x.AddAsync(It.IsNotNull<VehicleModel>()), Times.Never);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.AddAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.AddAsync)} method", Times.Never, null);
        }

        [Fact]
        public async Task GivenValidRequestAndRepositoryIsDown_WhenAddAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            _repositoryMock.Setup(x => x.AddAsync(It.IsNotNull<VehicleModel>())).ThrowsAsync(new Exception());

            // Act
            var service = new VehicleService(_loggerMock.Object, _repositoryMock.Object, _mapper);
            Func<Task> func = () => service.AddAsync(_request);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            _repositoryMock.Verify(x => x.AddAsync(It.IsNotNull<VehicleModel>()), Times.Once);
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
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(_request);
            _repositoryMock.Setup(x => x.UpdateAsync(It.IsNotNull<VehicleModel>())).Callback<object>(k => callbackObject = k);

            // Act
            var service = new VehicleService(_loggerMock.Object, _repositoryMock.Object, _mapper);
            var result = await service.UpdateAsync(_request);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            _repositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _repositoryMock.Verify(x => x.UpdateAsync(It.IsNotNull<VehicleModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.UpdateAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.UpdateAsync)} method", Times.Never, null);
        }
        [Fact]
        public async Task GivenValidRequestAndNonExistingSettingGuid_WhenUpdateAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            Object callbackObject;
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync((VehicleModel)null);
            _repositoryMock.Setup(x => x.UpdateAsync(It.IsNotNull<VehicleModel>())).Callback<object>(k => callbackObject = k);

            // Act
            var service = new VehicleService(_loggerMock.Object, _repositoryMock.Object, _mapper);
            var result = await service.UpdateAsync(_request);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Count(x => x.Code == 404).Should().Be(1);
            _repositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _repositoryMock.Verify(x => x.UpdateAsync(It.IsNotNull<VehicleModel>()), Times.Never);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.UpdateAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.UpdateAsync)} method", Times.Never, null);
        }

        [Fact]
        public async Task GivenValidRequestAndRepositoryIsDown_WhenUpdateAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(_request);
            _repositoryMock.Setup(x => x.UpdateAsync(It.IsNotNull<VehicleModel>())).ThrowsAsync(new Exception());

            // Act
            var service = new VehicleService(_loggerMock.Object, _repositoryMock.Object, _mapper);
            Func<Task> func = () => service.UpdateAsync(_request);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            _repositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _repositoryMock.Verify(x => x.UpdateAsync(It.IsNotNull<VehicleModel>()), Times.Once);
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
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(_request);
            _repositoryMock.Setup(x => x.RemoveAsync(It.IsNotNull<VehicleModel>())).Callback<object>(k => callbackObject = k);

            // Act
            var service = new VehicleService(_loggerMock.Object, _repositoryMock.Object, _mapper);
            var result = await service.RemoveAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeEmpty();
            result.Succeeded.Should().BeTrue();
            _repositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _repositoryMock.Verify(x => x.RemoveAsync(It.IsNotNull<VehicleModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.RemoveAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.RemoveAsync)} method", Times.Never, null);
        }

        [Fact]
        public async Task GivenValidRequestAndNonExistingSettingGuid_WhenRemoveAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync((VehicleModel)null);

            // Act
            var service = new VehicleService(_loggerMock.Object, _repositoryMock.Object, _mapper);
            var result = await service.RemoveAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeEmpty();
            result.Succeeded.Should().BeFalse();
            result.Errors.Count(x => x.Code == 404).Should().Be(1);
            _repositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _repositoryMock.Verify(x => x.RemoveAsync(It.IsNotNull<VehicleModel>()), Times.Never);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.RemoveAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.RemoveAsync)} method", Times.Never, null);
        }

        [Fact]
        public async Task GivenValidRequestAndRepositoryIsDown_WhenRemoveAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ThrowsAsync(new Exception());

            // Act
            var service = new VehicleService(_loggerMock.Object, _repositoryMock.Object, _mapper);
            Func<Task> func = () => service.RemoveAsync(id);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            _repositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _repositoryMock.Verify(x => x.RemoveAsync(It.IsNotNull<VehicleModel>()), Times.Never);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.RemoveAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.RemoveAsync)} method", Times.Once, null);
        }
        #endregion

        #region GetByIdAsync
        [Fact]
        public async Task GivenValidRequest_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(_request);

            // Act
            var service = new VehicleService(_loggerMock.Object, _repositoryMock.Object, _mapper);
            var result = await service.GetByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            _repositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetByIdAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.GetByIdAsync)} method", Times.Never, null);
        }

        [Fact]
        public async Task GivenValidRequestButNotFound_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync((VehicleModel)null);

            // Act
            var service = new VehicleService(_loggerMock.Object, _repositoryMock.Object, _mapper);
            var result = await service.GetByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
            _repositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetByIdAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.GetByIdAsync)} method", Times.Never, null);
        }

        [Fact]
        public async Task GivenValidRequestAndRepositoryIsDown_WhenGetByIdAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ThrowsAsync(new Exception());

            // Act
            var service = new VehicleService(_loggerMock.Object, _repositoryMock.Object, _mapper);
            Func<Task> func = () => service.GetByIdAsync(id);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            _repositoryMock.Verify(x => x.GetByIdAsync(It.IsNotNull<Guid>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.GetByIdAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.GetByIdAsync)} method", Times.Once, null);
        }
        #endregion
    }
}
