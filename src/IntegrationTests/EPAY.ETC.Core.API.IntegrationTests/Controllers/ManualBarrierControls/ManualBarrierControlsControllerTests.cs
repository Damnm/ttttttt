using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.IntegrationTests.Common;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.Request;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using XUnitPriorityOrderer;

namespace EPAY.ETC.Core.API.IntegrationTests.Controllers.ManualBarrierControls
{
    [TestCaseOrderer(CasePriorityOrderer.TypeName, CasePriorityOrderer.AssembyName)]
    public class ManualBarrierControlsControllerTests : IntegrationTestBase
    {
        private static Guid _manualBarrierControlsControllerId = Guid.NewGuid();
        private static ManualBarrierControlAddOrUpdateRequestModel request = new ManualBarrierControlAddOrUpdateRequestModel()
        {
            EmployeeId = "a71717fb-cffd-4974-89d9-775a62d89395",
            Action = BarrierActionEnum.Close,
            LaneOutId = "001"
        };
        #region AddAsync
        [Fact, Order(1)]
        public async Task GivenValidRequest_WhenApiAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);
            // Act
            var result = await HttpClient.PostAsJsonAsync($"/api/ManualBarrierControl/v1/manualbarriercontrol", request);
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var data = reports?["data"]?.AsObject();
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.Created);
            content.Should().NotBeEmpty();
            successful?.GetValue<bool>().Should().BeTrue();
            data.Should().NotBeNull();
            _manualBarrierControlsControllerId = data?["id"]?.GetValue<Guid?>() ?? Guid.NewGuid();
        }

        [Fact, Order(2)]
        public async Task GivenRequestIsInValid_WhenAddAsyncIsCalled_ThenReturnBadRequest()
        {
            // Arrange
            ManualBarrierControlAddOrUpdateRequestModel request = new ManualBarrierControlAddOrUpdateRequestModel();
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.PostAsJsonAsync($"/api/ManualBarrierControl/v1/manualbarriercontrol", request);
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().NotBeEmpty();
            successful!.GetValue<bool>().Should().BeFalse();
        }
        #endregion

        #region GetByIdAsync
        [Fact, Order(3)]
        public async Task GivenRequestIsValid_WhenGetByIdAsync_ThenReturnCorrectResult()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.GetAsync($"/api/ManualBarrierControl/v1/manualbarriercontrols/{_manualBarrierControlsControllerId}");
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var data = reports?["data"]?.AsObject();
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeEmpty();
            data.Should().NotBeNull();
            successful?.GetValue<bool>().Should().BeTrue();
            data?["EmployeeId"]?.GetValue<string>().Should().Be(request.EmployeeId);
            data?["Action"]?.GetValue<BarrierActionEnum>().Should().Be(request.Action);
            data?["LaneOutId"]?.GetValue<string>().Should().Be(request.LaneOutId);
        }

        [Fact, Order(3)]
        public async Task GivenNonExistingGuid_WhenGetByIdAsyncIsCalled_ThenReturnEmpty()
        {
            // Arrange
            var _manualBarrierControlsControllerId = Guid.NewGuid();
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.GetAsync($"/api/ManualBarrierControl/v1/manualbarriercontrols/{_manualBarrierControlsControllerId}");
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var data = reports?["data"]?.AsObject();
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeEmpty();
            data.Should().BeNull();
            successful?.GetValue<bool>().Should().BeFalse();
        }
        #endregion
        #region UpdateAsync
        [Fact, Order(4)]
        public async Task GivenRequestIsValid_WhenUpdateAsync_ThenReturnCorrectResult()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);
            request.EmployeeId = "a71717fb-cffd-4974-89d9-775a62d89399";
            request.Action = BarrierActionEnum.Close;
            request.LaneOutId = "001";

            // Act
            var result = await HttpClient.PutAsJsonAsync($"/api/ManualBarrierControl/v1/manualbarriercontrols/{_manualBarrierControlsControllerId}", request);
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var data = reports?["data"]?.AsObject();
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeEmpty();
            successful?.GetValue<bool>().Should().BeTrue();
            data.Should().NotBeNull();
            data?["EmployeeId"]?.GetValue<string>().Should().Be(request.EmployeeId);
            data?["Action"]?.GetValue<BarrierActionEnum>().Should().Be(request.Action);
            data?["LaneOutId"]?.GetValue<string>().Should().Be(request.LaneOutId);
        }

        [Fact, Order(5)]
        public async Task GivenNonExistingGuid_WhenUpdateAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);
            var _manualBarrierControlsControllerId = Guid.NewGuid();

            // Act
            var result = await HttpClient.PutAsJsonAsync($"/api/ManualBarrierControl/v1/manualbarriercontrols/{_manualBarrierControlsControllerId}", request);
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var data = reports?["data"]?.AsObject();
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            content.Should().NotBeEmpty();
            successful?.GetValue<bool>().Should().BeFalse();
            data.Should().BeNull();
        }

        [Fact, Order(5)]
        public async Task GivenRequestIsInValid_WhenUpdateAsyncIsCalled_ThenReturnBadRequest()
        {
            // Arrange
            VehicleRequestModel request = new VehicleRequestModel();
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.PutAsJsonAsync($"/api/ManualBarrierControl/v1/manualbarriercontrols/{_manualBarrierControlsControllerId}", request);
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().NotBeEmpty();
            successful?.GetValue<bool>().Should().BeFalse();
        }
        #endregion
        #region RemoveAsync
        [Fact, Order(6)]
        public async Task GivenRequestIsValid_WhenRemoveAsync_ThenReturnCorrect()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.DeleteAsync($"/api/ManualBarrierControl/v1/manualbarriercontrols/{_manualBarrierControlsControllerId}");
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var data = reports?["data"]?.AsObject();
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeEmpty();
            successful!.GetValue<bool>().Should().BeTrue();
            data.Should().BeNull();
        }

        [Fact, Order(7)]
        public async Task GivenRequestIsValidAndNonExistingGuid_WhenRemoveAsync_ThenReturnNotFound()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.DeleteAsync($"/api/ManualBarrierControl/v1/manualbarriercontrols/{_manualBarrierControlsControllerId}");
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var data = reports?["data"]?.AsObject();
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            content.Should().NotBeEmpty();
            successful!.GetValue<bool>().Should().BeFalse();
            data.Should().BeNull();
        }
        #endregion
    }
}
