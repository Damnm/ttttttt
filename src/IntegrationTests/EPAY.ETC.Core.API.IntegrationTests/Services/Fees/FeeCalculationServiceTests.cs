using EPAY.ETC.Core.API.Core.Interfaces.Services.Fees;
using EPAY.ETC.Core.API.IntegrationTests.Common;
using EPAY.ETC.Core.Models.Enums;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using XUnitPriorityOrderer;

namespace EPAY.ETC.Core.API.IntegrationTests.Services.Fees
{
    [TestCaseOrderer(CasePriorityOrderer.TypeName, CasePriorityOrderer.AssembyName)]
    public class FeeCalculationServiceTests : IntegrationTestBase
    {
        #region Init Method
        private IFeeCalculationService? _service;
        #endregion

        #region Init Data test
        #endregion

        #region CalculateFeeAsync
        [Theory]
        [InlineData("843206065135832015", "51A3268", 1694482425, 1694483925, 15000)]
        [InlineData("", "29A3268", 1694483925, 1694501925, 0)]
        [InlineData("235345234524234442", "79A3268", 1694483925, 1694501925, 70000)]
        public async Task GivenRequestIsValid_WhenCalculateFeeAsyncIsCalled_ThenReturnCorrectResult(string rfid, string? plateNumber, long checkInDateEpoch, long checkOutDateEpoch, double amount)
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _service = scope.ServiceProvider.GetRequiredService<IFeeCalculationService>();

            // Arrange
            int duration = Convert.ToInt32(checkOutDateEpoch - checkInDateEpoch);

            // Act
            var result = await _service.CalculateFeeAsync(rfid, plateNumber, checkInDateEpoch, checkOutDateEpoch);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data?.Fee.Should().NotBeNull();
            result.Data?.Fee?.Amount.Should().Be(amount);
        }

        [Theory]
        [InlineData("", CustomVehicleTypeEnum.Type1, 1694482425, 1694483425, 14000)]
        [InlineData("", CustomVehicleTypeEnum.Type2, 1694483425, 1694483925, 14000)]
        [InlineData("", CustomVehicleTypeEnum.Type3, 1694483925, 1694491925, 66000)]
        [InlineData("", CustomVehicleTypeEnum.Type4, 1694491925, 1694501925, 80000)]
        public async Task GivenRequestIsValidAndRFIDIsNotExists_WhenCalculateFeeAsyncIsCalled_ThenReturnCorrectResult(string plateNumber, CustomVehicleTypeEnum customVehicleType, long checkInDateEpoch, long checkOutDateEpoch, double amount)
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _service = scope.ServiceProvider.GetRequiredService<IFeeCalculationService>();

            // Arrange
            int duration = Convert.ToInt32(checkOutDateEpoch - checkInDateEpoch);

            // Act
            var result = await _service.CalculateFeeAsync(plateNumber, customVehicleType, checkInDateEpoch, checkOutDateEpoch);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data?.Fee.Should().NotBeNull();
            result.Data?.Fee?.Amount.Should().Be(amount);
        }
        #endregion
    }
}
