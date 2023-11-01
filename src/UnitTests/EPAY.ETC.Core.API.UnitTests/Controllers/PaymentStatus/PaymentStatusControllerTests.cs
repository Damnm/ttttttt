using EPAY.ETC.Core.API.Controllers.PaymentStatus;
using EPAY.ETC.Core.API.Core.Interfaces.Services.PaymentStatus;
using EPAY.ETC.Core.API.Core.Interfaces.Services.UIActions;
using EPAY.ETC.Core.API.Models.Configs;
using EPAY.ETC.Core.API.UnitTests.Common;
using EPAY.ETC.Core.API.UnitTests.Helpers;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.Fees;
using EPAY.ETC.Core.Models.Fees.PaymentStatusHistory;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using EPAY.ETC.Core.Publisher.Common.Options;
using EPAY.ETC.Core.Publisher.Interface;
using EPAY.ETC.Core.RabbitMQ.Common.Events;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using PaymentStatusModel = EPAY.ETC.Core.API.Core.Models.PaymentStatus.PaymentStatusModel;
namespace EPAY.ETC.Core.API.UnitTests.Controllers.PaymentStatus
{
    public class PaymentStatusControllerTests : TestBase<PaymentStatusController>
    {
        private readonly Exception _exception = null!;
        private Mock<IPaymentStatusService> _paymentStatusServiceMock = new Mock<IPaymentStatusService>();
        private Mock<IUIActionService> _uiActionServiceMock = new();
        private Mock<IPublisherService> _publisherServiceMock = new();
        IOptions<List<PublisherConfigurationOption>> _publisherOptions = Options.Create(new List<PublisherConfigurationOption> { new PublisherConfigurationOption() });
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

        private static Guid objectId = Guid.NewGuid();
        private static Guid paymentId = Guid.NewGuid();
        private PaymentStatusUIRequestModel updatePaymentStatusRequest = new PaymentStatusUIRequestModel()
        {
            Amount = 10000,
            PaymentId = paymentId,
            PaymentMethod = PaymentMethodEnum.Cash,
            Status = PaymentStatusEnum.Paid,
            ObjectId = objectId
        };
        private PaymenStatusResponseModel paymentStatusResponse = new PaymenStatusResponseModel()
        {
            PaymentStatus = new ETC.Core.Models.Fees.PaymentStatusModel()
            {
                Amount = 10000,
                PaymentId = paymentId,
                PaymentMethod = PaymentMethodEnum.Cash,
                Status = PaymentStatusEnum.Paid,
            },
            ObjectId = objectId
        };

