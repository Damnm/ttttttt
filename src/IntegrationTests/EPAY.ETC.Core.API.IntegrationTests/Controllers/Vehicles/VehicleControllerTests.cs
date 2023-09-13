using EPAY.ETC.Core.API.Core.Interfaces.Services.Vehicles;
using EPAY.ETC.Core.API.IntegrationTests.Common;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using XUnitPriorityOrderer;
using FluentAssertions;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Core.Models.Common;
using EPAY.ETC.Core.API.Core.Extensions;

namespace EPAY.ETC.Core.API.IntegrationTests.Controllers.Vehicles
{
    [TestCaseOrderer(CasePriorityOrderer.TypeName, CasePriorityOrderer.AssembyName)]
    public class VehicleControllerTests : IntegrationTestBase
    {
        private static Guid? _vehicleId = Guid.Empty;

        private static VehicleRequestModel request = new VehicleRequestModel()
        {
            //Id = Guid.NewGuid(),
            //CreatedDate = DateTime.Now.ConvertToAsianTime(DateTimeKind.Local),
            PlateNumber = "Some Plate number",
            PlateColor = "Some Plate colour",
            RFID = "Some RFID",
            Make = "Some make",
            Seat = 10,
            VehicleType = "Loại 2",
            Weight = 7000,
        };

        #region AddAsync
        [Fact, Order(1)]
        public async Task GivenValidRequest_WhenApiAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);
            // Act
            var result = await HttpClient.PostAsJsonAsync($"/api/Vehicle/v1/vehicles", request);
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var data = reports?["data"]?.AsObject();
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.Created);
            content.Should().NotBeEmpty();
            successful?.GetValue<bool>().Should().BeTrue();
            data.Should().NotBeNull();
        }
        [Fact, Order(2)]
        public async Task GivenRequestIsValidAndVehiclesAlreadyExists_WhenAddAsyncIsCalled_ThenReturnConflict()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.PostAsJsonAsync($"/api/Vehicle/v1/vehicles/{_vehicleId}", request);
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var data = reports?["data"]?.AsObject();
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.Conflict);
            content.Should().NotBeEmpty();
            successful.GetValue<bool>().Should().BeFalse();
            data.Should().BeNull();
        }

        [Fact, Order(2)]
        public async Task GivenRequestIsInValid_WhenAddAsyncIsCalled_ThenReturnBadRequest()
        {
            // Arrange
            request = new VehicleRequestModel();
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.PostAsJsonAsync($"/api/Vehicle/v1/vehicles", request);
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
            var result = await HttpClient.GetAsync($"/api/Vehicle/v1/vehicles/{_vehicleId}");
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var data = reports?["data"]?.AsObject();
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeEmpty();
            data.Should().NotBeNull();
            successful?.GetValue<bool>().Should().BeTrue();
            data?["PlateNumber"]?.GetValue<string>().Should().Be(request.PlateNumber);
            data?["PlateColor"]?.GetValue<string>().Should().Be(request.PlateColor);
            data?["RFID"]?.GetValue<string>().Should().Be(request.RFID);
            data?["Make"]?.GetValue<string>().Should().Be(request.Make);
            data?["Seat"]?.GetValue<int>().Should().Be(request.Seat);
            data?["VehicleType"]?.GetValue<string>().Should().Be(request.VehicleType);
            data?["Weight"]?.GetValue<int>().Should().Be(request.Weight);

        }

        [Fact, Order(3)]
        public async Task GivenNonExistingGuid_WhenGetByIdAsyncIsCalled_ThenReturnEmpty()
        {
            // Arrange
            var _vehicleId = Guid.NewGuid();
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.GetAsync($"/api/Vehicle/v1/vehicles/ {_vehicleId}");
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
            request.PlateNumber = "Some Plate number";
            request.PlateColor = "Some Plate colour";
            request.RFID = "Some RFID";
            request.Make = "Some make";
            request.Seat = 10;
            request.VehicleType = "Loại 2";
            request.Weight = 7000;

            // Act
            var result = await HttpClient.PutAsJsonAsync($"/api/Vehicle/v1/vehicles/{_vehicleId}", request);
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var data = reports?["data"]?.AsObject();
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeEmpty();
            successful?.GetValue<bool>().Should().BeTrue();
            data.Should().NotBeNull();
            data?["PlateNumber"]?.GetValue<string>().Should().Be(request.PlateNumber);
            data?["PlateColor"]?.GetValue<string>().Should().Be(request.PlateColor);
            data?["RFID"]?.GetValue<string>().Should().Be(request.RFID);
            data?["Make"]?.GetValue<string>().Should().Be(request.Make);
            data?["Seat"]?.GetValue<int>().Should().Be(request.Seat);
            data?["VehicleType"]?.GetValue<string>().Should().Be(request.VehicleType);
            data?["Weight"]?.GetValue<int>().Should().Be(request.Weight);
        }

        [Fact, Order(5)]
        public async Task GivenNonExistingGuid_WhenUpdateAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);
            var _settingId = Guid.NewGuid();

            // Act
            var result = await HttpClient.PutAsJsonAsync($"/api/Vehicle/v1/vehicles/{_vehicleId}", request);
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
            VehicleRequestModel _request = new VehicleRequestModel();
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.PutAsJsonAsync($"/api/Vehicle/v1/vehicles/{_vehicleId}", request);
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
            var result = await HttpClient.DeleteAsync($"/api/Vehicle/v1/vehicles/{_vehicleId}");
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var data = reports?["data"]?.AsObject();
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeEmpty();
            successful!.GetValue<bool>().Should().BeTrue();
        }

        [Fact, Order(7)]
        public async Task GivenRequestIsValidAndNonExistingGuid_WhenRemoveAsync_ThenReturnNotFound()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.DeleteAsync($"/api/Vehicle/v1/vehicles/ {_vehicleId}");
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
