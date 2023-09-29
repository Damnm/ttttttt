using EPAY.ETC.Core.API.Controllers.UIActions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.UIActions;
using EPAY.ETC.Core.API.Models.Configs;
using EPAY.ETC.Core.API.UnitTests.Common;
using EPAY.ETC.Core.API.UnitTests.Helpers;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.Fees;
using EPAY.ETC.Core.Models.Receipt;
using EPAY.ETC.Core.Models.Receipt.SessionReports;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using EPAY.ETC.Core.Publisher.Common.Options;
using EPAY.ETC.Core.Publisher.Interface;
using EPAY.ETC.Core.RabbitMQ.Common.Events;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace EPAY.ETC.Core.API.UnitTests.Controllers.UIActions
{
    public class UIActionControllerTests : TestBase<UIActionController>
    {
        #region Init mock instance
        private Mock<IUIActionService> _uiActionServiceMock = new();
        private Mock<IPublisherService> _publisherServiceMock = new();
        IOptions<List<PublisherConfigurationOption>> _publisherOptions = Options.Create(new List<PublisherConfigurationOption> { new PublisherConfigurationOption() });
        #endregion

        #region Init mock data
        private static Guid objectId = Guid.NewGuid();
        private static Guid paymentId = Guid.NewGuid();
        private SessionReportRequestModel sessionReportRequest = new SessionReportRequestModel()
        {
            FromDate = new DateTime(2023, 9, 29, 15, 32, 19),
            ToDate = new DateTime(2023, 9, 29, 15, 43, 53),
            EmployeeId = "Some employee",
            LaneId = "Some lane"
        };
        private PaymentStatusUIRequestModel updatePaymentStatusRequest = new PaymentStatusUIRequestModel()
        {
            Amount = 10000,
            PaymentId = paymentId,
            PaymentMethod = PaymentMethodEnum.Cash,
            Status = PaymentStatusEnum.Paid,
            ObjectId = objectId
        };
        private SessionReportModel sessionReportResponse = new SessionReportModel()
        {
            PrintType = ReceiptTypeEnum.SessionReport,
            Layout = new SessionLayoutModel()
            {
                Header = new HeaderModel()
                {
                    Heading = "Some heading",
                    Line1 = "Some line",
                    Line2 = "Some line",
                    SubHeading = "Some sub heading"
                },
                Footer = new FooterModel()
                {
                    Line1 = "Some line",
                    Line2 = "Some line",
                    Line3 = "Some line"
                },
                Body = new SessionBodyModel()
                {
                    Columns = new List<string>() { "Some column" },
                    Heading = "Some Heading",
                    SubHeading1 = "Some sub heading 1",
                    SubHeading2 = "Some sub heading 2",
                    SubHeading3 = "Some sub heading 3",
                    Data = new SessionDataModel()
                    {
                        BottomLine = "Some line",
                        GrandTotal = 153000,
                        Qty = 15,
                        Payments = new List<SessionPaymentDataModel> { new SessionPaymentDataModel() }
                    }
                }
            }
        };
        private PaymenStatusResponseModel paymentStatusResponse = new PaymenStatusResponseModel()
        {
            PaymentStatus = new PaymentStatusModel()
            {
                Amount = 10000,
                PaymentId = paymentId,
                PaymentMethod = PaymentMethodEnum.Cash,
                Status = PaymentStatusEnum.Paid,
            },
            ObjectId = objectId
        };
        #endregion

        #region PrintLaneSessionReport
        // Happy case 200/201
        [Fact]
        public void GiveRequestIsValid_WhenApiPrintLaneSessionReportIsCalled_ThenReturnGoodValidation()
        {
            //arrange

            //act
            var actualResult = ValidateModelTest.ValidateModel(sessionReportRequest);

            //assert 
            actualResult.Should().NotBeNull();
            Assert.True(actualResult.Count() == 0);
        }
        [Fact]
        public async Task GivenRequestIsValidAndCustomVehicleTypeIsNull_WhenPrintLaneSessionReportIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _uiActionServiceMock.Setup(x => x.PrintLaneSessionReport(It.IsAny<SessionReportRequestModel>())).ReturnsAsync(ValidationResult.Success(sessionReportResponse));

            // Act
            var controller = new UIActionController(_loggerMock.Object, _uiActionServiceMock.Object, _publisherServiceMock.Object, _publisherOptions, _mapper);
            var actualResult = await controller.PrintLaneSessionReport(sessionReportRequest);
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<SessionReportModel>;

            // Assert
            _uiActionServiceMock.Verify(x => x.PrintLaneSessionReport(It.IsAny<SessionReportRequestModel>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.PrintLaneSessionReport)}...", Times.Once, _nullException);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.PrintLaneSessionReport)} method", Times.Never, _nullException);

            actualResult.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data?.Should().NotBeNull();
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().BeEquivalentTo(sessionReportResponse);
        }

        // Unhappy case 400
        [Fact]
        public void GiveRequestIsInValid_WhenApiPrintLaneSessionReportIsCalled_ThenReturnBadValidation()
        {
            //arrange
            var sessionReportRequest = new SessionReportRequestModel();

            //act
            var actualResult = ValidateModelTest.ValidateModel(sessionReportRequest);

            //assert 
            actualResult.Should().NotBeNull();
            Assert.True(actualResult.Count() > 0);
        }
        [Fact]
        public async Task GivenRequestIsValidAndFeesCalculationServiceIsDown_WhenPrintLaneSessionReportIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            var someEx = new Exception("An error occurred when calling PrintLaneSessionReport method");
            _uiActionServiceMock.Setup(x => x.PrintLaneSessionReport(It.IsAny<SessionReportRequestModel>())).ThrowsAsync(someEx);

            // Act
            var controller = new UIActionController(_loggerMock.Object, _uiActionServiceMock.Object, _publisherServiceMock.Object, _publisherOptions, _mapper);
            var actualResult = await controller.PrintLaneSessionReport(sessionReportRequest);
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.PrintLaneSessionReport)}...", Times.Once, _nullException);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.PrintLaneSessionReport)} method", Times.Once, _nullException);
            ((ObjectResult)actualResult).StatusCode.Should().Be(500);
            actualResultRespone?.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone?.Errors.Count > 0);
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
            var controller = new UIActionController(_loggerMock.Object, _uiActionServiceMock.Object, _publisherServiceMock.Object, _publisherOptions, _mapper);
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
            var controller = new UIActionController(_loggerMock.Object, _uiActionServiceMock.Object, _publisherServiceMock.Object, _publisherOptions, _mapper);
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
            var controller = new UIActionController(_loggerMock.Object, _uiActionServiceMock.Object, _publisherServiceMock.Object, _publisherOptions, _mapper);
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
    }
}
