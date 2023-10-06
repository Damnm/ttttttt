using EPAY.ETC.Core.API.Controllers.Fusion;
using EPAY.ETC.Core.API.Controllers.ManualBarrierControls;
using EPAY.ETC.Core.API.Controllers.Payment;
using EPAY.ETC.Core.API.Core.Interfaces.Services.ManualBarrierControls;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Payment;
using EPAY.ETC.Core.API.Core.Models.Fusion;
using EPAY.ETC.Core.API.Core.Models.ManualBarrierControl;
using EPAY.ETC.Core.API.UnitTests.Helpers;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.UnitTests.Controllers.ManualBarrierControls
{
    public class ManualBarrierControlsControllerTests : ControllerBase
    {
        private readonly Exception _exception = null!;
        private Mock<IManualBarrierControlsService> _manualBarrierControlsServiceMock = new Mock<IManualBarrierControlsService>();
        private Mock<ILogger<ManualBarrierControlController>> _loggerMock = new Mock<ILogger<ManualBarrierControlController>>();
        private static Guid id = Guid.NewGuid();
        private ValidationResult<ManualBarrierControlModel> responseMock = new ValidationResult<ManualBarrierControlModel>(new ManualBarrierControlModel()
        {
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.Now,
            EmployeeId ="sdasd132a4d564",
            Action = BarrierActionEnum.Close,
            LaneOutId = "6666"

        });
        private ManualBarrierControlAddOrUpdateRequestModel addRequestMock = new ManualBarrierControlAddOrUpdateRequestModel()
        {
            EmployeeId = "sdasd132a4d564",
            Action = BarrierActionEnum.Open,
            LaneOutId = "6666"
        };
        private ManualBarrierControlAddOrUpdateRequestModel updateRequestMock = new ManualBarrierControlAddOrUpdateRequestModel()
        {
            EmployeeId = "sdasd132a4d564",
            Action = BarrierActionEnum.Open,
            LaneOutId = "2754"
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
            _manualBarrierControlsServiceMock.Setup(x => x.AddAsync(It.IsAny<ManualBarrierControlAddOrUpdateRequestModel>())).ReturnsAsync(responseMock);

            // Act
            var manualBarrierControlController = new ManualBarrierControlController(_loggerMock.Object, _manualBarrierControlsServiceMock.Object);
            var actualResult = await manualBarrierControlController.AddAsync(It.IsAny<ManualBarrierControlAddOrUpdateRequestModel>());
            var data = ((CreatedResult)actualResult).Value as ValidationResult<ManualBarrierControlModel>;

            // Assert
            _loggerMock.VerifyLog(Microsoft.Extensions.Logging.LogLevel.Information, $"Executing {nameof(manualBarrierControlController.AddAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(manualBarrierControlController.AddAsync)} method", Times.Never, _exception);
            actualResult.Should().BeOfType<CreatedResult>();
            ((CreatedResult)actualResult).StatusCode.Should().Be(StatusCodes.Status201Created);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().NotBeNull();          
        }
        [Fact]
        public async Task GivenValidRequestAndSettingsAlreadyExists_WhenAddAsyncIsCalled_ThenReturnConflict()
        {
            // Arrange
            responseMock = new ValidationResult<ManualBarrierControlModel>(null!, new List<ValidationError>()
            {
                ValidationError.Conflict
            });
            _manualBarrierControlsServiceMock.Setup(x => x.AddAsync(It.IsAny<ManualBarrierControlAddOrUpdateRequestModel>())).ReturnsAsync(responseMock);

            // Act
            var manualBarrierControlController = new ManualBarrierControlController(_loggerMock.Object, _manualBarrierControlsServiceMock.Object);
            var actualResult = await manualBarrierControlController.AddAsync(It.IsAny<ManualBarrierControlAddOrUpdateRequestModel>());
            var data = actualResult as ConflictObjectResult;
            var actualResultRespone = data!.Value as ValidationResult<ManualBarrierControlModel>;
            // Assert
            _manualBarrierControlsServiceMock.Verify(x => x.AddAsync(It.IsAny<ManualBarrierControlAddOrUpdateRequestModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(manualBarrierControlController.AddAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(manualBarrierControlController.AddAsync)} method", Times.Never, _exception);
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
            _manualBarrierControlsServiceMock.Setup(x => x.AddAsync(It.IsAny<ManualBarrierControlAddOrUpdateRequestModel>())).ThrowsAsync(someEx);

            // Act
            var ManualBarrierControlController = new ManualBarrierControlController(_loggerMock.Object, _manualBarrierControlsServiceMock.Object);
            var actualResult = await ManualBarrierControlController.AddAsync(It.IsAny<ManualBarrierControlAddOrUpdateRequestModel>());
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(ManualBarrierControlController.AddAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(ManualBarrierControlController.AddAsync)} method", Times.Once, _exception);
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
            responseMock.Data!.Action = updateRequestMock.Action;
            responseMock.Data!.LaneOutId = updateRequestMock.LaneOutId;
            responseMock.Data!.EmployeeId = updateRequestMock.EmployeeId;

            _manualBarrierControlsServiceMock.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ManualBarrierControlAddOrUpdateRequestModel>())).ReturnsAsync(responseMock);

            // Act
            var manualBarrierControlController = new ManualBarrierControlController(_loggerMock.Object
                , _manualBarrierControlsServiceMock.Object);
            var actualResult = await manualBarrierControlController.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ManualBarrierControlAddOrUpdateRequestModel>());
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<ManualBarrierControlModel>;
            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(manualBarrierControlController.UpdateAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(manualBarrierControlController.UpdateAsync)} method", Times.Never, _exception);
            actualResult.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().NotBeNull();
            data?.Data?.EmployeeId.Should().Be(updateRequestMock.EmployeeId);
            data?.Data?.Action.Should().Be(updateRequestMock.Action);
            data?.Data?.LaneOutId.Should().Be(updateRequestMock.LaneOutId);
            

        }
        [Fact]
        public async Task GivenValidRequestAndNonExistingVehicleId_WhenApiUpdateAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            var request = new ManualBarrierControlModel()
            {
                Id = Guid.NewGuid()
            };
            responseMock = new ValidationResult<ManualBarrierControlModel>(new List<ValidationError>()
            {
                ValidationError.NotFound
            });
            _manualBarrierControlsServiceMock.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ManualBarrierControlAddOrUpdateRequestModel>())).ReturnsAsync(responseMock);

            // Act
            var manualBarrierControlController = new ManualBarrierControlController(_loggerMock.Object
                , _manualBarrierControlsServiceMock.Object);
            var actualResult = await manualBarrierControlController.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ManualBarrierControlAddOrUpdateRequestModel>());
            var data = actualResult as NotFoundObjectResult;
            var actualResultRespone = data!.Value as ValidationResult<ManualBarrierControlModel>;

            // Assert
            _manualBarrierControlsServiceMock.Verify(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ManualBarrierControlAddOrUpdateRequestModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(manualBarrierControlController.UpdateAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(manualBarrierControlController.UpdateAsync)} method", Times.Never, _exception);
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status404NotFound);
            actualResultRespone!.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone.Errors.Count(x => x.Code == StatusCodes.Status404NotFound) > 0);
        }
        // Unhappy case 400
        [Fact]
        public async Task GivenValidRequestAndVehicleServiceIsDown_WhenApiUpdateAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            var request = new ManualBarrierControlModel()
            {
                Id = Guid.NewGuid()
            };
            var someEx = new Exception("An error occurred when calling UpdateAsync method");
            _manualBarrierControlsServiceMock.Setup(x => x.UpdateAsync(Guid.NewGuid(), new ManualBarrierControlAddOrUpdateRequestModel())).ThrowsAsync(someEx);

            // Act
            var manualBarrierControlController = new ManualBarrierControlController(_loggerMock.Object
                , _manualBarrierControlsServiceMock.Object);
            var actualResult = await manualBarrierControlController.UpdateAsync(Guid.NewGuid(), It.IsAny<ManualBarrierControlAddOrUpdateRequestModel>());
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(manualBarrierControlController.UpdateAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(manualBarrierControlController.UpdateAsync)} method", Times.Once, _exception);
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
            var responseMock = new ValidationResult<ManualBarrierControlModel>(new List<ValidationError>());
            _manualBarrierControlsServiceMock.Setup(x => x.RemoveAsync(It.IsNotNull<Guid>())).ReturnsAsync(responseMock);

            // Act
            var ManualBarrierControlController = new ManualBarrierControlController(_loggerMock.Object, _manualBarrierControlsServiceMock.Object);
            var actualResult = await ManualBarrierControlController.RemoveAsync(It.IsNotNull<Guid>());
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<ManualBarrierControlModel>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(ManualBarrierControlController.RemoveAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(ManualBarrierControlController.RemoveAsync)} method", Times.Never, _exception);
            actualResult.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().BeNull();
        }

        [Fact]
        public async Task GivenValidRequestAndNonExistingSettingsGuid_WhenRemoveAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            var responseMock = new ValidationResult<ManualBarrierControlModel>(new List<ValidationError>()
            {
                ValidationError.NotFound
            });
            _manualBarrierControlsServiceMock.Setup(x => x.RemoveAsync(It.IsNotNull<Guid>())).ReturnsAsync(responseMock);

            // Act
            var ManualBarrierControlController = new ManualBarrierControlController(_loggerMock.Object, _manualBarrierControlsServiceMock.Object);
            var actualResult = await ManualBarrierControlController.RemoveAsync(It.IsNotNull<Guid>());
            var data = actualResult as NotFoundObjectResult;
            var actualResultRespone = data!.Value as ValidationResult<ManualBarrierControlModel>;
            // Assert
            _manualBarrierControlsServiceMock.Verify(x => x.RemoveAsync(It.IsNotNull<Guid>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(ManualBarrierControlController.RemoveAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(ManualBarrierControlController.RemoveAsync)} method", Times.Never, _exception);
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
            _manualBarrierControlsServiceMock.Setup(x => x.RemoveAsync(It.IsNotNull<Guid>())).ThrowsAsync(someEx);

            // Act
            var ManualBarrierControlController = new ManualBarrierControlController(_loggerMock.Object, _manualBarrierControlsServiceMock.Object);
            var actualResult = await ManualBarrierControlController.RemoveAsync(It.IsNotNull<Guid>());
            var data = ((ObjectResult)actualResult).Value as ValidationResult<FusionAddRequestModel>;
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(ManualBarrierControlController.RemoveAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(ManualBarrierControlController.RemoveAsync)} method", Times.Once, _exception);
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
            responseMock.Data.Action = BarrierActionEnum.Open;
            _manualBarrierControlsServiceMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(responseMock);

            // Act
            var ManualBarrierControlController = new ManualBarrierControlController(_loggerMock.Object, _manualBarrierControlsServiceMock.Object);
            var actualResult = await ManualBarrierControlController.GetByIdAsync(It.IsNotNull<Guid>());
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<ManualBarrierControlModel>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(ManualBarrierControlController.GetByIdAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(ManualBarrierControlController.GetByIdAsync)} method", Times.Never, _exception);
            actualResult.Should().BeOfType<OkObjectResult>();
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().NotBeNull();
            data?.Data?.EmployeeId.Should().Be(addRequestMock.EmployeeId);
            data?.Data?.Action.Should().Be(addRequestMock.Action);
            data?.Data?.LaneOutId.Should().Be(addRequestMock.LaneOutId);
        }

        // Unhappy case 400
        [Fact]
        public async Task GivenValidRequestAndSettingsServiceIsDown_WhenGetByIdAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            var someEx = new Exception("An error occurred when calling GetByIdAsync method");
            _manualBarrierControlsServiceMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ThrowsAsync(someEx);

            // Act
            var ManualBarrierControlController = new ManualBarrierControlController(_loggerMock.Object, _manualBarrierControlsServiceMock.Object);
            var actualResult = await ManualBarrierControlController.GetByIdAsync(It.IsNotNull<Guid>());
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(ManualBarrierControlController.GetByIdAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(ManualBarrierControlController.GetByIdAsync)} method", Times.Once, _exception);
            ((ObjectResult)actualResult).StatusCode.Should().Be(500);
            actualResultRespone?.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone?.Errors.Count() > 0);
        }
        #endregion

    }
}