        private ValidationResult<List<PaymentStatusHistoryModel>> paymentSta = new ValidationResult<List<PaymentStatusHistoryModel>>()
        {
            Data = new List<PaymentStatusHistoryModel>()
            {
                new PaymentStatusHistoryModel()
            {
                DateTimeEpoch = 324324343,
                PaymentStatus = PaymentStatusEnum.Failed,
                Reason = "dfd222fdsf",
                PaymentMethod = PaymentMethodEnum.RFID,
            },
            new PaymentStatusHistoryModel()
            {
                DateTimeEpoch = 32443432,
                PaymentStatus = PaymentStatusEnum.Failed,
                Reason = "dfd111fdsf",
                PaymentMethod = PaymentMethodEnum.QRCode,
            },
            new PaymentStatusHistoryModel()
            {
                DateTimeEpoch = 7657676,
                PaymentStatus = PaymentStatusEnum.Failed,
                Reason = "dfd66fdsf",
                PaymentMethod = PaymentMethodEnum.Cash,
            }
            }
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
            var paymentStatusController = new PaymentStatusController(_loggerMock.Object, _paymentStatusServiceMock.Object, _uiActionServiceMock.Object, _publisherServiceMock.Object, _publisherOptions, _mapper);
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
            var paymentStatusController = new PaymentStatusController(_loggerMock.Object, _paymentStatusServiceMock.Object, _uiActionServiceMock.Object, _publisherServiceMock.Object, _publisherOptions, _mapper);
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
            var paymentStatusController = new PaymentStatusController(_loggerMock.Object, _paymentStatusServiceMock.Object, _uiActionServiceMock.Object, _publisherServiceMock.Object, _publisherOptions, _mapper);
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
            var vehicleController = new PaymentStatusController(_loggerMock.Object, _paymentStatusServiceMock.Object, _uiActionServiceMock.Object, _publisherServiceMock.Object, _publisherOptions, _mapper);
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
            var vehicleController = new PaymentStatusController(_loggerMock.Object, _paymentStatusServiceMock.Object, _uiActionServiceMock.Object, _publisherServiceMock.Object, _publisherOptions, _mapper);
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
            var vehicleController = new PaymentStatusController(_loggerMock.Object, _paymentStatusServiceMock.Object, _uiActionServiceMock.Object, _publisherServiceMock.Object, _publisherOptions, _mapper);
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
            var responseMock = new ValidationResult<PaymentStatusModel?>(new List<ValidationError>());
            _paymentStatusServiceMock.Setup(x => x.RemoveAsync(It.IsNotNull<Guid>())).ReturnsAsync(responseMock);

            // Act
            var PaymentStatusController = new PaymentStatusController(_loggerMock.Object, _paymentStatusServiceMock.Object, _uiActionServiceMock.Object, _publisherServiceMock.Object, _publisherOptions, _mapper);
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
            var responseMock = new ValidationResult<PaymentStatusModel?>(new List<ValidationError>()
            {
                ValidationError.NotFound
            });
            _paymentStatusServiceMock.Setup(x => x.RemoveAsync(It.IsNotNull<Guid>())).ReturnsAsync(responseMock);

            // Act
            var PaymentStatusController = new PaymentStatusController(_loggerMock.Object, _paymentStatusServiceMock.Object, _uiActionServiceMock.Object, _publisherServiceMock.Object, _publisherOptions, _mapper);
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
            var PaymentStatusController = new PaymentStatusController(_loggerMock.Object, _paymentStatusServiceMock.Object, _uiActionServiceMock.Object, _publisherServiceMock.Object, _publisherOptions, _mapper);
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
            var PaymentStatusController = new PaymentStatusController(_loggerMock.Object, _paymentStatusServiceMock.Object, _uiActionServiceMock.Object, _publisherServiceMock.Object, _publisherOptions, _mapper);
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
            var PaymentStatusController = new PaymentStatusController(_loggerMock.Object, _paymentStatusServiceMock.Object, _uiActionServiceMock.Object, _publisherServiceMock.Object, _publisherOptions, _mapper);
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

        #region UpdatePaymentMethod
        // Happy case 200/201
        [Fact]
        public void GiveRequestIsValid_WhenApiUpdatePaymentMethodIsCalled_ThenReturnGoodValidation()
        {
            //arrange

            //act
            var actualResult = ValidateModelTest.ValidateModel(updatePaymentStatusRequest);

            //assert 
            actualResult.Should().NotBeNull();
            Assert.True(actualResult.Count() == 0);
        }
        [Fact]
        public async Task GivenRequestIsValidAndCustomVehicleTypeIsNull_WhenUpdatePaymentMethodIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _uiActionServiceMock.Setup(x => x.UpdatePaymentMethod(It.IsAny<PaymentStatusUIRequestModel>())).ReturnsAsync(ValidationResult.Success(paymentStatusResponse));
            _publisherServiceMock.Setup(x => x.SendMessage(It.IsAny<RabbitMessageOutbound>(), It.IsAny<PublisherOptions>()));

            // Act
            var controller = new PaymentStatusController(_loggerMock.Object, _paymentStatusServiceMock.Object, _uiActionServiceMock.Object, _publisherServiceMock.Object, _publisherOptions, _mapper);
            var actualResult = await controller.UpdatePaymentMethod(updatePaymentStatusRequest);

            // Assert
            _uiActionServiceMock.Verify(x => x.UpdatePaymentMethod(It.IsAny<PaymentStatusUIRequestModel>()), Times.Once);
            _publisherServiceMock.Verify(x => x.SendMessage(It.IsAny<RabbitMessageOutbound>(), It.IsAny<PublisherOptions>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.UpdatePaymentMethod)}...", Times.Once, _nullException);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.UpdatePaymentMethod)} method", Times.Never, _nullException);

