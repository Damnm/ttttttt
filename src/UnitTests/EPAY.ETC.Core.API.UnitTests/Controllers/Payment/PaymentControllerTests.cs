using EPAY.ETC.Core.API.Controllers.PaymentStatus;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Payment;
using EPAY.ETC.Core.API.UnitTests.Helpers;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using PaymentModel = EPAY.ETC.Core.API.Core.Models.Payment.PaymentModel;
namespace EPAY.ETC.Core.API.UnitTests.Controllers.Payment
{
    public class PaymentControllerTests : ControllerBase
    {
        private readonly Exception _exception = null!;
        private Mock<IPaymentService> _paymentServiceMock = new Mock<IPaymentService>();
        private Mock<ILogger<PaymentController>> _loggerMock = new Mock<ILogger<PaymentController>>();
        private static Guid id = Guid.NewGuid();
        private ValidationResult<PaymentModel> responseMock = new ValidationResult<PaymentModel>(new PaymentModel()
        {
            Id = Guid.NewGuid(),
            LaneInId = "1",
            LaneOutId ="1",
            RFID = "dfsdfdsfds",
            Make = "Toyota",
            Amount = 300
           
        });
        private PaymentAddRequestModel addRequestMock = new PaymentAddRequestModel()
        {
            PaymentId = Guid.NewGuid(),
            LaneInId = "1",
            LaneOutId = "1",
            RFID = "dfsdfdsfds",
            Make = "Toyota",
            Amount = 300
        };
        private PaymentUpdateRequestModel updateRequestMock = new PaymentUpdateRequestModel()
        {
            PaymentId = Guid.NewGuid(),
            LaneInId = "1",
            LaneOutId = "1",
            RFID = "dfsdfdsfds",
            Make = "Toyota",
            Amount = 300
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
            _paymentServiceMock.Setup(x => x.AddAsync(It.IsAny<PaymentAddRequestModel>())).ReturnsAsync(responseMock);

            // Act
            var paymentController = new PaymentController(_loggerMock.Object, _paymentServiceMock.Object);
            var actualResult = await paymentController.AddAsync(It.IsAny<PaymentAddRequestModel>());
            var data = ((ObjectResult)actualResult).Value as ValidationResult<PaymentModel>;

            // Assert
            _loggerMock.VerifyLog(Microsoft.Extensions.Logging.LogLevel.Information, $"Executing {nameof(paymentController.AddAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(paymentController.AddAsync)} method", Times.Never, _exception);
            actualResult.Should().BeOfType<ObjectResult>();
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status201Created);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().NotBeNull();
            data?.Data.Amount.Should().Be(addRequestMock.Amount);
            data?.Data.LaneInId.Should().Be(addRequestMock.LaneInId);
            data?.Data.LaneOutId.Should().Be(addRequestMock.LaneOutId);
            data?.Data.Make.Should().Be(addRequestMock.Make);
        }
        [Fact]
        public async Task GivenValidRequestAndSettingsAlreadyExists_WhenAddAsyncIsCalled_ThenReturnConflict()
        {
            // Arrange
            responseMock = new ValidationResult<PaymentModel>(null!, new List<ValidationError>()
            {
                ValidationError.Conflict
            });
            _paymentServiceMock.Setup(x => x.AddAsync(It.IsAny<PaymentAddRequestModel>())).ReturnsAsync(responseMock);

            // Act
            var paymentController = new PaymentController(_loggerMock.Object, _paymentServiceMock.Object);
            var actualResult = await paymentController.AddAsync(It.IsAny<PaymentAddRequestModel>());
            var data = actualResult as ConflictObjectResult;
            var actualResultRespone = data!.Value as ValidationResult<PaymentModel>;
            // Assert
            _paymentServiceMock.Verify(x => x.AddAsync(It.IsAny<PaymentAddRequestModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(paymentController.AddAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(paymentController.AddAsync)} method", Times.Never, _exception);
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
            _paymentServiceMock.Setup(x => x.AddAsync(It.IsAny<PaymentAddRequestModel>())).ThrowsAsync(someEx);

            // Act
            var paymentController = new PaymentController(_loggerMock.Object, _paymentServiceMock.Object);
            var actualResult = await paymentController.AddAsync(It.IsAny<PaymentAddRequestModel>());
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(paymentController.AddAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(paymentController.AddAsync)} method", Times.Once, _exception);
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
            _paymentServiceMock.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<PaymentUpdateRequestModel>())).ReturnsAsync(responseMock);

            // Act
            var vehicleController = new PaymentController(_loggerMock.Object
                , _paymentServiceMock.Object);
            var actualResult = await vehicleController.UpdateAsync(It.IsAny<Guid>(), It.IsAny<PaymentUpdateRequestModel>());
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<PaymentModel>;
            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleController.UpdateAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleController.UpdateAsync)} method", Times.Never, _exception);
            actualResult.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().NotBeNull();
            data?.Data.Amount.Should().Be(addRequestMock.Amount);
            data?.Data.LaneInId.Should().Be(addRequestMock.LaneInId);
            data?.Data.LaneOutId.Should().Be(addRequestMock.LaneOutId);
            data?.Data.Make.Should().Be(addRequestMock.Make);

        }
        [Fact]
        public async Task GivenValidRequestAndNonExistingVehicleId_WhenApiUpdateAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            var request = new PaymentModel()
            {
                Id = Guid.NewGuid()
            };
            responseMock = new ValidationResult<PaymentModel>(new List<ValidationError>()
            {
                ValidationError.NotFound
            });
            _paymentServiceMock.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<PaymentUpdateRequestModel>())).ReturnsAsync(responseMock);

            // Act
            var vehicleController = new PaymentController(_loggerMock.Object
                , _paymentServiceMock.Object);
            var actualResult = await vehicleController.UpdateAsync(It.IsAny<Guid>(), It.IsAny<PaymentUpdateRequestModel>());
            var data = actualResult as NotFoundObjectResult;
            var actualResultRespone = data!.Value as ValidationResult<PaymentModel>;

            // Assert
            _paymentServiceMock.Verify(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<PaymentUpdateRequestModel>()), Times.Once);
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
            var request = new PaymentModel()
            {
                Id = Guid.NewGuid()
            };
            var someEx = new Exception("An error occurred when calling UpdateAsync method");
            _paymentServiceMock.Setup(x => x.UpdateAsync(Guid.NewGuid(), new PaymentUpdateRequestModel())).ThrowsAsync(someEx);

            // Act
            var vehicleController = new PaymentController(_loggerMock.Object
                , _paymentServiceMock.Object);
            var actualResult = await vehicleController.UpdateAsync(Guid.NewGuid(), It.IsAny<PaymentUpdateRequestModel>());
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
            var responseMock = new ValidationResult<PaymentModel>(new List<ValidationError>());
            _paymentServiceMock.Setup(x => x.RemoveAsync(It.IsNotNull<Guid>())).ReturnsAsync(responseMock);

            // Act
            var paymentController = new PaymentController(_loggerMock.Object, _paymentServiceMock.Object);
            var actualResult = await paymentController.RemoveAsync(It.IsNotNull<Guid>());
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<PaymentModel>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(paymentController.RemoveAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(paymentController.RemoveAsync)} method", Times.Never, _exception);
            actualResult.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().BeNull();
        }

        [Fact]
        public async Task GivenValidRequestAndNonExistingSettingsGuid_WhenRemoveAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            var responseMock = new ValidationResult<PaymentModel>(new List<ValidationError>()
            {
                ValidationError.NotFound
            });
            _paymentServiceMock.Setup(x => x.RemoveAsync(It.IsNotNull<Guid>())).ReturnsAsync(responseMock);

            // Act
            var paymentController = new PaymentController(_loggerMock.Object, _paymentServiceMock.Object);
            var actualResult = await paymentController.RemoveAsync(It.IsNotNull<Guid>());
            var data = actualResult as NotFoundObjectResult;
            var actualResultRespone = data!.Value as ValidationResult<PaymentModel>;
            // Assert
            _paymentServiceMock.Verify(x => x.RemoveAsync(It.IsNotNull<Guid>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(paymentController.RemoveAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(paymentController.RemoveAsync)} method", Times.Never, _exception);
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
            _paymentServiceMock.Setup(x => x.RemoveAsync(It.IsNotNull<Guid>())).ThrowsAsync(someEx);

            // Act
            var paymentController = new PaymentController(_loggerMock.Object, _paymentServiceMock.Object);
            var actualResult = await paymentController.RemoveAsync(It.IsNotNull<Guid>());
            var data = ((ObjectResult)actualResult).Value as ValidationResult<PaymentAddRequestModel>;
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(paymentController.RemoveAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(paymentController.RemoveAsync)} method", Times.Once, _exception);
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
            _paymentServiceMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ReturnsAsync(responseMock);

            // Act
            var paymentController = new PaymentController(_loggerMock.Object, _paymentServiceMock.Object);
            var actualResult = await paymentController.GetByIdAsync(It.IsNotNull<Guid>());
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<PaymentModel>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(paymentController.GetByIdAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(paymentController.GetByIdAsync)} method", Times.Never, _exception);
            actualResult.Should().BeOfType<OkObjectResult>();
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().NotBeNull();
            data?.Data.Amount.Should().Be(addRequestMock.Amount);
            data?.Data.LaneInId.Should().Be(addRequestMock.LaneInId);
            data?.Data.LaneOutId.Should().Be(addRequestMock.LaneOutId);
            data?.Data.Make.Should().Be(addRequestMock.Make);
        }

        // Unhappy case 400
        [Fact]
        public async Task GivenValidRequestAndSettingsServiceIsDown_WhenGetByIdAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            var someEx = new Exception("An error occurred when calling GetByIdAsync method");
            _paymentServiceMock.Setup(x => x.GetByIdAsync(It.IsNotNull<Guid>())).ThrowsAsync(someEx);

            // Act
            var paymentController = new PaymentController(_loggerMock.Object, _paymentServiceMock.Object);
            var actualResult = await paymentController.GetByIdAsync(It.IsNotNull<Guid>());
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(paymentController.GetByIdAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(paymentController.GetByIdAsync)} method", Times.Once, _exception);
            ((ObjectResult)actualResult).StatusCode.Should().Be(500);
            actualResultRespone?.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone?.Errors.Count() > 0);
        }
        #endregion
    }
}
