using EPAY.ETC.Core.API.Controllers.Fees;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Fees;
using EPAY.ETC.Core.API.Core.Interfaces.Services.UIActions;
using EPAY.ETC.Core.API.Core.Models.Fees;
using EPAY.ETC.Core.API.Models.Configs;
using EPAY.ETC.Core.API.Services;
using EPAY.ETC.Core.API.UnitTests.Helpers;
using EPAY.ETC.Core.Models.Validation;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Linq.Expressions;
using CoreModel = EPAY.ETC.Core.Models.Fees;

namespace EPAY.ETC.Core.API.UnitTests.Controllers.Fees
{
    public class FeeControllerTests : ControllerBase
    {

        #region Init mock instance
        private readonly Mock<ILogger<FeeController>> _loggerMock = new();
        private readonly Mock<IFeeService> _feeServiceMock = new();
        private readonly Mock<IUIActionService> _iuActionServiceMock = new();
        private readonly Mock<IRabbitMQPublisherService> _rabbitMQPublisherServiceMock = new();
        private IOptions<List<PublisherConfigurationOption>> _publisherOptions = Options.Create(new List<PublisherConfigurationOption>()
        {
            new PublisherConfigurationOption(){ PublisherTarget = ETC.Core.Models.Enums.PublisherTargetEnum.Fee}
        });
        #endregion

        #region Init test data
        private static Guid feeId = Guid.Parse("a71717fb-cffd-4994-89d9-775a62d89399");
        private CoreModel.FeeModel request = new CoreModel.FeeModel()
        {
            CreatedDate = new DateTime(2023, 9, 15, 3, 6, 8),
            EmployeeId = "Some employee",
            FeeId = feeId,
            ObjectId = Guid.Parse("a71717fb-cffd-4994-89d9-775a62d89399"),
            ShiftId = "Some Id",
            Payment = new CoreModel.PaymentModel()
            {
                Duration = 32541,
                Model = "Some model",
                PlateNumber = "Some plate number",
                Amount = 9000
            }
        };
        private static FeeModel feeModel = new FeeModel()
        {
            CreatedDate = new DateTime(2023, 9, 15, 3, 6, 8),
            EmployeeId = "Some employee",
            Id = feeId,
            ObjectId = Guid.Parse("a71717fb-cffd-4994-89d9-775a62d89399"),
            ShiftId = "Some",
            Duration = 32541,
            Model = "Some model",
            PlateNumber = "Some plate number",
            Amount = 9000
        };
        private ValidationResult<FeeModel> response = ValidationResult.Success(feeModel);
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
            _feeServiceMock.Setup(x => x.AddAsync(It.IsAny<CoreModel.FeeModel>())).ReturnsAsync(response);

            // Act
            var controller = new FeeController(_loggerMock.Object, _feeServiceMock.Object, _iuActionServiceMock.Object, _rabbitMQPublisherServiceMock.Object, _publisherOptions);
            var actualResult = await controller.AddAsync(request);
            var data = ((CreatedResult)actualResult).Value as ValidationResult<FeeModel>;

            // Assert
            _feeServiceMock.Verify(x => x.AddAsync(It.IsAny<CoreModel.FeeModel>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.AddAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.AddAsync)} method", Times.Never, _exception);

            actualResult.Should().BeOfType<CreatedResult>();
            ((CreatedResult)actualResult).StatusCode.Should().Be(StatusCodes.Status201Created);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().NotBeNull();
            data?.Data?.Id.Should().Be((Guid)request.FeeId!);
            data?.Data?.ObjectId.Should().Be(request.ObjectId);
            data?.Data?.ShiftId.Should().Be(request.ShiftId);
            data?.Data?.EmployeeId.Should().Be(request.EmployeeId);
            data?.Data?.Amount.Should().Be(request.Payment?.Amount);
            data?.Data?.Duration.Should().Be(request.Payment?.Duration);
            data?.Data?.Model.Should().Be(request.Payment?.Model);
            data?.Data?.PlateNumber.Should().Be(request.Payment?.PlateNumber);
        }

