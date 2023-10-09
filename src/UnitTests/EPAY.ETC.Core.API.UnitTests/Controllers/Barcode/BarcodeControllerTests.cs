using EPAY.ETC.Core.API.Controllers.Barcode;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Barcode;
using EPAY.ETC.Core.API.Core.Models.Barcode;
using EPAY.ETC.Core.API.UnitTests.Common;
using EPAY.ETC.Core.API.UnitTests.Helpers;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPAY.ETC.Core.API.UnitTests.Controllers.Barcode
{
    public class BarcodeControllerTests : TestBase<BarcodeController>
    {
        private readonly Exception _exception = null!;
        private Mock<IBarcodeService> _barcodeServiceMock = new Mock<IBarcodeService>();
        private static Guid id = Guid.NewGuid();
        private ValidationResult<BarcodeModel> responseMock = new ValidationResult<BarcodeModel>(new BarcodeModel()
        {
            ActionCode = "111",
            ActionDesc = "Open Barrier",
            EmployeeId = "23232"
        });
        private BarcodeAddOrUpdateRequestModel addRequestMock = new BarcodeAddOrUpdateRequestModel()
        {
            ActionCode = "111",
            ActionDesc = "Open Barrier",
            EmployeeId = "23232"
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
            _barcodeServiceMock.Setup(x => x.AddAsync(It.IsAny<BarcodeAddOrUpdateRequestModel>())).ReturnsAsync(responseMock);
            // Act
            var barcodeController = new BarcodeController(_loggerMock.Object, _barcodeServiceMock.Object, _mapper);
            var actualResult = await barcodeController.AddAsync(It.IsAny<BarcodeAddOrUpdateRequestModel>());
            var data = ((ObjectResult)actualResult).Value as ValidationResult<BarcodeModel>;

            // Assert
            _loggerMock.VerifyLog(Microsoft.Extensions.Logging.LogLevel.Information, $"Executing {nameof(barcodeController.AddAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(barcodeController.AddAsync)} method", Times.Never, _exception);
            actualResult.Should().BeOfType<ObjectResult>();
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status201Created);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().NotBeNull();
            data?.Data?.ActionCode.Should().Be(addRequestMock.ActionCode);
            data?.Data?.ActionDesc.Should().Be(addRequestMock.ActionDesc);
            data?.Data?.EmployeeId.Should().Be(addRequestMock.EmployeeId);
        }
        [Fact]
        public async Task GivenValidRequestAndSettingsAlreadyExists_WhenAddAsyncIsCalled_ThenReturnConflict()
        {
            // Arrange
            responseMock = new ValidationResult<BarcodeModel>(null!, new List<ValidationError>()
            {
                ValidationError.Conflict
            });
            _barcodeServiceMock.Setup(x => x.AddAsync(It.IsAny<BarcodeAddOrUpdateRequestModel>())).ReturnsAsync(responseMock);

            // Act
            var barcodeController = new BarcodeController(_loggerMock.Object, _barcodeServiceMock.Object, _mapper);
            var actualResult = await barcodeController.AddAsync(It.IsAny<BarcodeAddOrUpdateRequestModel>());
            var data = actualResult as ConflictObjectResult;
            var actualResultRespone = data!.Value as ValidationResult<BarcodeModel>;
            // Assert
            _barcodeServiceMock.Verify(x => x.AddAsync(It.IsAny<BarcodeAddOrUpdateRequestModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(barcodeController.AddAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(barcodeController.AddAsync)} method", Times.Never, _exception);
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
            _barcodeServiceMock.Setup(x => x.AddAsync(It.IsAny<BarcodeAddOrUpdateRequestModel>())).ThrowsAsync(someEx);

            // Act
            var barcodeController = new BarcodeController(_loggerMock.Object, _barcodeServiceMock.Object, _mapper);
            var actualResult = await barcodeController.AddAsync(It.IsAny<BarcodeAddOrUpdateRequestModel>());
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(barcodeController.AddAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(barcodeController.AddAsync)} method", Times.Once, _exception);
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
            _barcodeServiceMock.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<BarcodeAddOrUpdateRequestModel>())).ReturnsAsync(responseMock);

            // Act
            var vehicleController = new BarcodeController(_loggerMock.Object, _barcodeServiceMock.Object, _mapper);
            var actualResult = await vehicleController.UpdateAsync(It.IsAny<string>(), It.IsAny<BarcodeAddOrUpdateRequestModel>());
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<BarcodeModel>;
            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleController.UpdateAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleController.UpdateAsync)} method", Times.Never, _exception);
            actualResult.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().NotBeNull();
            data?.Data?.ActionCode.Should().Be(addRequestMock.ActionCode);
            data?.Data?.ActionDesc.Should().Be(addRequestMock.ActionDesc);
            data?.Data?.EmployeeId.Should().Be(addRequestMock.EmployeeId);

        }

        // Unhappy case 400
        [Fact]
        public async Task GivenValidRequestAndServiceIsDown_WhenApiUpdateAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            var request = new BarcodeModel()
            {
                Id = Guid.NewGuid()
            };
            var someEx = new Exception("An error occurred when calling UpdateAsync method");
            _barcodeServiceMock.Setup(x => x.UpdateAsync(Guid.NewGuid(), new BarcodeAddOrUpdateRequestModel())).ThrowsAsync(someEx);

            // Act
            var vehicleController = new BarcodeController(_loggerMock.Object, _barcodeServiceMock.Object, _mapper);
            var actualResult = await vehicleController.UpdateAsync("", It.IsAny<BarcodeAddOrUpdateRequestModel>());
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
            var responseMock = new ValidationResult<BarcodeModel>(new List<ValidationError>());
            _barcodeServiceMock.Setup(x => x.RemoveAsync(It.IsNotNull<Guid>())).ReturnsAsync(responseMock);

            // Act
            var barcodeController = new BarcodeController(_loggerMock.Object, _barcodeServiceMock.Object, _mapper);
            var actualResult = await barcodeController.RemoveAsync(It.IsNotNull<string>());
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<BarcodeModel>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(barcodeController.RemoveAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(barcodeController.RemoveAsync)} method", Times.Never, _exception);
            actualResult.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().BeNull();
        }

        [Fact]
        public async Task GivenValidRequestAndNonExistingSettingsGuid_WhenRemoveAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            var responseMock = new ValidationResult<BarcodeModel>(new List<ValidationError>()
            {
                ValidationError.NotFound
            });
            _barcodeServiceMock.Setup(x => x.RemoveAsync(It.IsNotNull<Guid>())).ReturnsAsync(responseMock);

            // Act
            var barcodeController = new BarcodeController(_loggerMock.Object, _barcodeServiceMock.Object, _mapper);
            var actualResult = await barcodeController.RemoveAsync(It.IsNotNull<string>());
            var data = actualResult as NotFoundObjectResult;
            var actualResultRespone = data!.Value as ValidationResult<BarcodeModel>;
            // Assert
            _barcodeServiceMock.Verify(x => x.RemoveAsync(It.IsNotNull<Guid>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(barcodeController.RemoveAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(barcodeController.RemoveAsync)} method", Times.Never, _exception);
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
            _barcodeServiceMock.Setup(x => x.RemoveAsync(It.IsNotNull<Guid>())).ThrowsAsync(someEx);

            // Act
            var barcodeController = new BarcodeController(_loggerMock.Object, _barcodeServiceMock.Object, _mapper);
            var actualResult = await barcodeController.RemoveAsync(It.IsNotNull<string>());
            var data = ((ObjectResult)actualResult).Value as ValidationResult<BarcodeAddOrUpdateRequestModel>;
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(barcodeController.RemoveAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(barcodeController.RemoveAsync)} method", Times.Once, _exception);
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
            _barcodeServiceMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(responseMock);

            // Act
            var barcodeController = new BarcodeController(_loggerMock.Object, _barcodeServiceMock.Object, _mapper);
            var actualResult = await barcodeController.GetByIdAsync(It.IsNotNull<string>());
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<BarcodeModel>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(barcodeController.GetByIdAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(barcodeController.GetByIdAsync)} method", Times.Never, _exception);
            actualResult.Should().BeOfType<OkObjectResult>();
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().NotBeNull();
            data?.Data?.ActionCode.Should().Be(addRequestMock.ActionCode);
            data?.Data?.ActionDesc.Should().Be(addRequestMock.ActionDesc);
            data?.Data?.EmployeeId.Should().Be(addRequestMock.EmployeeId);
        }

        // Unhappy case 400
        [Fact]
        public async Task GivenValidRequestAndSettingsServiceIsDown_WhenGetByIdAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            var someEx = new Exception("An error occurred when calling GetByIdAsync method");
            _barcodeServiceMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ThrowsAsync(someEx);

            // Act
            var barcodeController = new BarcodeController(_loggerMock.Object, _barcodeServiceMock.Object, _mapper);
            var actualResult = await barcodeController.GetByIdAsync(It.IsNotNull<string>());
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(barcodeController.GetByIdAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(barcodeController.GetByIdAsync)} method", Times.Once, _exception);
            ((ObjectResult)actualResult).StatusCode.Should().Be(500);
            actualResultRespone?.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone?.Errors.Count() > 0);
        }
        #endregion
    }
}
