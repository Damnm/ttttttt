using EPAY.ETC.Core.API.IntegrationTests.Common;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.VehicleFee;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace EPAY.ETC.Core.API.IntegrationTests.Controllers.Fees
{
    public class FeeCalculationControllerTests : IntegrationTestBase
    {
        #region AddAsync
        [Theory]
        [InlineData("843206065135832015", "51A3268", null, 1694482425, 1694483925, 15000)]
        [InlineData(null, "29A3268", null, 1694483925, 1694501925, 0)]
        [InlineData("235345234524234442", "79A3268", null, 1694483925, 1694501925, 70000)]
        [InlineData(null, null, CustomVehicleTypeEnum.Type1, 1694482425, 1694483425, 14000)]
        [InlineData(null, null, CustomVehicleTypeEnum.Type2, 1694483425, 1694483925, 14000)]
        [InlineData(null, null, CustomVehicleTypeEnum.Type3, 1694483925, 1694491925, 66000)]
        [InlineData(null, null, CustomVehicleTypeEnum.Type4, 1694491925, 1694501925, 80000)]
        public async Task GivenRequestIsValid_WhenApiAddAsyncIsCalled_ThenReturnCorrectResult(string? rfid, string? plateNumber, CustomVehicleTypeEnum? customVehicleType, long checkInDateEpoch, long checkOutDateEpoch, double amount)
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            FeeCalculationRequestModel request = new FeeCalculationRequestModel()
            {
                CheckInDateEpoch = checkInDateEpoch,
                CheckOutDateEpoch = checkOutDateEpoch,
                PlateNumber = plateNumber,
                RFID = rfid,
                CustomVehicleType = customVehicleType
            };
            int duration = Convert.ToInt32(checkOutDateEpoch - checkInDateEpoch);

            // Act
            var result = await HttpClient.PostAsJsonAsync($"/api/FeeCalculation/v1/calculate", request);
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var data = reports?["data"]?.AsObject();
            var fee = data?["fee"]?.AsObject();
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeEmpty();
            successful?.GetValue<bool>().Should().BeTrue();
            data.Should().NotBeNull();
            fee.Should().NotBeNull();
            fee?["amount"]?.AsValue().GetValue<double>().Should().Be(amount);
            fee?["duration"]?.AsValue().GetValue<int>().Should().Be(duration);
        }

        [Fact]
        public async Task GivenRequestIsInValid_WhenAddAsyncIsCalled_ThenReturnBadRequest()
        {
            // Arrange
            FeeCalculationRequestModel request = new FeeCalculationRequestModel();
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.PostAsJsonAsync($"/api/FeeCalculation/v1/calculate", request);
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().NotBeEmpty();
            successful?.GetValue<bool>().Should().BeFalse();
        }
        #endregion
    }
}
