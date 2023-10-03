using EPAY.ETC.Core.API.Controllers.Devices;
using EPAY.ETC.Core.API.Core.Interfaces.Services.UIActions;
using EPAY.ETC.Core.API.Models.Configs;
using EPAY.ETC.Core.API.UnitTests.Common;
using EPAY.ETC.Core.API.UnitTests.Helpers;
using EPAY.ETC.Core.Models.BarrierOpenStatus;
using EPAY.ETC.Core.Models.Enums;
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

namespace EPAY.ETC.Core.API.UnitTests.Controllers.Devices
{
    public class DeviceControllerTests : TestBase<DeviceController>
    {
        #region Init mock instance
        private Mock<IUIActionService> _uiActionServiceMock = new();
        private Mock<IPublisherService> _publisherServiceMock = new();
        IOptions<List<PublisherConfigurationOption>> _publisherOptions = Options.Create(new List<PublisherConfigurationOption> { new PublisherConfigurationOption() });
        #endregion

        #region Init mock data
        private BarrierRequestModel barrierRequest = new BarrierRequestModel()
        {
            EmployeeId = "Some",
            Action = BarrierActionEnum.Open,
            Limit = 0
        };
        private BarrierOpenStatus barrierOpenStatusResponse = new BarrierOpenStatus()
        {
            Limit = 0,
            Status = BarrierActionEnum.Open
        };
        #endregion

        #region ManipulateBarrier
        // Happy case 200/201
        [Fact]
        public void GiveRequestIsValid_WhenApiManipulateBarrierIsCalled_ThenReturnGoodValidation()
        {
            //arrange

            //act
            var actualResult = ValidateModelTest.ValidateModel(barrierRequest);

            //assert 
            actualResult.Should().NotBeNull();
            Assert.True(actualResult.Count() == 0);
        }
        [Fact]
        public async Task GivenRequestIsValidAndCustomVehicleTypeIsNull_WhenManipulateBarrierIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _uiActionServiceMock.Setup(x => x.ManipulateBarrier(It.IsAny<BarrierRequestModel>())).ReturnsAsync(ValidationResult.Success(barrierOpenStatusResponse));
            _publisherServiceMock.Setup(x => x.SendMessage(It.IsAny<RabbitMessageOutbound>(), It.IsAny<PublisherOptions>()));

            // Act
            var controller = new DeviceController(_loggerMock.Object, _uiActionServiceMock.Object, _publisherServiceMock.Object, _publisherOptions, _mapper);
            var actualResult = await controller.ManipulateBarrier(barrierRequest);

            // Assert
            _uiActionServiceMock.Verify(x => x.ManipulateBarrier(It.IsAny<BarrierRequestModel>()), Times.Once);
            _publisherServiceMock.Verify(x => x.SendMessage(It.IsAny<RabbitMessageOutbound>(), It.IsAny<PublisherOptions>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.ManipulateBarrier)}...", Times.Once, _nullException);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.ManipulateBarrier)} method", Times.Never, _nullException);

            actualResult.Should().BeOfType<OkResult>();
            ((OkResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        // Unhappy case 400
        [Fact]
        public async Task GivenRequestIsValidAndFeesCalculationServiceIsDown_WhenManipulateBarrierIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            var someEx = new Exception("An error occurred when calling ManipulateBarrier method");
            _uiActionServiceMock.Setup(x => x.ManipulateBarrier(It.IsAny<BarrierRequestModel>())).ThrowsAsync(someEx);

            // Act
            var controller = new DeviceController(_loggerMock.Object, _uiActionServiceMock.Object, _publisherServiceMock.Object, _publisherOptions, _mapper);
            var actualResult = await controller.ManipulateBarrier(barrierRequest);
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.ManipulateBarrier)}...", Times.Once, _nullException);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.ManipulateBarrier)} method", Times.Once, _nullException);
            ((ObjectResult)actualResult).StatusCode.Should().Be(500);
            actualResultRespone?.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone?.Errors.Count > 0);
        }
        [Fact]
        public async Task GivenRequestIsValidAndPublisherServiceIsDown_WhenManipulateBarrierIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            var someEx = new Exception("An error occurred when calling ManipulateBarrier method");
            _uiActionServiceMock.Setup(x => x.ManipulateBarrier(It.IsAny<BarrierRequestModel>())).ReturnsAsync(ValidationResult.Success(barrierOpenStatusResponse));
            _publisherServiceMock.Setup(x => x.SendMessage(It.IsAny<RabbitMessageOutbound>(), It.IsAny<PublisherOptions>())).Throws(someEx);

            // Act
            var controller = new DeviceController(_loggerMock.Object, _uiActionServiceMock.Object, _publisherServiceMock.Object, _publisherOptions, _mapper);
            var actualResult = await controller.ManipulateBarrier(barrierRequest);
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.ManipulateBarrier)}...", Times.Once, _nullException);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.ManipulateBarrier)} method", Times.Once, _nullException);
            ((ObjectResult)actualResult).StatusCode.Should().Be(500);
            actualResultRespone?.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone?.Errors.Count > 0);
        }
        #endregion
    }
}
