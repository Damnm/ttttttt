using EPAY.ETC.Core.API.Controllers.PrintLog;
using EPAY.ETC.Core.API.Core.Interfaces.Services.PrintLog;
using EPAY.ETC.Core.API.Core.Models.PrintLog;
using EPAY.ETC.Core.API.UnitTests.Helpers;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace EPAY.ETC.Core.API.UnitTests.Controllers.PrintLog
{
    public class PrintLogControllerTests : ControllerBase
    {
        private readonly Exception _exception = null!;
        private Mock<IPrintLogService> _printLogServiceMock = new Mock<IPrintLogService>();
        private Mock<ILogger<PrintLogController>> _loggerMock = new Mock<ILogger<PrintLogController>>();
        private PrintLogModel requestMock = new PrintLogModel()
        {
            PlateNumber = "Some Plate number",
           
        };
        private ValidationResult<PrintLogModel> responseMock = new ValidationResult<PrintLogModel>(new PrintLogModel()
        {
            Id = new Guid(),
            CreatedDate = DateTime.Now,
            PlateNumber = "Some Plate number",
           
        });
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
        public async Task GivenValidRequest_WhenApiAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _printLogServiceMock.Setup(x => x.AddAsync(It.IsAny<PrintLogRequestModel>())).ReturnsAsync(responseMock);
            var request = new PrintLogModel();

            // Act
            var PrintLogController = new PrintLogController(_loggerMock.Object
                , _printLogServiceMock.Object);
            var actualResult = await PrintLogController.AddAsync(It.IsAny<PrintLogRequestModel>());
            var data = ((ObjectResult)actualResult).Value as ValidationResult<PrintLogModel>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PrintLogController.AddAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(PrintLogController.AddAsync)} method", Times.Never, _exception);
            actualResult.Should().BeOfType<ObjectResult>();
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status201Created);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().NotBeNull();
        }

        // Unhappy case 400
        [Fact]
        public async Task GivenValidRequestAndVehicleServiceIsDown_WhenApiAddAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            var someEx = new Exception("An error occurred when calling AddAsync method");
            _printLogServiceMock.Setup(x => x.AddAsync(It.IsAny<PrintLogRequestModel>())).ThrowsAsync(someEx);

            // Act
            var PrintLogController = new PrintLogController(_loggerMock.Object
                , _printLogServiceMock.Object);
            var actualResult = await PrintLogController.AddAsync(It.IsAny<PrintLogRequestModel>());
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PrintLogController.AddAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(PrintLogController.AddAsync)} method", Times.Once, _exception);
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
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
            _printLogServiceMock.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<PrintLogRequestModel>())).ReturnsAsync(responseMock);

            // Act
            var PrintLogController = new PrintLogController(_loggerMock.Object
                , _printLogServiceMock.Object);
            var actualResult = await PrintLogController.UpdateAsync(It.IsAny<Guid>(), It.IsAny<PrintLogRequestModel>());
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<PrintLogModel>;
            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PrintLogController.UpdateAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(PrintLogController.UpdateAsync)} method", Times.Never, _exception);
            actualResult.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().NotBeNull();
            data?.Data?.PlateNumber.Should().Be(responseMock.Data?.PlateNumber);
                   }
        [Fact]
        public async Task GivenValidRequestAndNonExistingVehicleId_WhenApiUpdateAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            var request = new PrintLogModel()
            {
                Id = Guid.NewGuid()
            };
            responseMock = new ValidationResult<PrintLogModel>(new List<ValidationError>()
            {
                ValidationError.NotFound
            });
            _printLogServiceMock.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<PrintLogRequestModel>())).ReturnsAsync(responseMock);

            // Act
            var PrintLogController = new PrintLogController(_loggerMock.Object
                , _printLogServiceMock.Object);
            var actualResult = await PrintLogController.UpdateAsync(It.IsAny<Guid>(), It.IsAny<PrintLogRequestModel>());
            var data = actualResult as NotFoundObjectResult;
            var actualResultRespone = data!.Value as ValidationResult<PrintLogModel>;

            // Assert
            _printLogServiceMock.Verify(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<PrintLogRequestModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PrintLogController.UpdateAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(PrintLogController.UpdateAsync)} method", Times.Never, _exception);
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status404NotFound);
            actualResultRespone!.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone.Errors.Count(x => x.Code == StatusCodes.Status404NotFound) > 0);
        }
        // Unhappy case 400
        [Fact]
        public async Task GivenValidRequestAndVehicleServiceIsDown_WhenApiUpdateAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            var request = new PrintLogModel()
            {
                Id = Guid.NewGuid()
            };
            var someEx = new Exception("An error occurred when calling UpdateAsync method");
            _printLogServiceMock.Setup(x => x.UpdateAsync(Guid.NewGuid(), new PrintLogRequestModel())).ThrowsAsync(someEx);

            // Act
            var PrintLogController = new PrintLogController(_loggerMock.Object
                , _printLogServiceMock.Object);
            var actualResult = await PrintLogController.UpdateAsync(Guid.NewGuid(), It.IsAny<PrintLogRequestModel>());
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PrintLogController.UpdateAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(PrintLogController.UpdateAsync)} method", Times.Once, _exception);
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            actualResultRespone!.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone.Errors.Count() > 0);
        }
        #endregion
        #region RemoveAsync
        // Happy case 200
        [Fact]
        public async Task GivenValidRequest_WhenApiRemoveAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var responseMock = new ValidationResult<PrintLogModel>(new List<ValidationError>());
            _printLogServiceMock.Setup(x => x.RemoveAsync(It.IsNotNull<Guid>())).ReturnsAsync(responseMock);

            // Act
            var PrintLogController = new PrintLogController(_loggerMock.Object
                , _printLogServiceMock.Object);
            var actualResult = await PrintLogController.RemoveAsync(It.IsNotNull<Guid>());
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<PrintLogModel>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PrintLogController.RemoveAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(PrintLogController.RemoveAsync)} method", Times.Never, _exception);
            actualResult.Should().BeOfType<OkObjectResult>();
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().BeNull();
        }
        [Fact]
        public async Task GivenValidRequestAndNonExistingVehicleId_WhenApiRemoveAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            var responseMock = new ValidationResult<PrintLogModel>(new List<ValidationError>()

            {
                ValidationError.NotFound
            });
            _printLogServiceMock.Setup(x => x.RemoveAsync(It.IsNotNull<Guid>())).ReturnsAsync(responseMock);

            // Act
            var PrintLogController = new PrintLogController(_loggerMock.Object
                , _printLogServiceMock.Object);
            var actualResult = await PrintLogController.RemoveAsync(It.IsNotNull<Guid>());
            var data = actualResult as NotFoundObjectResult;
            var actualResultRespone = data!.Value as ValidationResult<PrintLogModel>;
            // Assert
            _printLogServiceMock.Verify(x => x.RemoveAsync(It.IsNotNull<Guid>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PrintLogController.RemoveAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(PrintLogController.RemoveAsync)} method", Times.Never, _exception);
            ((NotFoundObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status404NotFound);
            actualResultRespone!.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone.Errors.Count(x => x.Code == StatusCodes.Status404NotFound) > 0);
        }
        // Unhappy case 400
        [Fact]
        public async Task GivenValidRequestAndVehicleServiceIsDown_WhenApiRemoveAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            var someEx = new Exception("An error occurred when calling RemoveAsync method");
            _printLogServiceMock.Setup(x => x.RemoveAsync(It.IsNotNull<Guid>())).ThrowsAsync(new Exception("Some exception"));

            // Act
            var PrintLogController = new PrintLogController(_loggerMock.Object
                , _printLogServiceMock.Object);
            var actualResult = await PrintLogController.RemoveAsync(It.IsNotNull<Guid>());
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PrintLogController.RemoveAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(PrintLogController.RemoveAsync)} method", Times.Once, _exception);
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            actualResultRespone!.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone.Errors.Count(x => x.Code == StatusCodes.Status500InternalServerError) > 0);
        }
        #endregion
        #region GetByIdAsync
        [Fact]
        public async Task GivenValidRequest_WhenApiGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _printLogServiceMock.Setup(x => x.GetByIdAsync(new Guid())).ReturnsAsync(responseMock);

            // Act
            var PrintLogController = new PrintLogController(_loggerMock.Object
                , _printLogServiceMock.Object);
            var actualResult = await PrintLogController.GetByIdAsync(new Guid());
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<PrintLogModel>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PrintLogController.GetByIdAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(PrintLogController.GetByIdAsync)} method", Times.Never, _exception);
            actualResult.Should().BeOfType<OkObjectResult>();
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data?.Succeeded.Should().BeTrue();
            data?.Data?.Should().NotBeNull();
            data?.Data?.PlateNumber.Should().Be(responseMock.Data?.PlateNumber);
           
        }
        // Unhappy case 400
        [Fact]
        public async Task GivenValidRequestAndPriorityVehicleServiceIsDown_WhenApiGetByIdAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            var someEx = new Exception("An error occurred when calling GetByIdAsync method");
            _printLogServiceMock.Setup(x => x.GetByIdAsync(new Guid())).ThrowsAsync(someEx);

            // Act
            var PrintLogController = new PrintLogController(_loggerMock.Object
                , _printLogServiceMock.Object);
            var actualResult = await PrintLogController.GetByIdAsync(new Guid());
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PrintLogController.GetByIdAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(PrintLogController.GetByIdAsync)} method", Times.Once, _exception);
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            actualResultRespone!.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone.Errors.Count() > 0);
        }
        #endregion
    }
}
