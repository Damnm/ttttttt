using EPAY.ETC.Core.API.Controllers.PaymentStatus;
using EPAY.ETC.Core.API.Core.Interfaces.Services.PaymentStatus;
using EPAY.ETC.Core.API.UnitTests.Helpers;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using PaymentStatusModel = EPAY.ETC.Core.API.Core.Models.PaymentStatus.PaymentStatusModel;
namespace EPAY.ETC.Core.API.UnitTests.Controllers.PaymentStatus
{
    public class PaymentStatusControllerTests : ControllerBase
    {
        private readonly Exception _exception = null!;
        private Mock<IPaymentStatusService> _paymentStatusServiceMock = new Mock<IPaymentStatusService>();
        private Mock<ILogger<PaymentStatusController>> _loggerMock = new Mock<ILogger<PaymentStatusController>>();
        private static Guid id = Guid.NewGuid();
        private ValidationResult<PaymentStatusModel> responseMock = new ValidationResult<PaymentStatusModel>(new PaymentStatusModel()
        {
            Id = Guid.NewGuid(),
            PaymentId = id,
            Amount = 300,
            Currency = "vnd",
        });
        private PaymentStatusAddRequestModel addRequestMock = new PaymentStatusAddRequestModel()
        {
            PaymentId = id,
            Amount = 300,
            Currency = "vnd",
        };
        private PaymentStatusUpdateRequestModel updateRequestMock = new PaymentStatusUpdateRequestModel()
        {
            PaymentId = Guid.NewGuid(),
            Amount = 300,
            Currency = "vnd",
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
            _paymentStatusServiceMock.Setup(x => x.AddAsync(It.IsAny<PaymentStatusAddRequestModel>())).ReturnsAsync(responseMock);

            // Act
            var paymentStatusController = new PaymentStatusController(_loggerMock.Object, _paymentStatusServiceMock.Object);
            var actualResult = await paymentStatusController.AddAsync(It.IsAny<PaymentStatusAddRequestModel>());
            var data = ((ObjectResult)actualResult).Value as ValidationResult<PaymentStatusModel>;

            // Assert
            _loggerMock.VerifyLog(Microsoft.Extensions.Logging.LogLevel.Information, $"Executing {nameof(paymentStatusController.AddAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(paymentStatusController.AddAsync)} method", Times.Never, _exception);
            actualResult.Should().BeOfType<ObjectResult>();
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status201Created);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().NotBeNull();
            data?.Data?.Amount.Should().Be(addRequestMock.Amount);
            data?.Data?.Currency.Should().Be(addRequestMock.Currency);
            data?.Data?.PaymentId.Should().Be(addRequestMock.PaymentId);
        }
        [Fact]
        public async Task GivenValidRequestAndSettingsAlreadyExists_WhenAddAsyncIsCalled_ThenReturnConflict()
        {
            // Arrange
            responseMock = new ValidationResult<PaymentStatusModel>(null!, new List<ValidationError>()
            {
                ValidationError.Conflict
            });
            _paymentStatusServiceMock.Setup(x => x.AddAsync(It.IsAny<PaymentStatusAddRequestModel>())).ReturnsAsync(responseMock);

            // Act
            var paymentStatusController = new PaymentStatusController(_loggerMock.Object, _paymentStatusServiceMock.Object);
            var actualResult = await paymentStatusController.AddAsync(It.IsAny<PaymentStatusAddRequestModel>());
            var data = actualResult as ConflictObjectResult;
            var actualResultRespone = data!.Value as ValidationResult<PaymentStatusModel>;
            // Assert
            _paymentStatusServiceMock.Verify(x => x.AddAsync(It.IsAny<PaymentStatusAddRequestModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(paymentStatusController.AddAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(paymentStatusController.AddAsync)} method", Times.Never, _exception);
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
            _paymentStatusServiceMock.Setup(x => x.AddAsync(It.IsAny<PaymentStatusAddRequestModel>())).ThrowsAsync(someEx);

            // Act
            var paymentStatusController = new PaymentStatusController(_loggerMock.Object, _paymentStatusServiceMock.Object);
            var actualResult = await paymentStatusController.AddAsync(It.IsAny<PaymentStatusAddRequestModel>());
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(paymentStatusController.AddAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(paymentStatusController.AddAsync)} method", Times.Once, _exception);
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
            _paymentStatusServiceMock.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<PaymentStatusUpdateRequestModel>())).ReturnsAsync(responseMock);

            // Act
            var vehicleController = new PaymentStatusController(_loggerMock.Object
                , _paymentStatusServiceMock.Object);
            var actualResult = await vehicleController.UpdateAsync(It.IsAny<Guid>(), It.IsAny<PaymentStatusUpdateRequestModel>());
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<PaymentStatusModel>;
            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleController.UpdateAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleController.UpdateAsync)} method", Times.Never, _exception);
            actualResult.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().NotBeNull();
            data?.Data?.Amount.Should().Be(addRequestMock.Amount);
            data?.Data?.Currency.Should().Be(addRequestMock.Currency);
            data?.Data?.PaymentId.Should().Be(addRequestMock.PaymentId);

        }
        [Fact]
        public async Task GivenValidRequestAndNonExistingVehicleId_WhenApiUpdateAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            var request = new PaymentStatusModel()
            {
                Id = Guid.NewGuid()
            };
            responseMock = new ValidationResult<PaymentStatusModel>(new List<ValidationError>()
            {
                ValidationError.NotFound
            });
            _paymentStatusServiceMock.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<PaymentStatusUpdateRequestModel>())).ReturnsAsync(responseMock);

            // Act
            var vehicleController = new PaymentStatusController(_loggerMock.Object
                , _paymentStatusServiceMock.Object);
            var actualResult = await vehicleController.UpdateAsync(It.IsAny<Guid>(), It.IsAny<PaymentStatusUpdateRequestModel>());
            var data = actualResult as NotFoundObjectResult;
            var actualResultRespone = data!.Value as ValidationResult<PaymentStatusModel>;

            // Assert
            _paymentStatusServiceMock.Verify(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<PaymentStatusUpdateRequestModel>()), Times.Once);
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
            var request = new PaymentStatusModel()
            {
                Id = Guid.NewGuid()
            };
            var someEx = new Exception("An error occurred when calling UpdateAsync method");
            _paymentStatusServiceMock.Setup(x => x.UpdateAsync(Guid.NewGuid(), new PaymentStatusUpdateRequestModel())).ThrowsAsync(someEx);

            // Act
            var vehicleController = new PaymentStatusController(_loggerMock.Object
                , _paymentStatusServiceMock.Object);
            var actualResult = await vehicleController.UpdateAsync(Guid.NewGuid(), It.IsAny<PaymentStatusUpdateRequestModel>());
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
            var responseMock = new ValidationResult<PaymentStatusModel>(new List<ValidationError>());
            _paymentStatusServiceMock.Setup(x => x.RemoveAsync(It.IsNotNull<Guid>())).ReturnsAsync(responseMock);

            // Act
            var PaymentStatusController = new PaymentStatusController(_loggerMock.Object, _paymentStatusServiceMock.Object);
            var actualResult = await PaymentStatusController.RemoveAsync(It.IsNotNull<Guid>());
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<PaymentStatusModel>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PaymentStatusController.RemoveAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(PaymentStatusController.RemoveAsync)} method", Times.Never, _exception);
            actualResult.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().BeNull();
        }

        [Fact]
        public async Task GivenValidRequestAndNonExistingSettingsGuid_WhenRemoveAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            var responseMock = new ValidationResult<PaymentStatusModel>(new List<ValidationError>()
            {
                ValidationError.NotFound
            });
            _paymentStatusServiceMock.Setup(x => x.RemoveAsync(It.IsNotNull<Guid>())).ReturnsAsync(responseMock);

            // Act
            var PaymentStatusController = new PaymentStatusController(_loggerMock.Object, _paymentStatusServiceMock.Object);
            var actualResult = await PaymentStatusController.RemoveAsync(It.IsNotNull<Guid>());
            var data = actualResult as NotFoundObjectResult;
            var actualResultRespone = data!.Value as ValidationResult<PaymentStatusModel>;
            // Assert
            _paymentStatusServiceMock.Verify(x => x.RemoveAsync(It.IsNotNull<Guid>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PaymentStatusController.RemoveAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(PaymentStatusController.RemoveAsync)} method", Times.Never, _exception);
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
            _paymentStatusServiceMock.Setup(x => x.RemoveAsync(It.IsNotNull<Guid>())).ThrowsAsync(someEx);

            // Act
            var PaymentStatusController = new PaymentStatusController(_loggerMock.Object, _paymentStatusServiceMock.Object);
            var actualResult = await PaymentStatusController.RemoveAsync(It.IsNotNull<Guid>());
            var data = ((ObjectResult)actualResult).Value as ValidationResult<PaymentStatusAddRequestModel>;
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PaymentStatusController.RemoveAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(PaymentStatusController.RemoveAsync)} method", Times.Once, _exception);
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
            _paymentStatusServiceMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(responseMock);

            // Act
            var PaymentStatusController = new PaymentStatusController(_loggerMock.Object, _paymentStatusServiceMock.Object);
            var actualResult = await PaymentStatusController.GetByIdAsync(It.IsNotNull<Guid>());
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<PaymentStatusModel>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PaymentStatusController.GetByIdAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(PaymentStatusController.GetByIdAsync)} method", Times.Never, _exception);
            actualResult.Should().BeOfType<OkObjectResult>();
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().NotBeNull();
            data?.Data?.Amount.Should().Be(addRequestMock.Amount);
            data?.Data?.Currency.Should().Be(addRequestMock.Currency);
            data?.Data?.PaymentId.Should().Be(addRequestMock.PaymentId);
        }

        // Unhappy case 400
        [Fact]
        public async Task GivenValidRequestAndSettingsServiceIsDown_WhenGetByIdAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            var someEx = new Exception("An error occurred when calling GetByIdAsync method");
            _paymentStatusServiceMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ThrowsAsync(someEx);

            // Act
            var PaymentStatusController = new PaymentStatusController(_loggerMock.Object, _paymentStatusServiceMock.Object);
            var actualResult = await PaymentStatusController.GetByIdAsync(It.IsNotNull<Guid>());
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PaymentStatusController.GetByIdAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(PaymentStatusController.GetByIdAsync)} method", Times.Once, _exception);
            ((ObjectResult)actualResult).StatusCode.Should().Be(500);
            actualResultRespone?.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone?.Errors.Count() > 0);
        }
        #endregion
    }
}
