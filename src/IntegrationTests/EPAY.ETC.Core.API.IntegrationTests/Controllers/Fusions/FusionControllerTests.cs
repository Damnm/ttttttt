using EPAY.ETC.Core.API.Core.Models.Fusion;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.IntegrationTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using XUnitPriorityOrderer;

namespace EPAY.ETC.Core.API.IntegrationTests.Controllers.Fusions
{
    [TestCaseOrderer(CasePriorityOrderer.TypeName, CasePriorityOrderer.AssembyName)]
    public class FusionControllerTests : IntegrationTestBase
    {
        private static Guid _fusionId = Guid.NewGuid();

        private static FusionAddRequestModel request = new FusionAddRequestModel()
        {
            Epoch = 01524,
            Loop1 = true,
            RFID = false,
            Cam1 = "12A12356",
            Loop2 = true,
            Cam2 = "12A12356",
            Loop3 = true,
            ReversedLoop1 = true,
            ReversedLoop2 = true,
        };
        #region AddAsync
        [Fact, Order(1)]
        public async Task GivenValidRequest_WhenApiAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);
            // Act
            var result = await HttpClient.PostAsJsonAsync($"/api/Fusion/v1/fusions", request);
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var data = reports?["data"]?.AsObject();
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.Created);
            content.Should().NotBeEmpty();
            successful?.GetValue<bool>().Should().BeTrue();
            data.Should().NotBeNull();
            _fusionId = data?["id"]?.GetValue<Guid?>() ?? Guid.NewGuid();
        }

        [Fact, Order(2)]
        public async Task GivenRequestIsInValid_WhenAddAsyncIsCalled_ThenReturnBadRequest()
        {
            // Arrange
            VehicleRequestModel request = new VehicleRequestModel();
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.PostAsJsonAsync($"/api/Fusion/v1/fusions", request);
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
            var result = await HttpClient.GetAsync($"/api/Fusion/v1/fusions/{_fusionId}");
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var data = reports?["data"]?.AsObject();
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeEmpty();
            data.Should().NotBeNull();
            successful?.GetValue<bool>().Should().BeTrue();
            data?["Epoch"]?.GetValue<float>().Should().Be(request.Epoch);
            data?["Loop1"]?.GetValue<bool>().Should().Be(request.Loop1);
            data?["RFID"]?.GetValue<bool>().Should().Be(request.RFID);
            data?["Cam1"]?.GetValue<string>().Should().Be(request.Cam1);
            data?["Loop2"]?.GetValue<bool>().Should().Be(request.Loop2);
            data?["Cam2"]?.GetValue<string>().Should().Be(request.Cam2);
            data?["Loop3"]?.GetValue<bool>().Should().Be(request.Loop3);
            data?["ReversedLoop1"]?.GetValue<bool>().Should().Be(request.ReversedLoop1);
            data?["ReversedLoop2"]?.GetValue<bool>().Should().Be(request.ReversedLoop2);

        }

        [Fact, Order(3)]
        public async Task GivenNonExistingGuid_WhenGetByIdAsyncIsCalled_ThenReturnEmpty()
        {
            // Arrange
            var _fusionId = Guid.NewGuid();
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.GetAsync($"/api/Fusion/v1/fusions/{_fusionId}");
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
            request.Epoch = 666;
            request.Loop1 = false;
            request.RFID = false;
            request.Cam1 = "12A9999";
            request.Loop2 = false;
            request.Cam2 = "12A9999";
            request.Loop3 = false;
            request.ReversedLoop1 = false;
            request.ReversedLoop2 = false;

            // Act
            var result = await HttpClient.PutAsJsonAsync($"/api/Fusion/v1/fusions/{_fusionId}", request);
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var data = reports?["data"]?.AsObject();
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeEmpty();
            successful?.GetValue<bool>().Should().BeTrue();
            data.Should().NotBeNull();
            data?["Epoch"]?.GetValue<float>().Should().Be(request.Epoch);
            data?["Loop1"]?.GetValue<bool>().Should().Be(request.Loop1);
            data?["RFID"]?.GetValue<bool>().Should().Be(request.RFID);
            data?["Cam1"]?.GetValue<string>().Should().Be(request.Cam1);
            data?["Loop2"]?.GetValue<bool>().Should().Be(request.Loop2);
            data?["Cam2"]?.GetValue<string>().Should().Be(request.Cam2);
            data?["Loop3"]?.GetValue<bool>().Should().Be(request.Loop3);
            data?["ReversedLoop1"]?.GetValue<bool>().Should().Be(request.ReversedLoop1);
            data?["ReversedLoop2"]?.GetValue<bool>().Should().Be(request.ReversedLoop2);
        }

        [Fact, Order(5)]
        public async Task GivenNonExistingGuid_WhenUpdateAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);
            var _fusionId = Guid.NewGuid();

            // Act
            var result = await HttpClient.PutAsJsonAsync($"/api/Fusion/v1/fusions/{_fusionId}", request);
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
            var result = await HttpClient.PutAsJsonAsync($"/api/Fusion/v1/fusions/{_fusionId}", request);
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
            var result = await HttpClient.DeleteAsync($"/api/Fusion/v1/fusions/{_fusionId}");
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
            var result = await HttpClient.DeleteAsync($"/api/Fusion/v1/fusions/{_fusionId}");
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
