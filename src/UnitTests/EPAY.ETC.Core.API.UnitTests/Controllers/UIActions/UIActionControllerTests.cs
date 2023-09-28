using EPAY.ETC.Core.API.Controllers.UIActions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.UIActions;
using EPAY.ETC.Core.API.UnitTests.Common;
using EPAY.ETC.Core.API.UnitTests.Helpers;
using EPAY.ETC.Core.Models.Receipt.SessionReports;
using EPAY.ETC.Core.Models.Validation;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPAY.ETC.Core.API.UnitTests.Controllers.UIActions
{
    public class UIActionControllerTests : TestBase<UIActionController>
    {
        #region Init mock instance
        private Mock<IUIActionService> _uiActionServiceMock = new();
        #endregion

        #region Init mock data
        private SessionReportRequestModel request = new SessionReportRequestModel()
        {
            FromDate = new DateTime(2023, 9, 29, 15, 32, 19),
            ToDate = new DateTime(2023, 9, 29, 15, 43, 53),
            EmployeeId = "Some employee",
            LaneId = "Some lane"
        };
        private SessionReportModel response = new SessionReportModel()
        {
            PrintType = Models.Enums.ReceiptTypeEnum.SessionReport,
            Layout = new SessionLayoutModel()
            {
                Header = new Models.Receipt.HeaderModel()
                {
                    Heading = "Some heading",
                    Line1 = "Some line",
                    Line2 = "Some line",
                    SubHeading = "Some sub heading"
                },
                Footer = new Models.Receipt.FooterModel()
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
        #endregion

        #region PrintLaneSessionReport
        // Happy case 200/201
        [Fact]
        public void GiveRequestIsValid_WhenApiPrintLaneSessionReportIsCalled_ThenReturnGoodValidation()
        {
            //arrange

            //act
            var actualResult = ValidateModelTest.ValidateModel(request);

            //assert 
            actualResult.Should().NotBeNull();
            Assert.True(actualResult.Count() == 0);
        }
        [Fact]
        public async Task GivenRequestIsValidAndCustomVehicleTypeIsNull_WhenPrintLaneSessionReportIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _uiActionServiceMock.Setup(x => x.PrintLaneSessionReport(It.IsAny<SessionReportRequestModel>())).ReturnsAsync(ValidationResult.Success(response));

            // Act
            var controller = new UIActionController(_loggerMock.Object, _uiActionServiceMock.Object);
            var actualResult = await controller.PrintLaneSessionReport(request);
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<SessionReportModel>;

            // Assert
            _uiActionServiceMock.Verify(x => x.PrintLaneSessionReport(It.IsAny<SessionReportRequestModel>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.PrintLaneSessionReport)}...", Times.Once, _nullException);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.PrintLaneSessionReport)} method", Times.Never, _nullException);

            actualResult.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data?.Should().NotBeNull();
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().BeEquivalentTo(response);
        }

        // Unhappy case 400
        [Fact]
        public void GiveRequestIsInValid_WhenApiPrintLaneSessionReportIsCalled_ThenReturnBadValidation()
        {
            //arrange
            var request = new SessionReportRequestModel();

            //act
            var actualResult = ValidateModelTest.ValidateModel(request);

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
            var controller = new UIActionController(_loggerMock.Object, _uiActionServiceMock.Object);
            var actualResult = await controller.PrintLaneSessionReport(request);
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.PrintLaneSessionReport)}...", Times.Once, _nullException);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.PrintLaneSessionReport)} method", Times.Once, _nullException);
            ((ObjectResult)actualResult).StatusCode.Should().Be(500);
            actualResultRespone?.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone?.Errors.Count > 0);
        }
        #endregion
    }
}
