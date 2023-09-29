using EPAY.ETC.Core.API.Controllers.ETCCheckouts;
using EPAY.ETC.Core.API.Core.Interfaces.Services.ETCCheckouts;
using EPAY.ETC.Core.API.Core.Models.ETCCheckOuts;
using EPAY.ETC.Core.API.UnitTests.Helpers;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.UnitTests.Controllers.ETCCheckouts
{
    public class ETCCheckoutControllerTests : ControllerBase
    {

        #region Init mock instance
        private readonly Mock<ILogger<ETCCheckoutController>> _loggerMock = new();
        private readonly Mock<IETCCheckoutService> _etcCheckoutServiceMock = new();
        #endregion

        #region Init test data
        private static Guid etcCheckoutId = Guid.Parse("a71717fb-cffd-4994-89d9-775a62d89399");
        private ETCCheckoutAddUpdateRequestModel request = new ETCCheckoutAddUpdateRequestModel()
        {
            TransactionId = "Some Transaction",
            PaymentId = Guid.Parse("d87a071c-596b-4bc2-9205-240b9f6b03ca"),
            PlateNumber = "Some Plate",
            Amount = 7000,
            RFID = "Some RFID",
            ServiceProvider = ETCServiceProviderEnum.VETC,
            TransactionStatus = TransactionStatusEnum.CheckOut
        };
        private static ETCCheckoutResponseModel etcCheckoutModel = new ETCCheckoutResponseModel()
        {
            CreatedDate = new DateTime(2023, 9, 15, 3, 6, 8),
            Id = etcCheckoutId,
            TransactionId = "Some Transaction",
            PaymentId = Guid.Parse("d87a071c-596b-4bc2-9205-240b9f6b03ca"),
            PlateNumber = "Some Plate",
            Amount = 7000,
            RFID = "Some RFID",
            ServiceProvider = ETCServiceProviderEnum.VETC,
            TransactionStatus = TransactionStatusEnum.CheckOut
        };
        private ValidationResult<ETCCheckoutResponseModel> response = ValidationResult.Success(etcCheckoutModel);
        private Exception _exception = null!;
        #endregion

        #region AddAsync
        // 200, 201
        [Fact]
        public void GiveRequestIsValid_WhenApiAddAsyncIsCalled_ThenReturnGoodValidation()
        {
            //arrange

            //act
            var actualResult = ValidateModelTest.ValidateModel(request);

            //assert 
            actualResult.Should().NotBeNull();
            Assert.True(actualResult.Count() == 0);
        }

        [Fact]
        public async Task GivenRequestIsValid_WhenApiAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _etcCheckoutServiceMock.Setup(x => x.AddAsync(It.IsAny<ETCCheckoutAddUpdateRequestModel>())).ReturnsAsync(response);

            // Act
            var controller = new ETCCheckoutController(_loggerMock.Object, _etcCheckoutServiceMock.Object);
            var actualResult = await controller.AddAsync(request);
            var data = ((CreatedResult)actualResult).Value as ValidationResult<ETCCheckoutResponseModel>;

            // Assert
            _etcCheckoutServiceMock.Verify(x => x.AddAsync(It.IsAny<ETCCheckoutAddUpdateRequestModel>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.AddAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.AddAsync)} method", Times.Never, _exception);

            actualResult.Should().BeOfType<CreatedResult>();
            ((CreatedResult)actualResult).StatusCode.Should().Be(StatusCodes.Status201Created);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().NotBeNull();
            data?.Data?.Id.Should().Be(etcCheckoutId);
            data?.Data?.Amount.Should().Be(request.Amount);
            data?.Data?.PaymentId.Should().Be(request.PaymentId);
            data?.Data?.PlateNumber.Should().Be(request.PlateNumber);
            data?.Data?.Amount.Should().Be(request.Amount);
            data?.Data?.RFID.Should().Be(request.RFID);
            data?.Data?.ServiceProvider.Should().Be(request.ServiceProvider);
            data?.Data?.TransactionStatus.Should().Be(request.TransactionStatus);
            data?.Data?.TransactionId.Should().Be(request.TransactionId);
        }

        [Fact]
        public async Task GivenRequestIsValidAndETCCheckoutAlreadyExists_WhenApiAddAsyncIsCalled_ThenReturnConflict()
        {
            // Arrange
            var response = ValidationResult.Failed<ETCCheckoutResponseModel>(new List<ValidationError>() { ValidationError.Conflict });
            _etcCheckoutServiceMock.Setup(x => x.AddAsync(It.IsAny<ETCCheckoutAddUpdateRequestModel>())).ReturnsAsync(response);

            // Act
            var controller = new ETCCheckoutController(_loggerMock.Object, _etcCheckoutServiceMock.Object);
            var actualResult = await controller.AddAsync(request);
            var data = ((ConflictObjectResult)actualResult).Value as ValidationResult<ETCCheckoutResponseModel>;

            // Assert
            _etcCheckoutServiceMock.Verify(x => x.AddAsync(It.IsAny<ETCCheckoutAddUpdateRequestModel>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.AddAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.AddAsync)} method", Times.Never, _exception);

            actualResult.Should().BeOfType<ConflictObjectResult>();
            ((ConflictObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status409Conflict);
            data?.Succeeded.Should().BeFalse();
            data?.Data.Should().BeNull();
        }

        [Fact]
        public async Task GivenRequestIsValidAndPaymentIdIsNotExists_WhenApiAddAsyncIsCalled_ThenReturnBadRequest()
        {
            // Arrange
            var response = ValidationResult.Failed<ETCCheckoutResponseModel>(new List<ValidationError>() { ValidationError.BadRequest });
            _etcCheckoutServiceMock.Setup(x => x.AddAsync(It.IsAny<ETCCheckoutAddUpdateRequestModel>())).ReturnsAsync(response);

            // Act
            var controller = new ETCCheckoutController(_loggerMock.Object, _etcCheckoutServiceMock.Object);
            var actualResult = await controller.AddAsync(request);
            var data = ((BadRequestObjectResult)actualResult).Value as ValidationResult<ETCCheckoutResponseModel>;

            // Assert
            _etcCheckoutServiceMock.Verify(x => x.AddAsync(It.IsAny<ETCCheckoutAddUpdateRequestModel>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.AddAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.AddAsync)} method", Times.Never, _exception);

            actualResult.Should().BeOfType<ConflictObjectResult>();
            ((BadRequestObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            data?.Succeeded.Should().BeFalse();
            data?.Data.Should().BeNull();
        }

        // 400, 500
        [Fact]
        public void GiveRequestIsInValid_WhenApiAddAsyncIsCalled_ThenReturnBadValidation()
        {
            //arrange
            var request = new ETCCheckoutAddUpdateRequestModel();

            //act
            var actualResult = ValidateModelTest.ValidateModel(request);

            //assert 
            actualResult.Should().NotBeNull();
            Assert.True(actualResult.Count() > 0);
        }

        [Fact]
        public async Task GivenRequestIsValidAndETCCheckoutServiceIsDown_WhenApiAddAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            _etcCheckoutServiceMock.Setup(x => x.AddAsync(It.IsAny<ETCCheckoutAddUpdateRequestModel>())).ThrowsAsync(new Exception("Some ex"));

            // Act
            var controller = new ETCCheckoutController(_loggerMock.Object, _etcCheckoutServiceMock.Object);
            var actualResult = await controller.AddAsync(request);
            var data = ((ObjectResult)actualResult).Value as ValidationResult<ETCCheckoutResponseModel>;

            // Assert
            _etcCheckoutServiceMock.Verify(x => x.AddAsync(It.IsAny<ETCCheckoutAddUpdateRequestModel>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.AddAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.AddAsync)} method", Times.Once, _exception);

            actualResult.Should().BeOfType<ObjectResult>();
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            data?.Succeeded.Should().BeFalse();
            data?.Data.Should().BeNull();
        }
        #endregion

        #region UpdateAsync
        // 200
        [Fact]
        public void GiveRequestIsValid_WhenApiUpdateAsyncIsCalled_ThenReturnGoodValidation()
        {
            //arrange

            //act
            var actualResult = ValidateModelTest.ValidateModel(request);

            //assert 
            actualResult.Should().NotBeNull();
            Assert.True(actualResult.Count() == 0);
        }

        [Fact]
        public async Task GivenRequestIsValid_WhenApiUpdateAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _etcCheckoutServiceMock.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ETCCheckoutAddUpdateRequestModel>())).ReturnsAsync(response);

            // Act
            var controller = new ETCCheckoutController(_loggerMock.Object, _etcCheckoutServiceMock.Object);
            var actualResult = await controller.UpdateAsync(etcCheckoutId, request);
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<ETCCheckoutResponseModel>;

            // Assert
            _etcCheckoutServiceMock.Verify(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ETCCheckoutAddUpdateRequestModel>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.UpdateAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.UpdateAsync)} method", Times.Never, _exception);

            actualResult.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().NotBeNull();
            data?.Data?.Id.Should().Be(etcCheckoutId);
            data?.Data?.Amount.Should().Be(request.Amount);
            data?.Data?.PaymentId.Should().Be(request.PaymentId);
            data?.Data?.PlateNumber.Should().Be(request.PlateNumber);
            data?.Data?.Amount.Should().Be(request.Amount);
            data?.Data?.RFID.Should().Be(request.RFID);
            data?.Data?.ServiceProvider.Should().Be(request.ServiceProvider);
            data?.Data?.TransactionStatus.Should().Be(request.TransactionStatus);
            data?.Data?.TransactionId.Should().Be(request.TransactionId);
        }

        [Fact]
        public async Task GivenRequestIsValidAndPaymentIdIsNotExists_WhenApiUpdateAsyncIsCalled_ThenReturnBadRequest()
        {
            // Arrange
            var response = ValidationResult.Failed<ETCCheckoutResponseModel>(new List<ValidationError>() { ValidationError.BadRequest });
            _etcCheckoutServiceMock.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ETCCheckoutAddUpdateRequestModel>())).ReturnsAsync(response);

            // Act
            var controller = new ETCCheckoutController(_loggerMock.Object, _etcCheckoutServiceMock.Object);
            var actualResult = await controller.UpdateAsync(etcCheckoutId, request);
            var data = ((BadRequestObjectResult)actualResult).Value as ValidationResult<ETCCheckoutResponseModel>;

            // Assert
            _etcCheckoutServiceMock.Verify(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ETCCheckoutAddUpdateRequestModel>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.UpdateAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.UpdateAsync)} method", Times.Never, _exception);

            actualResult.Should().BeOfType<ConflictObjectResult>();
            ((BadRequestObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            data?.Succeeded.Should().BeFalse();
            data?.Data.Should().BeNull();
        }

        [Fact]
        public async Task GivenRequestIsValidAndETCCheckoutAlreadyExists_WhenApiUpdateAsyncIsCalled_ThenReturnConflict()
        {
            // Arrange
            var response = ValidationResult.Failed<ETCCheckoutResponseModel>(new List<ValidationError>() { ValidationError.Conflict });
            _etcCheckoutServiceMock.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ETCCheckoutAddUpdateRequestModel>())).ReturnsAsync(response);

            // Act
            var controller = new ETCCheckoutController(_loggerMock.Object, _etcCheckoutServiceMock.Object);
            var actualResult = await controller.UpdateAsync(etcCheckoutId, request);
            var data = ((ConflictObjectResult)actualResult).Value as ValidationResult<ETCCheckoutResponseModel>;

            // Assert
            _etcCheckoutServiceMock.Verify(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ETCCheckoutAddUpdateRequestModel>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.UpdateAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.UpdateAsync)} method", Times.Never, _exception);

            actualResult.Should().BeOfType<ConflictObjectResult>();
            ((ConflictObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status409Conflict);
            data?.Succeeded.Should().BeFalse();
            data?.Data.Should().BeNull();
        }

        [Fact]
        public async Task GivenRequestIsValidAndETCCheckoutIsNotExists_WhenApiUpdateAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            var response = ValidationResult.Failed<ETCCheckoutResponseModel>(new List<ValidationError>() { ValidationError.NotFound });
            _etcCheckoutServiceMock.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ETCCheckoutAddUpdateRequestModel>())).ReturnsAsync(response);

            // Act
            var controller = new ETCCheckoutController(_loggerMock.Object, _etcCheckoutServiceMock.Object);
            var actualResult = await controller.UpdateAsync(etcCheckoutId, request);
            var data = ((NotFoundObjectResult)actualResult).Value as ValidationResult<ETCCheckoutResponseModel>;

            // Assert
            _etcCheckoutServiceMock.Verify(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ETCCheckoutAddUpdateRequestModel>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.UpdateAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.UpdateAsync)} method", Times.Never, _exception);

            actualResult.Should().BeOfType<NotFoundObjectResult>();
            ((NotFoundObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status404NotFound);
            data?.Succeeded.Should().BeFalse();
            data?.Data.Should().BeNull();
        }

        // 500
        [Fact]
        public void GiveRequestIsInValid_WhenApiUpdateAsyncIsCalled_ThenReturnBadValidation()
        {
            //arrange
            var request = new ETCCheckoutAddUpdateRequestModel();

            //act
            var actualResult = ValidateModelTest.ValidateModel(request);

            //assert 
            actualResult.Should().NotBeNull();
            Assert.True(actualResult.Count() > 0);
        }

        [Fact]
        public async Task GivenRequestIsValidAndETCCheckoutServiceIsDown_WhenApiUpdateAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            _etcCheckoutServiceMock.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ETCCheckoutAddUpdateRequestModel>())).ThrowsAsync(new Exception("Some ex"));

            // Act
            var controller = new ETCCheckoutController(_loggerMock.Object, _etcCheckoutServiceMock.Object);
            var actualResult = await controller.UpdateAsync(etcCheckoutId, request);
            var data = ((ObjectResult)actualResult).Value as ValidationResult<ETCCheckoutResponseModel>;

            // Assert
            _etcCheckoutServiceMock.Verify(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ETCCheckoutAddUpdateRequestModel>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.UpdateAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.UpdateAsync)} method", Times.Once, _exception);

            actualResult.Should().BeOfType<ObjectResult>();
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            data?.Succeeded.Should().BeFalse();
            data?.Data.Should().BeNull();
        }
        #endregion

        #region RemoveAsync
        // 200
        [Fact]
        public async Task GivenRequestIsValid_WhenApiRemoveAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var response = ValidationResult.Success<ETCCheckoutResponseModel?>(null);
            _etcCheckoutServiceMock.Setup(x => x.RemoveAsync(It.IsAny<Guid>())).ReturnsAsync(response);

            // Act
            var controller = new ETCCheckoutController(_loggerMock.Object, _etcCheckoutServiceMock.Object);
            var actualResult = await controller.RemoveAsync(etcCheckoutId);
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<ETCCheckoutResponseModel>;

            // Assert
            _etcCheckoutServiceMock.Verify(x => x.RemoveAsync(It.IsAny<Guid>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.RemoveAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.RemoveAsync)} method", Times.Never, _exception);

            actualResult.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().BeNull();
        }

        [Fact]
        public async Task GivenRequestIsValidAndETCCheckoutIsNotExists_WhenApiRemoveAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            var response = ValidationResult.Failed<ETCCheckoutResponseModel?>(new List<ValidationError>() { ValidationError.NotFound });
            _etcCheckoutServiceMock.Setup(x => x.RemoveAsync(It.IsAny<Guid>())).ReturnsAsync(response);

            // Act
            var controller = new ETCCheckoutController(_loggerMock.Object, _etcCheckoutServiceMock.Object);
            var actualResult = await controller.RemoveAsync(Guid.NewGuid());
            var data = ((NotFoundObjectResult)actualResult).Value as ValidationResult<ETCCheckoutResponseModel>;

            // Assert
            _etcCheckoutServiceMock.Verify(x => x.RemoveAsync(It.IsAny<Guid>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.RemoveAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.RemoveAsync)} method", Times.Never, _exception);

            actualResult.Should().BeOfType<NotFoundObjectResult>();
            ((NotFoundObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status404NotFound);
            data?.Succeeded.Should().BeFalse();
            data?.Data.Should().BeNull();
        }

        // 500
        [Fact]
        public async Task GivenRequestIsValidAndETCCheckoutServiceIsDown_WhenApiRemoveAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            _etcCheckoutServiceMock.Setup(x => x.RemoveAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception("Some ex"));

            // Act
            var controller = new ETCCheckoutController(_loggerMock.Object, _etcCheckoutServiceMock.Object);
            var actualResult = await controller.RemoveAsync(etcCheckoutId);
            var data = ((ObjectResult)actualResult).Value as ValidationResult<ETCCheckoutResponseModel>;

            // Assert
            _etcCheckoutServiceMock.Verify(x => x.RemoveAsync(It.IsAny<Guid>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.RemoveAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.RemoveAsync)} method", Times.Once, _exception);

            actualResult.Should().BeOfType<ObjectResult>();
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            data?.Succeeded.Should().BeFalse();
            data?.Data.Should().BeNull();
        }
        #endregion

        #region GetByIdAsync
        // 200
        [Fact]
        public async Task GivenRequestIsValid_WhenApiGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var response = ValidationResult.Success<ETCCheckoutResponseModel?>(etcCheckoutModel);
            _etcCheckoutServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(response);

            // Act
            var controller = new ETCCheckoutController(_loggerMock.Object, _etcCheckoutServiceMock.Object);
            var actualResult = await controller.GetByIdAsync(etcCheckoutId);
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<ETCCheckoutResponseModel>;

            // Assert
            _etcCheckoutServiceMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.GetByIdAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.GetByIdAsync)} method", Times.Never, _exception);

            actualResult.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().NotBeNull();
            data?.Data?.Id.Should().Be(etcCheckoutId);
            data?.Data?.Amount.Should().Be(request.Amount);
            data?.Data?.PaymentId.Should().Be(request.PaymentId);
            data?.Data?.PlateNumber.Should().Be(request.PlateNumber);
            data?.Data?.Amount.Should().Be(request.Amount);
            data?.Data?.RFID.Should().Be(request.RFID);
            data?.Data?.ServiceProvider.Should().Be(request.ServiceProvider);
            data?.Data?.TransactionStatus.Should().Be(request.TransactionStatus);
            data?.Data?.TransactionId.Should().Be(request.TransactionId);
        }

        // 500
        [Fact]
        public async Task GivenRequestIsValidAndETCCheckoutServiceIsDown_WhenApiGetByIdAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            _etcCheckoutServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception("Some ex"));

            // Act
            var controller = new ETCCheckoutController(_loggerMock.Object, _etcCheckoutServiceMock.Object);
            var actualResult = await controller.GetByIdAsync(etcCheckoutId);
            var data = ((ObjectResult)actualResult).Value as ValidationResult<ETCCheckoutResponseModel>;

            // Assert
            _etcCheckoutServiceMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.GetByIdAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.GetByIdAsync)} method", Times.Once, _exception);

            actualResult.Should().BeOfType<ObjectResult>();
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            data?.Succeeded.Should().BeFalse();
            data?.Data.Should().BeNull();
        }
        #endregion

        #region GetAllAsync
        // 200
        [Fact]
        public async Task GivenRequestIsValid_WhenApiGetAllAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var response = ValidationResult.Success<IEnumerable<ETCCheckoutResponseModel>>(new List<ETCCheckoutResponseModel>() { etcCheckoutModel });
            _etcCheckoutServiceMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<ETCCheckoutDataModel, bool>>>())).ReturnsAsync(response);

            // Act
            var controller = new ETCCheckoutController(_loggerMock.Object, _etcCheckoutServiceMock.Object);
            var actualResult = await controller.GetAllAsync();
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<IEnumerable<ETCCheckoutResponseModel>>;

            // Assert
            _etcCheckoutServiceMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<ETCCheckoutDataModel, bool>>>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.GetAllAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.GetAllAsync)} method", Times.Never, _exception);

            actualResult.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().NotBeNull().And.HaveCount(1);
        }

        // 500
        [Fact]
        public async Task GivenRequestIsValidAndETCCheckoutServiceIsDown_WhenApiGetAllAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            _etcCheckoutServiceMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<ETCCheckoutDataModel, bool>>>())).ThrowsAsync(new Exception("Some ex"));

            // Act
            var controller = new ETCCheckoutController(_loggerMock.Object, _etcCheckoutServiceMock.Object);
            var actualResult = await controller.GetAllAsync();
            var data = ((ObjectResult)actualResult).Value as ValidationResult<ETCCheckoutResponseModel>;

            // Assert
            _etcCheckoutServiceMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<ETCCheckoutDataModel, bool>>>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.GetAllAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.GetAllAsync)} method", Times.Once, _exception);

            actualResult.Should().BeOfType<ObjectResult>();
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            data?.Succeeded.Should().BeFalse();
            data?.Data.Should().BeNull();
        }
        #endregion
    }
}
