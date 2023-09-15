using EPAY.ETC.Core.API.Controllers.Fees;
using EPAY.ETC.Core.API.Core.Extensions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Fees;
using EPAY.ETC.Core.API.Core.Models.Enum;
using EPAY.ETC.Core.API.UnitTests.Helpers;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.Validation;
using EPAY.ETC.Core.Models.VehicleFee;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPAY.ETC.Core.API.UnitTests.Controllers.Fees
{
    public class FeeCalculationControllerTests : ControllerBase
    {
        #region Init method test
        private Mock<IFeeCalculationService> _feesCalculationService = new();
        private Mock<ILogger<FeeCalculationController>> _loggerMock = new();
        #endregion

        #region Init data test
        private Exception exception = null!;
        private VehicleFeeModel vehicleFee = new VehicleFeeModel()
        {
            Vehicle = new VehicleModel()
            {
                CustomVehicleTypeCode = CustomVehicleTypeEnum.Type1.ToString(),
                CustomVehicleTypeName = CustomVehicleTypeEnum.Type1.ToEnumMemberAttrValue(),
                Make = "Some make",
                Model = "Some model",
                PlateNumber = "Some plate",
                RFID = "Some RFID",
                VehicleCategoryName = "Some category",
                VehicleGroupName = "Some group"
            },
            Fee = new FeeModel()
            {
                Amount = 55000,
                Currency = CurrencyEnum.VND.ToString(),
                Duration = 10300
            }
        };
        private FeeCalculationRequestModel request = new FeeCalculationRequestModel()
        {
            CheckInDateEpoch = 1694403911,
            CheckOutDateEpoch = 1694414211,
            PlateNumber = "29A123456",
            RFID = "123456879213547",
            CustomVehicleType = CustomVehicleTypeEnum.Type1
        };
        #endregion

        #region CalculateFeeAsync
        // Happy case 200/201
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
        public async Task GivenRequestIsValidAndCustomVehicleTypeIsNull_WhenCalculateFeeAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var response = ValidationResult.Success(vehicleFee);
            var request = new FeeCalculationRequestModel()
            {
                CheckInDateEpoch = 1694403911,
                CheckOutDateEpoch = 1694414211,
                PlateNumber = "29A123456",
                RFID = "123456879213547"
            };

            _feesCalculationService.Setup(x => x.CalculateFeeAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<long>(), It.IsAny<long>())).ReturnsAsync(response);

            // Act
            var controller = new FeeCalculationController(_loggerMock.Object, _feesCalculationService.Object);
            var actualResult = await controller.CalculateFeeAsync(request);
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<VehicleFeeModel>;

            // Assert
            _feesCalculationService.Verify(x => x.CalculateFeeAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<long>(), It.IsAny<long>()), Times.Once);
            _feesCalculationService.Verify(x => x.CalculateFeeAsync(It.IsAny<string>(), It.IsAny<CustomVehicleTypeEnum>(), It.IsAny<long>(), It.IsAny<long>()), Times.Never);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.CalculateFeeAsync)}...", Times.Once, exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.CalculateFeeAsync)} method", Times.Never, exception);

            actualResult.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().NotBeNull();
            data?.Data.Should().BeEquivalentTo(vehicleFee);
        }
        [Fact]
        public async Task GivenRequestIsValidAndCustomVehicleTypeIsNotNull_WhenAddAsyncIsCalled_ThenReturnConflict()
        {
            // Arrange
            var response = ValidationResult.Success(vehicleFee);

            _feesCalculationService.Setup(x => x.CalculateFeeAsync(It.IsAny<string>(), It.IsAny<CustomVehicleTypeEnum>(), It.IsAny<long>(), It.IsAny<long>())).ReturnsAsync(response);

            // Act
            var controller = new FeeCalculationController(_loggerMock.Object, _feesCalculationService.Object);
            var actualResult = await controller.CalculateFeeAsync(request);
            var data = ((OkObjectResult)actualResult).Value as ValidationResult<VehicleFeeModel>;

            // Assert
            _feesCalculationService.Verify(x => x.CalculateFeeAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<long>(), It.IsAny<long>()), Times.Never);
            _feesCalculationService.Verify(x => x.CalculateFeeAsync(It.IsAny<string>(), It.IsAny<CustomVehicleTypeEnum>(), It.IsAny<long>(), It.IsAny<long>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.CalculateFeeAsync)}...", Times.Once, exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.CalculateFeeAsync)} method", Times.Never, exception);

            actualResult.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)actualResult).StatusCode.Should().Be(StatusCodes.Status200OK);
            data?.Succeeded.Should().BeTrue();
            data?.Data.Should().NotBeNull();
            data?.Data.Should().BeEquivalentTo(vehicleFee);
        }

        // Unhappy case 400
        [Fact]
        public void GiveRequestIsInValid_WhenApiAddAsyncIsCalled_ThenReturnBadValidation()
        {
            //arrange
            var request = new FeeCalculationRequestModel();

            //act
            var actualResult = ValidateModelTest.ValidateModel(request);

            //assert 
            actualResult.Should().NotBeNull();
            Assert.True(actualResult.Count() > 0);
        }
        [Fact]
        public async Task GivenRequestIsValidAndFeesCalculationServiceIsDown_WhenAddAsyncIsCalled_ThenReturnInternalServerError()
        {
            // Arrange
            var someEx = new Exception("An error occurred when calling AddAsync method");
            _feesCalculationService.Setup(x => x.CalculateFeeAsync(It.IsAny<string>(), It.IsAny<CustomVehicleTypeEnum>(), It.IsAny<long>(), It.IsAny<long>())).ThrowsAsync(someEx);

            // Act
            var controller = new FeeCalculationController(_loggerMock.Object, _feesCalculationService.Object);
            var actualResult = await controller.CalculateFeeAsync(request);
            var actualResultRespone = ((ObjectResult)actualResult).Value as ValidationResult<string>;

            // Assert
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(controller.CalculateFeeAsync)}...", Times.Once, exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(controller.CalculateFeeAsync)} method", Times.Once, exception);
            ((ObjectResult)actualResult).StatusCode.Should().Be(500);
            actualResultRespone?.Succeeded.Should().BeFalse();
            Assert.True(actualResultRespone?.Errors.Count > 0);
        }
        #endregion
    }
}