            actualResult.Should().BeOfType<OkResult>();
            ((OkResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        // Unhappy case 400
        [Fact]
        public void GiveRequestIsInValid_WhenApiUpdatePaymentMethodIsCalled_ThenReturnBadValidation()
        {
            //arrange
            var updatePaymentStatusRequest = new PaymentStatusUIRequestModel();

            //act
            var actualResult = ValidateModelTest.ValidateModel(updatePaymentStatusRequest);

            //assert 
            actualResult.Should().NotBeNull();
            Assert.True(actualResult.Count() > 0);
        }
        [Fact]
        public async Task GivenRequestIsValidAndFeesCalculationServiceIsDown_WhenUpdatePaymentMethodIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            var someEx = new Exception("An error occurred when calling UpdatePaymentMethod method");
            _uiActionServiceMock.Setup(x => x.UpdatePaymentMethod(It.IsAny<PaymentStatusUIRequestModel>())).ThrowsAsync(someEx);

            // Act
            var controller = new PaymentStatusController(_loggerMock.Object, _paymentStatusServiceMock.Object, _uiActionServiceMock.Object, _publisherServiceMock.Object, _publisherOptions, _mapper);
            var actualResult = await controller.UpdatePaymentMethod(updatePaymentStatusRequest);
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.UpdatePaymentMethod)}...", Times.Once, _nullException);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.UpdatePaymentMethod)} method", Times.Once, _nullException);
            ((ObjectResult)actualResult).StatusCode.Should().Be(500);
            actualResultRespone?.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone?.Errors.Count > 0);
        }
        [Fact]
        public async Task GivenRequestIsValidAndPublisherServiceIsDown_WhenUpdatePaymentMethodIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            var someEx = new Exception("An error occurred when calling UpdatePaymentMethod method");
            _uiActionServiceMock.Setup(x => x.UpdatePaymentMethod(It.IsAny<PaymentStatusUIRequestModel>())).ReturnsAsync(ValidationResult.Success(paymentStatusResponse));
            _publisherServiceMock.Setup(x => x.SendMessage(It.IsAny<RabbitMessageOutbound>(), It.IsAny<PublisherOptions>())).Throws(someEx);

            // Act
            var controller = new PaymentStatusController(_loggerMock.Object, _paymentStatusServiceMock.Object, _uiActionServiceMock.Object, _publisherServiceMock.Object, _publisherOptions, _mapper);
            var actualResult = await controller.UpdatePaymentMethod(updatePaymentStatusRequest);
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.UpdatePaymentMethod)}...", Times.Once, _nullException);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.UpdatePaymentMethod)} method", Times.Once, _nullException);
            ((ObjectResult)actualResult).StatusCode.Should().Be(500);
            actualResultRespone?.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone?.Errors.Count > 0);
        }
        #endregion

        #region GetPaymentStatusHistoryAsync
        // Happy case 200
        [Fact]
        public async Task GivenValidRequest_WhenGetPaymentStatusHistoryAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _paymentStatusServiceMock.Setup(x => x.GetPaymentStatusHistoryAsync(It.IsNotNull<Guid>())).ReturnsAsync(paymentSta);

            // Act
            var PaymentStatusController = new PaymentStatusController(_loggerMock.Object, _paymentStatusServiceMock.Object, _uiActionServiceMock.Object, _publisherServiceMock.Object, _publisherOptions, _mapper);
            var actualResult = await PaymentStatusController.GetPaymentStatusHistoryAsync(It.IsNotNull<Guid>());
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<List<PaymentStatusHistoryModel>>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PaymentStatusController.GetPaymentStatusHistoryAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(PaymentStatusController.GetPaymentStatusHistoryAsync)} method", Times.Never, _exception);
            actualResult.Should().BeOfType<OkObjectResult>();
            ((ObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().NotBeNull();
            data?.Data?.Count.Should().Be(3);

        }

        // Unhappy case 400
        [Fact]
        public async Task GivenValidRequestAndSettingsServiceIsDown_WhenGetPaymentStatusHistoryAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            var someEx = new Exception("An error occurred when calling GetByIdAsync method");
            _paymentStatusServiceMock.Setup(x => x.GetPaymentStatusHistoryAsync(It.IsNotNull<Guid>())).ThrowsAsync(someEx);

            // Act
            var PaymentStatusController = new PaymentStatusController(_loggerMock.Object, _paymentStatusServiceMock.Object, _uiActionServiceMock.Object, _publisherServiceMock.Object, _publisherOptions, _mapper);
            var actualResult = await PaymentStatusController.GetPaymentStatusHistoryAsync(It.IsNotNull<Guid>());
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(PaymentStatusController.GetPaymentStatusHistoryAsync)}...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(PaymentStatusController.GetPaymentStatusHistoryAsync)} method", Times.Once, _exception);
            ((ObjectResult)actualResult).StatusCode.Should().Be(500);
            actualResultRespone?.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone?.Errors.Count() > 0);
        }
        #endregion
    }
}
