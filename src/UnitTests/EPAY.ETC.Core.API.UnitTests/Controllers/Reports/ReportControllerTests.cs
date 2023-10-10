using EPAY.ETC.Core.API.Controllers.Reports;
using EPAY.ETC.Core.API.Core.Interfaces.Services.UIActions;
using EPAY.ETC.Core.API.UnitTests.Common;
using EPAY.ETC.Core.API.UnitTests.Helpers;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.Receipt;
using EPAY.ETC.Core.Models.Receipt.SessionReports;
using EPAY.ETC.Core.Models.Validation;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPAY.ETC.Core.API.UnitTests.Controllers.Reports
{
    public class ReportControllerTests : TestBase<ReportController>
    {
        #region Init mock instance
        private Mock<IUIActionService> _uiActionServiceMock = new();
        #endregion

        #region Init mock data
        private LaneSessionReportRequestModel sessionReportRequest = new LaneSessionReportRequestModel()
        {
            FromDateTimeEpoch = 1696909646,
            ToDateTimeEpoch = 1696909646,
            EmployeeId = "Some employee",
            LaneOutId = "Some lane"
        };
        private LaneSessionReportModel sessionReportResponse = new LaneSessionReportModel()
        {
            PrintType = ReceiptTypeEnum.SessionReport,
            Layout = new LaneSessionLayoutModel()
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
                Body = new LaneSessionBodyModel()
                {
                    Columns = new List<string>() { "Some column" },
                    Heading = "Some Heading",
                    SubHeading1 = "Some sub heading 1",
                    SubHeading2 = "Some sub heading 2",
                    SubHeading3 = "Some sub heading 3",
                    Data = new LaneSessionDataModel()
                    {
                        BottomLine = "Some line",
                        GrandTotal = 153000,
                        Qty = 15,
                        Payments = new List<LaneSessionPaymentDataModel> { new LaneSessionPaymentDataModel() }
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
            var actualResult = ValidateModelTest.ValidateModel(sessionReportRequest);

            //assert 
            actualResult.Should().NotBeNull();
            Assert.True(actualResult.Count() == 0);
        }
        [Fact]
        public async Task GivenRequestIsValidAndCustomVehicleTypeIsNull_WhenPrintLaneSessionReportIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _uiActionServiceMock.Setup(x => x.PrintLaneSessionReport(It.IsAny<LaneSessionReportRequestModel>())).ReturnsAsync(ValidationResult.Success(sessionReportResponse));

            // Act
            var controller = new ReportController(_loggerMock.Object, _uiActionServiceMock.Object);
            var actualResult = await controller.PrintLaneSessionReport(sessionReportRequest);
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<LaneSessionReportModel>;

            // Assert
            _uiActionServiceMock.Verify(x => x.PrintLaneSessionReport(It.IsAny<LaneSessionReportRequestModel>()), Times.Once);

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
            var sessionReportRequest = new LaneSessionReportRequestModel();

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
            _uiActionServiceMock.Setup(x => x.PrintLaneSessionReport(It.IsAny<LaneSessionReportRequestModel>())).ThrowsAsync(someEx);

            // Act
            var controller = new ReportController(_loggerMock.Object, _uiActionServiceMock.Object);
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
    }
}
