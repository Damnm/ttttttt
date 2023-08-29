using EPAY.ETC.Core.API.Core.Interfaces.Services.Vehicles;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
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

namespace EPAY.ETC.Core.API.IntegrationTests.Controllers
{
    [TestCaseOrderer(CasePriorityOrderer.TypeName, CasePriorityOrderer.AssembyName)]
    public class VehicleControllerTests : IntegrationTestBase
    {
        private static Guid _vehicleId = Guid.Empty;
        private readonly IVehicleService _vehicleService;
        private readonly ILogger _logger;

        private static VehicleModel request = new VehicleModel()
        {
            Id = _vehicleId,
            CreatedDate = DateTime.Now,
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
            successful.GetValue<bool>().Should().BeTrue();
            data.Should().NotBeNull();
            data?["PlateNumber"]?.GetValue<string>().Should().Be(request.PlateNumber);
            _vehicleId = (Guid)(data?["id"]?.GetValue<Guid>());
        }

        [Fact, Order(2)]
        public async Task GivenInvalidRequest_WhenApiAddAsyncIsCalled_ThenReturnBadRequest()
        {
            // Arrange
            VehicleModel _request = new VehicleModel();

            // Act
            var result = await HttpClient.PostAsJsonAsync($"/api/priorityVehicle/v1/vehicles", _request);
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().NotBeEmpty();
            successful.GetValue<bool>().Should().BeFalse();
        }
        #endregion
    }
}
