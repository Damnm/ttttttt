using EPAY.ETC.Core.API.Core.Models.Fusion;
using EPAY.ETC.Core.API.Core.Models.ManualBarrierControl;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.IntegrationTests.Common;
using EPAY.ETC.Core.Models.Enums;
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

namespace EPAY.ETC.Core.API.IntegrationTests.Controllers.ManualBarrierControls
{
    [TestCaseOrderer(CasePriorityOrderer.TypeName, CasePriorityOrderer.AssembyName)]
    public class ManualBarrierControlsControllerTests : IntegrationTestBase
    {
        private static Guid _manualBarrierControlsControllerId = Guid.NewGuid();
        private static ManualBarrierControlAddOrUpdateRequestModel request = new ManualBarrierControlAddOrUpdateRequestModel()
        {
            EmployeeId = "a71717fb-cffd-4974-89d9-775a62d89399",
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
            var result = await HttpClient.PostAsJsonAsync($"/v1/manualbarriercontrol", request);
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
            var result = await HttpClient.PostAsJsonAsync($"/v1/manualbarriercontrol", request);
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().NotBeEmpty();
            successful!.GetValue<bool>().Should().BeFalse();
        }
        #endregion
    }
}
