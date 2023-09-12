using EPAY.ETC.Core.API.Core.Utils;
using EPAY.ETC.Core.Models.Enums;
using FluentAssertions;

namespace EPAY.ETC.Core.API.Core.UnitTests.Utils
{
    public class VehicleTypeConverterTests
    {
        [Theory]
        [InlineData(4, null, CustomVehicleTypeEnum.Type1)]
        [InlineData(5, 0, CustomVehicleTypeEnum.Type1)]
        [InlineData(5, 1000, CustomVehicleTypeEnum.Type1)]
        [InlineData(7, null, CustomVehicleTypeEnum.Type1)]
        [InlineData(3, 1, CustomVehicleTypeEnum.Type1)]
        [InlineData(3, 750, CustomVehicleTypeEnum.Type2)]
        [InlineData(5, 950, CustomVehicleTypeEnum.Type2)]
        [InlineData(5, 960, CustomVehicleTypeEnum.Type1)]
        [InlineData(7, 1500, CustomVehicleTypeEnum.Type1)]
        [InlineData(7, 1600, CustomVehicleTypeEnum.Type2)]
        [InlineData(10, null, CustomVehicleTypeEnum.Type2)]
        [InlineData(10, 3500, CustomVehicleTypeEnum.Type2)]
        [InlineData(17, 3500, CustomVehicleTypeEnum.Type2)]
        [InlineData(29, 7000, CustomVehicleTypeEnum.Type3)]
        [InlineData(29, 7100, CustomVehicleTypeEnum.Type3)]
        [InlineData(17, null, CustomVehicleTypeEnum.Type3)]
        [InlineData(29, null, CustomVehicleTypeEnum.Type3)]
        [InlineData(30, null, CustomVehicleTypeEnum.Type4)]
        [InlineData(30, 7100, CustomVehicleTypeEnum.Type4)]
        public void GivenValidInput_WhenConvertVehicleTypeIsCalled_ThenReturnCorrectResult(int seat, int? payload, CustomVehicleTypeEnum customVehicleType)
        {
            // Arrange


            // Act
            var result = VehicleTypeConverter.ConvertVehicleType(seat, payload);

            // Assert
            result.Should().Be(customVehicleType);
        }

    }
}
