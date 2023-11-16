using EPAY.ETC.Core.API.IntegrationTests.Common;
using EPAY.ETC.Core.Models.Request;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using XUnitPriorityOrderer;

namespace EPAY.ETC.Core.API.IntegrationTests.Controllers.Vehicles
{
    [TestCaseOrderer(CasePriorityOrderer.TypeName, CasePriorityOrderer.AssembyName)]
    public class PrintLogControllerTests : IntegrationTestBase
    {
        private static Guid? _printLogId = new Guid("9cdf01fe-2989-4098-b65f-50edbeccd7d5");

        private static PrintLogRequestModel request = new PrintLogRequestModel()
        {
            PrintLogId = _printLogId ?? Guid.NewGuid(),
            PlateNumber = "Some Plate number",
        };

        #region AddAsync
        [Fact, Order(1)]
        public async Task GivenValidRequest_WhenApiAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);
            // Act
            var result = await HttpClient.PostAsJsonAsync($"/api/PrintLog/v1/PrintLogs", request);
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var data = reports?["Data"]?.AsObject();
            var successful = reports?["Succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.Created);
            content.Should().NotBeEmpty();
            successful?.GetValue<bool>().Should().BeTrue();
            data.Should().NotBeNull();
            _printLogId = data?["Id"]?.GetValue<Guid?>();
        }
       
        #endregion
        #region GetByIdAsync
        [Fact, Order(3)]
        public async Task GivenRequestIsValid_WhenGetByIdAsync_ThenReturnCorrectResult()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.GetAsync($"/api/PrintLog/v1/printlogs/{_printLogId}");
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var data = reports?["Data"]?.AsObject();
            var successful = reports?["Succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeEmpty();
            data.Should().NotBeNull();
            successful?.GetValue<bool>().Should().BeTrue();
            data?["PlateNumber"]?.GetValue<string>().Should().Be(request.PlateNumber);
          

        }

        [Fact, Order(3)]
        public async Task GivenNonExistingGuid_WhenGetByIdAsyncIsCalled_ThenReturnEmpty()
        {
            // Arrange
            var _printLogId = Guid.NewGuid();
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.GetAsync($"/api/PrintLog/v1/printlogs/{_printLogId}");
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var data = reports?["Data"]?.AsObject();
            var successful = reports?["Succeeded"]?.AsValue();

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
           

            // Act
            var result = await HttpClient.PutAsJsonAsync($"/api/PrintLog/v1/printlogs/{_printLogId}", request);
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var data = reports?["Data"]?.AsObject();
            var successful = reports?["Succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeEmpty();
            successful?.GetValue<bool>().Should().BeTrue();
            data.Should().NotBeNull();
            data?["PlateNumber"]?.GetValue<string>().Should().Be(request.PlateNumber);
          
        }

        [Fact, Order(5)]
        public async Task GivenNonExistingGuid_WhenUpdateAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);
            var _printLogId = Guid.NewGuid();

            // Act
            var result = await HttpClient.PutAsJsonAsync($"/api/PrintLog/v1/printlogs/{_printLogId}", request);
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var data = reports?["Data"]?.AsObject();
            var successful = reports?["Succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            content.Should().NotBeEmpty();
            successful?.GetValue<bool>().Should().BeFalse();
            data.Should().BeNull();
        }

      
        #endregion
        #region RemoveAsync
        [Fact, Order(6)]
        public async Task GivenRequestIsValid_WhenRemoveAsync_ThenReturnCorrect()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.DeleteAsync($"/api/PrintLog/v1/printlogs/{_printLogId}");
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var data = reports?["Data"]?.AsObject();
            var successful = reports?["Succeeded"]?.AsValue();

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
            var result = await HttpClient.DeleteAsync($"/api/PrintLog/v1/printlogs/{_printLogId}");
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var data = reports?["Data"]?.AsObject();
            var successful = reports?["Succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            content.Should().NotBeEmpty();
            successful!.GetValue<bool>().Should().BeFalse();
            data.Should().BeNull();
        }
        #endregion
    }
}
