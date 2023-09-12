using EPAY.ETC.Core.API.Controllers;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Fusion;
using EPAY.ETC.Core.API.Core.Models.Common;
using EPAY.ETC.Core.API.Core.Models.Fusion;
using EPAY.ETC.Core.API.Core.Validation;
using EPAY.ETC.Core.API.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPAY.ETC.Core.API.UnitTests.Controllers.Fusions
{
    public class FusionControllerTests : ControllerBase
    {
        private readonly Exception _exception = null!;
        private Mock<IFusionService> _fusionServiceMock = new Mock<IFusionService>();
        private Mock<ILogger<FusionController>> _loggerMock = new Mock<ILogger<FusionController>>();
        private ValidationResult<FusionModel> responseMock = new ValidationResult<FusionModel>(new FusionModel()
        {
            Id = Guid.NewGuid(),
            Epoch = 100,
            Loop1 = true,
            RFID = false,
            Cam1 = "12A12345",
            Loop2 = true,
            Cam2 = "12A12345",
            Loop3 = true,
            ReversedLoop1 = true,
            ReversedLoop2 = true
        });
        private FusionRequestModel requestMock = new FusionRequestModel()
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
        #region AddAsync
        // Happy case 200/201
        [Fact]
        public void GiveValidRequest_WhenApiAddAsyncIsCalled_ThenReturnGoodValidation()
        {
            //arrange

            //act
            var actualResult = ValidateModelTest.ValidateModel(responseMock);

            //assert 
            actualResult.Should().NotBeNull();
            Assert.True(actualResult.Count() == 0);
        }
        [Fact]
        public async Task GivenValidRequest_WhenAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _fusionServiceMock.Setup(x => x.AddAsync(It.IsAny<FusionRequestModel>())).ReturnsAsync(responseMock);

            // Act
            var fusionsController = new FusionController(_loggerMock.Object, _fusionServiceMock.Object);
            var actualResult = await fusionsController.AddAsync(It.IsAny<FusionRequestModel>());
            var data = ((ObjectResult)actualResult).Value as ValidationResult<FusionModel>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(fusionsController.AddAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(fusionsController.AddAsync)} method", Times.Never, _exception);
            actualResult.Should().BeOfType<ObjectResult>();
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status201Created);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().NotBeNull();
            data?.Data.Epoch.Should().Be(requestMock.Epoch);
            data?.Data.Loop1.Should().Be(requestMock.Loop1);
            data?.Data.RFID.Should().Be(requestMock.RFID);
            data?.Data.Cam1.Should().Be(requestMock.Cam1);
            data?.Data.Loop2.Should().Be(requestMock.Loop2);
            data?.Data.Cam2.Should().Be(requestMock.Cam2);
            data?.Data.Loop3.Should().Be(requestMock.Loop3);
            data?.Data.ReversedLoop1.Should().Be(requestMock.ReversedLoop1);
            data?.Data.ReversedLoop2.Should().Be(requestMock.ReversedLoop2);
        }
        [Fact]
        public async Task GivenValidRequestAndSettingsAlreadyExists_WhenAddAsyncIsCalled_ThenReturnConflict()
        {
            // Arrange
            responseMock = new ValidationResult<FusionModel>(null!, new List<ValidationError>()
            {
                ValidationError.Conflict
            });
            _fusionServiceMock.Setup(x => x.AddAsync(It.IsAny<FusionRequestModel>())).ReturnsAsync(responseMock);

            // Act
            var fusionsController = new FusionController(_loggerMock.Object, _fusionServiceMock.Object);
            var actualResult = await fusionsController.AddAsync(It.IsAny<FusionRequestModel>());
            var data = actualResult as ConflictObjectResult;
            var actualResultRespone = data!.Value as ValidationResult<FusionModel>;
            // Assert
            _fusionServiceMock.Verify(x => x.AddAsync(It.IsAny<FusionRequestModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(fusionsController.AddAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(fusionsController.AddAsync)} method", Times.Never, _exception);
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status409Conflict);
            actualResultRespone!.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone.Errors.Count(x => x.Code == StatusCodes.Status409Conflict) > 0);
        }
        // Unhappy case 400
        [Fact]
        public async Task GivenValidRequestAndSettingsServiceIsDown_WhenAddAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            var someEx = new Exception("An error occurred when calling AddAsync method");
            _fusionServiceMock.Setup(x => x.AddAsync(It.IsAny<FusionRequestModel>())).ThrowsAsync(someEx);

            // Act
            var fusionsController = new FusionController(_loggerMock.Object, _fusionServiceMock.Object);
            var actualResult = await fusionsController.AddAsync(It.IsAny<FusionRequestModel>());
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(fusionsController.AddAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(fusionsController.AddAsync)} method", Times.Once, _exception);
            ((ObjectResult)actualResult).StatusCode.Should().Be(500);
            actualResultRespone!.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone.Errors.Count() > 0);

        }
        #endregion
        #region UpdateAsync
        // Happy case 200
        [Fact]
        public async Task GivenValidRequest_WhenUpdateAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _fusionServiceMock.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<FusionRequestModel>())).ReturnsAsync(responseMock);

            // Act
            var vehicleController = new FusionController(_loggerMock.Object
                , _fusionServiceMock.Object);
            var actualResult = await vehicleController.UpdateAsync(It.IsAny<Guid>(), It.IsAny<FusionRequestModel>());
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<FusionModel>;
            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleController.UpdateAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleController.UpdateAsync)} method", Times.Never, _exception);
            actualResult.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().NotBeNull();
            data?.Data.Epoch.Should().Be(requestMock.Epoch);
            data?.Data.Loop1.Should().Be(requestMock.Loop1);
            data?.Data.RFID.Should().Be(requestMock.RFID);
            data?.Data.Cam1.Should().Be(requestMock.Cam1);
            data?.Data.Loop2.Should().Be(requestMock.Loop2);
            data?.Data.Cam2.Should().Be(requestMock.Cam2);
            data?.Data.Loop3.Should().Be(requestMock.Loop3);
            data?.Data.ReversedLoop1.Should().Be(requestMock.ReversedLoop1);
            data?.Data.ReversedLoop2.Should().Be(requestMock.ReversedLoop2);

        }
        [Fact]
        public async Task GivenValidRequestAndNonExistingVehicleId_WhenApiUpdateAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            var request = new FusionModel()
            {
                Id = Guid.NewGuid()
            };
            responseMock = new ValidationResult<FusionModel>(new List<ValidationError>()
            {
                ValidationError.NotFound
            });
            _fusionServiceMock.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<FusionRequestModel>())).ReturnsAsync(responseMock);

            // Act
            var vehicleController = new FusionController(_loggerMock.Object
                , _fusionServiceMock.Object);
            var actualResult = await vehicleController.UpdateAsync(It.IsAny<Guid>(), It.IsAny<FusionRequestModel>());
            var data = actualResult as NotFoundObjectResult;
            var actualResultRespone = data!.Value as ValidationResult<FusionModel>;

            // Assert
            _fusionServiceMock.Verify(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<FusionRequestModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleController.UpdateAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleController.UpdateAsync)} method", Times.Never, _exception);
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status404NotFound);
            actualResultRespone!.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone.Errors.Count(x => x.Code == StatusCodes.Status404NotFound) > 0);
        }
        // Unhappy case 400
        [Fact]
        public async Task GivenValidRequestAndVehicleServiceIsDown_WhenApiUpdateAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            var request = new FusionModel()
            {
                Id = Guid.NewGuid()
            };
            var someEx = new Exception("An error occurred when calling UpdateAsync method");
            _fusionServiceMock.Setup(x => x.UpdateAsync(Guid.NewGuid(), new FusionRequestModel())).ThrowsAsync(someEx);

            // Act
            var vehicleController = new FusionController(_loggerMock.Object
                , _fusionServiceMock.Object);
            var actualResult = await vehicleController.UpdateAsync(Guid.NewGuid(), It.IsAny<FusionRequestModel>());
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleController.UpdateAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleController.UpdateAsync)} method", Times.Once, _exception);
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            actualResultRespone!.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone.Errors.Count() > 0);
        }
        #endregion
        #region RemoveAsync
        // Happy case 200
        [Fact]
        public async Task GivenValidRequest_WhenRemoveAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var responseMock = new ValidationResult<FusionModel>(new List<ValidationError>());
            _fusionServiceMock.Setup(x => x.RemoveAsync(It.IsNotNull<Guid>())).ReturnsAsync(responseMock);

            // Act
            var fusionController = new FusionController(_loggerMock.Object, _fusionServiceMock.Object);
            var actualResult = await fusionController.RemoveAsync(It.IsNotNull<Guid>());
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<FusionModel>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(fusionController.RemoveAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(fusionController.RemoveAsync)} method", Times.Never, _exception);
            actualResult.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().BeNull();
        }

        [Fact]
        public async Task GivenValidRequestAndNonExistingSettingsGuid_WhenRemoveAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            var responseMock = new ValidationResult<FusionModel>(new List<ValidationError>()
            {
                ValidationError.NotFound
            });
            _fusionServiceMock.Setup(x => x.RemoveAsync(It.IsNotNull<Guid>())).ReturnsAsync(responseMock);

            // Act
            var fusionController = new FusionController(_loggerMock.Object, _fusionServiceMock.Object);
            var actualResult = await fusionController.RemoveAsync(It.IsNotNull<Guid>());
            var data = actualResult as NotFoundObjectResult;
            var actualResultRespone = data!.Value as ValidationResult<FusionModel>;
            // Assert
            _fusionServiceMock.Verify(x => x.RemoveAsync(It.IsNotNull<Guid>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(fusionController.RemoveAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(fusionController.RemoveAsync)} method", Times.Never, _exception);
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status404NotFound);
            actualResultRespone!.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone.Errors.Count(x => x.Code == StatusCodes.Status404NotFound) > 0);
        }

        // Unhappy case 400
        [Fact]
        public async Task GivenValidRequestAndSettingsServiceIsDown_WhenRemoveAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            var someEx = new Exception("An error occurred when calling RemoveAsync method");
            _fusionServiceMock.Setup(x => x.RemoveAsync(It.IsNotNull<Guid>())).ThrowsAsync(someEx);

            // Act
            var fusionController = new FusionController(_loggerMock.Object, _fusionServiceMock.Object);
            var actualResult = await fusionController.RemoveAsync(It.IsNotNull<Guid>());
            var data = ((ObjectResult)actualResult).Value as ValidationResult<FusionRequestModel>;
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(fusionController.RemoveAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(fusionController.RemoveAsync)} method", Times.Once, _exception);
            ((ObjectResult)actualResult).StatusCode.Should().Be(500);
            actualResultRespone!.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone.Errors.Count() > 0);
        }
        #endregion
        #region GetByIdAsync
        // Happy case 200
        [Fact]
        public async Task GivenValidRequest_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _fusionServiceMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(responseMock);

            // Act
            var fusionController = new FusionController(_loggerMock.Object, _fusionServiceMock.Object);
            var actualResult = await fusionController.GetByIdAsync(It.IsNotNull<Guid>());
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<FusionModel>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(fusionController.GetByIdAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(fusionController.GetByIdAsync)} method", Times.Never, _exception);
            actualResult.Should().BeOfType<OkObjectResult>();
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().NotBeNull();
            data?.Data.Epoch.Should().Be(requestMock.Epoch);
            data?.Data.Loop1.Should().Be(requestMock.Loop1);
            data?.Data.RFID.Should().Be(requestMock.RFID);
            data?.Data.Cam1.Should().Be(requestMock.Cam1);
            data?.Data.Loop2.Should().Be(requestMock.Loop2);
            data?.Data.Cam2.Should().Be(requestMock.Cam2);
            data?.Data.Loop3.Should().Be(requestMock.Loop3);
            data?.Data.ReversedLoop1.Should().Be(requestMock.ReversedLoop1);
            data?.Data.ReversedLoop2.Should().Be(requestMock.ReversedLoop2);
        }

        // Unhappy case 400
        [Fact]
        public async Task GivenValidRequestAndSettingsServiceIsDown_WhenGetByIdAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            var someEx = new Exception("An error occurred when calling GetByIdAsync method");
            _fusionServiceMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ThrowsAsync(someEx);

            // Act
            var fusionController = new FusionController(_loggerMock.Object, _fusionServiceMock.Object);
            var actualResult = await fusionController.GetByIdAsync(It.IsNotNull<Guid>());
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(fusionController.GetByIdAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(fusionController.GetByIdAsync)} method", Times.Once, _exception);
            ((ObjectResult)actualResult).StatusCode.Should().Be(500);
            actualResultRespone?.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone?.Errors.Count() > 0);
        }
        #endregion
    }
}