        [Fact]
        public async Task GivenRequestIsValidAndFeeIsExists_WhenApiAddAsyncIsCalled_ThenReturnConflict()
        {
            // Arrange
            var response = ValidationResult.Failed<FeeModel>(new List<ValidationError>() { ValidationError.Conflict });
            _feeServiceMock.Setup(x => x.AddAsync(It.IsAny<CoreModel.FeeModel>())).ReturnsAsync(response);

            // Act
            var controller = new FeeController(_loggerMock.Object, _feeServiceMock.Object, _iuActionServiceMock.Object, _rabbitMQPublisherServiceMock.Object, _publisherOptions);
            var actualResult = await controller.AddAsync(request);
            var data = ((ConflictObjectResult)actualResult).Value as ValidationResult<FeeModel>;

            // Assert
            _feeServiceMock.Verify(x => x.AddAsync(It.IsAny<CoreModel.FeeModel>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.AddAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.AddAsync)} method", Times.Never, _exception);

            actualResult.Should().BeOfType<ConflictObjectResult>();
            ((ConflictObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status409Conflict);
            data?.Succeeded.Should().BeFalse();
            data?.Data.Should().BeNull();
        }

        // 400, 500
        [Fact]
        public void GiveRequestIsInValid_WhenApiAddAsyncIsCalled_ThenReturnBadValidation()
        {
            //arrange
            var request = new CoreModel.FeeModel();

            //act
            var actualResult = ValidateModelTest.ValidateModel(request);

            //assert 
            actualResult.Should().NotBeNull();
            Assert.True(actualResult.Count() > 0);
        }

        [Fact]
        public async Task GivenRequestIsValidAndFeeServiceIsDown_WhenApiAddAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            _feeServiceMock.Setup(x => x.AddAsync(It.IsAny<CoreModel.FeeModel>())).ThrowsAsync(new Exception("Some ex"));

            // Act
            var controller = new FeeController(_loggerMock.Object, _feeServiceMock.Object, _iuActionServiceMock.Object, _rabbitMQPublisherServiceMock.Object, _publisherOptions);
            var actualResult = await controller.AddAsync(request);
            var data = ((ObjectResult)actualResult).Value as ValidationResult<FeeModel>;

            // Assert
            _feeServiceMock.Verify(x => x.AddAsync(It.IsAny<CoreModel.FeeModel>()), Times.Once);

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
            _feeServiceMock.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<CoreModel.FeeModel>())).ReturnsAsync(response);

            // Act
            var controller = new FeeController(_loggerMock.Object, _feeServiceMock.Object, _iuActionServiceMock.Object, _rabbitMQPublisherServiceMock.Object, _publisherOptions);
            var actualResult = await controller.UpdateAsync(feeId, request);
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<FeeModel>;

            // Assert
            _feeServiceMock.Verify(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<CoreModel.FeeModel>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.UpdateAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.UpdateAsync)} method", Times.Never, _exception);

            actualResult.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().NotBeNull();
            data?.Data?.Id.Should().Be((Guid)request.FeeId!);
            data?.Data?.ObjectId.Should().Be(request.ObjectId);
            data?.Data?.ShiftId.Should().Be(request.ShiftId);
            data?.Data?.EmployeeId.Should().Be(request.EmployeeId);
            data?.Data?.Amount.Should().Be(request.Payment?.Amount);
            data?.Data?.Duration.Should().Be(request.Payment?.Duration);
            data?.Data?.Model.Should().Be(request.Payment?.Model);
            data?.Data?.PlateNumber.Should().Be(request.Payment?.PlateNumber);
        }

        [Fact]
        public async Task GivenRequestIsValidAndFeeIsExists_WhenApiUpdateAsyncIsCalled_ThenReturnConflict()
        {
            // Arrange
            var response = ValidationResult.Failed<FeeModel>(new List<ValidationError>() { ValidationError.Conflict });
            _feeServiceMock.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<CoreModel.FeeModel>())).ReturnsAsync(response);

            // Act
            var controller = new FeeController(_loggerMock.Object, _feeServiceMock.Object, _iuActionServiceMock.Object, _rabbitMQPublisherServiceMock.Object, _publisherOptions);
            var actualResult = await controller.UpdateAsync(feeId, request);
            var data = ((ConflictObjectResult)actualResult).Value as ValidationResult<FeeModel>;

            // Assert
            _feeServiceMock.Verify(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<CoreModel.FeeModel>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.UpdateAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.UpdateAsync)} method", Times.Never, _exception);

            actualResult.Should().BeOfType<ConflictObjectResult>();
            ((ConflictObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status409Conflict);
            data?.Succeeded.Should().BeFalse();
            data?.Data.Should().BeNull();
        }

        [Fact]
        public async Task GivenRequestIsValidAndFeeIsNotExists_WhenApiUpdateAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            var response = ValidationResult.Failed<FeeModel>(new List<ValidationError>() { ValidationError.NotFound });
            _feeServiceMock.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<CoreModel.FeeModel>())).ReturnsAsync(response);

            // Act
            var controller = new FeeController(_loggerMock.Object, _feeServiceMock.Object, _iuActionServiceMock.Object, _rabbitMQPublisherServiceMock.Object, _publisherOptions);
            var actualResult = await controller.UpdateAsync(feeId, request);
            var data = ((NotFoundObjectResult)actualResult).Value as ValidationResult<FeeModel>;

            // Assert
            _feeServiceMock.Verify(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<CoreModel.FeeModel>()), Times.Once);

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
            var request = new CoreModel.FeeModel();

            //act
            var actualResult = ValidateModelTest.ValidateModel(request);

            //assert 
            actualResult.Should().NotBeNull();
            Assert.True(actualResult.Count() > 0);
        }

        [Fact]
        public async Task GivenRequestIsValidAndFeeServiceIsDown_WhenApiUpdateAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            _feeServiceMock.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<CoreModel.FeeModel>())).ThrowsAsync(new Exception("Some ex"));

            // Act
            var controller = new FeeController(_loggerMock.Object, _feeServiceMock.Object, _iuActionServiceMock.Object, _rabbitMQPublisherServiceMock.Object, _publisherOptions);
            var actualResult = await controller.UpdateAsync(feeId, request);
            var data = ((ObjectResult)actualResult).Value as ValidationResult<FeeModel>;

            // Assert
            _feeServiceMock.Verify(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<CoreModel.FeeModel>()), Times.Once);

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
            var response = ValidationResult.Success<FeeModel?>(null);
            _feeServiceMock.Setup(x => x.RemoveAsync(It.IsAny<Guid>())).ReturnsAsync(response);

            // Act
            var controller = new FeeController(_loggerMock.Object, _feeServiceMock.Object, _iuActionServiceMock.Object, _rabbitMQPublisherServiceMock.Object, _publisherOptions);
            var actualResult = await controller.RemoveAsync(feeId);
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<FeeModel>;

            // Assert
            _feeServiceMock.Verify(x => x.RemoveAsync(It.IsAny<Guid>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.RemoveAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.RemoveAsync)} method", Times.Never, _exception);

            actualResult.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().BeNull();
        }

        [Fact]
        public async Task GivenRequestIsValidAndFeeIsNotExists_WhenApiRemoveAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            var response = ValidationResult.Failed<FeeModel?>(new List<ValidationError>() { ValidationError.NotFound });
            _feeServiceMock.Setup(x => x.RemoveAsync(It.IsAny<Guid>())).ReturnsAsync(response);

            // Act
            var controller = new FeeController(_loggerMock.Object, _feeServiceMock.Object, _iuActionServiceMock.Object, _rabbitMQPublisherServiceMock.Object, _publisherOptions);
            var actualResult = await controller.RemoveAsync(Guid.NewGuid());
            var data = ((NotFoundObjectResult)actualResult).Value as ValidationResult<FeeModel>;

            // Assert
            _feeServiceMock.Verify(x => x.RemoveAsync(It.IsAny<Guid>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.RemoveAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.RemoveAsync)} method", Times.Never, _exception);

            actualResult.Should().BeOfType<NotFoundObjectResult>();
            ((NotFoundObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status404NotFound);
            data?.Succeeded.Should().BeFalse();
            data?.Data.Should().BeNull();
        }

        // 500
        [Fact]
        public async Task GivenRequestIsValidAndFeeServiceIsDown_WhenApiRemoveAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            _feeServiceMock.Setup(x => x.RemoveAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception("Some ex"));

            // Act
            var controller = new FeeController(_loggerMock.Object, _feeServiceMock.Object, _iuActionServiceMock.Object, _rabbitMQPublisherServiceMock.Object, _publisherOptions);
            var actualResult = await controller.RemoveAsync(feeId);
            var data = ((ObjectResult)actualResult).Value as ValidationResult<FeeModel>;

            // Assert
            _feeServiceMock.Verify(x => x.RemoveAsync(It.IsAny<Guid>()), Times.Once);

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
            var response = ValidationResult.Success<FeeModel?>(feeModel);
            _feeServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(response);

            // Act
            var controller = new FeeController(_loggerMock.Object, _feeServiceMock.Object, _iuActionServiceMock.Object, _rabbitMQPublisherServiceMock.Object, _publisherOptions);
            var actualResult = await controller.GetByIdAsync(feeId);
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<FeeModel>;

            // Assert
            _feeServiceMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.GetByIdAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.GetByIdAsync)} method", Times.Never, _exception);

            actualResult.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().NotBeNull();
            data?.Data?.Id.Should().Be((Guid)request.FeeId!);
            data?.Data?.ObjectId.Should().Be(request.ObjectId);
            data?.Data?.ShiftId.Should().Be(request.ShiftId);
            data?.Data?.EmployeeId.Should().Be(request.EmployeeId);
            data?.Data?.Amount.Should().Be(request.Payment?.Amount);
            data?.Data?.Duration.Should().Be(request.Payment?.Duration);
            data?.Data?.Model.Should().Be(request.Payment?.Model);
            data?.Data?.PlateNumber.Should().Be(request.Payment?.PlateNumber);
        }

        // 500
        [Fact]
        public async Task GivenRequestIsValidAndFeeServiceIsDown_WhenApiGetByIdAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            _feeServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception("Some ex"));

            // Act
            var controller = new FeeController(_loggerMock.Object, _feeServiceMock.Object, _iuActionServiceMock.Object, _rabbitMQPublisherServiceMock.Object, _publisherOptions);
            var actualResult = await controller.GetByIdAsync(feeId);
            var data = ((ObjectResult)actualResult).Value as ValidationResult<FeeModel>;

            // Assert
            _feeServiceMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);

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
            var response = ValidationResult.Success<IEnumerable<FeeModel>>(new List<FeeModel>() { feeModel });
            _feeServiceMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeModel, bool>>>())).ReturnsAsync(response);

            // Act
            var controller = new FeeController(_loggerMock.Object, _feeServiceMock.Object, _iuActionServiceMock.Object, _rabbitMQPublisherServiceMock.Object, _publisherOptions);
            var actualResult = await controller.GetAllAsync();
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<IEnumerable<FeeModel>>;

            // Assert
            _feeServiceMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeModel, bool>>>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.GetAllAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.GetAllAsync)} method", Times.Never, _exception);

            actualResult.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().NotBeNull().And.HaveCount(1);
        }

        // 500
        [Fact]
        public async Task GivenRequestIsValidAndFeeServiceIsDown_WhenApiGetAllAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            _feeServiceMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeModel, bool>>>())).ThrowsAsync(new Exception("Some ex"));

            // Act
            var controller = new FeeController(_loggerMock.Object, _feeServiceMock.Object, _iuActionServiceMock.Object, _rabbitMQPublisherServiceMock.Object, _publisherOptions);
            var actualResult = await controller.GetAllAsync();
            var data = ((ObjectResult)actualResult).Value as ValidationResult<FeeModel>;

            // Assert
            _feeServiceMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeModel, bool>>>()), Times.Once);

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
