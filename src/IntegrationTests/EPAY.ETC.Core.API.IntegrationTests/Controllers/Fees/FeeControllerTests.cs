using EPAY.ETC.Core.API.IntegrationTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using XUnitPriorityOrderer;
using CoreModel = EPAY.ETC.Core.Models.Fees;

namespace EPAY.ETC.Core.API.IntegrationTests.Controllers.Fees
{
    [TestCaseOrderer(CasePriorityOrderer.TypeName, CasePriorityOrderer.AssembyName)]
    public class FeeControllerTests : IntegrationTestBase
    {
        #region Init test data
        private static Guid testFeeId = Guid.Parse("b31717fb-cfad-4994-89d9-775a62d89399");
        private static Guid feeId = Guid.Parse("a71717fb-cfad-4994-89d9-775a62d89399");
        private CoreModel.FeeModel request = new CoreModel.FeeModel()
        {
            CreatedDate = new DateTime(2023, 9, 15, 3, 6, 8),
            EmployeeId = "Some employee",
            FeeId = feeId,
            ObjectId = Guid.Parse("a71717fb-cffd-4974-89d9-775a62d89399"),
            ShiftId = "Some Id",
            Payment = new CoreModel.PaymentModel()
            {
                Duration = 32541,
                Model = "Some model",
                PlateNumber = "Some plate number",
                Amount = 9000
            }
        };
        #endregion

        #region AddAsync
        [Fact, Order(1)]
        public async Task GivenValidRequest_WhenApiAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.PostAsJsonAsync($"/api/Fee/v1/fees", request);
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
        [Fact, Order(1)]
        public async Task GivenValidRequestAndNewFeeIdIsGenerated_WhenApiAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);
            request.FeeId = testFeeId;
            request.ObjectId = Guid.NewGuid();

            // Act
            var result = await HttpClient.PostAsJsonAsync($"/api/Fee/v1/fees", request);
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
        public async Task GivenRequestIsValidAndObjectIdAlreadyExists_WhenAddAsyncIsCalled_ThenReturnConflict()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);
            request.FeeId = Guid.NewGuid();

            // Act
            var result = await HttpClient.PostAsJsonAsync($"/api/Fee/v1/fees", request);
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var data = reports?["data"]?.AsObject();
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.Conflict);
            content.Should().NotBeEmpty();
            successful?.GetValue<bool>().Should().BeFalse();
            data.Should().BeNull();
        }

        [Fact, Order(2)]
        public async Task GivenRequestIsInValid_WhenAddAsyncIsCalled_ThenReturnBadRequest()
        {
            // Arrange
            CoreModel.FeeModel request = new CoreModel.FeeModel();
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.PostAsJsonAsync($"/api/Fee/v1/fees", request);
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().NotBeEmpty();
            successful!.GetValue<bool>().Should().BeFalse();
        }
        #endregion

        #region UpdateAsync
        [Fact, Order(3)]
        public async Task GivenValidRequest_WhenApiUpdateAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);
            request.Payment!.Amount = 10000;

            // Act
            var result = await HttpClient.PutAsJsonAsync($"/api/Fee/v1/fees/{feeId}", request);
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var data = reports?["data"]?.AsObject();
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeEmpty();
            successful?.GetValue<bool>().Should().BeTrue();
            data.Should().NotBeNull();
        }
        [Fact, Order(3)]
        public async Task GivenValidRequestAndFeeIdIsNotExists_WhenApiUpdateAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.PutAsJsonAsync($"/api/Fee/v1/fees/{Guid.NewGuid()}", request);
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
        [Fact, Order(3)]
        public async Task GivenRequestIsValidAndObjectIdAlreadyExists_WhenUpdateAsyncIsCalled_ThenReturnConflict()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.PutAsJsonAsync($"/api/Fee/v1/fees/{testFeeId}", request);
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var data = reports?["data"]?.AsObject();
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.Conflict);
            content.Should().NotBeEmpty();
            successful?.GetValue<bool>().Should().BeFalse();
            data.Should().BeNull();
        }

        [Fact, Order(3)]
        public async Task GivenRequestIsInValid_WhenUpdateAsyncIsCalled_ThenReturnBadRequest()
        {
            // Arrange
            CoreModel.FeeModel request = new CoreModel.FeeModel();
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.PutAsJsonAsync($"/api/Fee/v1/fees/{feeId}", request);
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().NotBeEmpty();
            successful!.GetValue<bool>().Should().BeFalse();
        }
        #endregion

        #region GetAllAsync
        [Fact, Order(4)]
        public async Task GivenValidRequest_WhenApiGetAllAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.GetAsync($"/api/Fee/v1/fees");
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var count = reports?["data"]?.AsArray().Count();
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeEmpty();
            successful?.GetValue<bool>().Should().BeTrue();
            count.Should().BeGreaterThanOrEqualTo(2);
        }
        #endregion

        #region GetByIdAsync
        [Fact, Order(4)]
        public async Task GivenValidRequest_WhenApiGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.GetAsync($"/api/Fee/v1/fees/{feeId}");
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var data = reports?["data"]?.AsObject();
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeEmpty();
            successful?.GetValue<bool>().Should().BeTrue();
            data.Should().NotBeNull();
        }
        #endregion

        #region RemoveAsync
        [Fact, Order(5)]
        public async Task GivenValidRequest_WhenApiRemoveAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.DeleteAsync($"/api/Fee/v1/fees/{feeId}");
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var data = reports?["data"]?.AsObject();
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeEmpty();
            successful?.GetValue<bool>().Should().BeTrue();
            data.Should().BeNull();
        }
        [Fact, Order(5)]
        public async Task GivenValidRequestAndNewFeeIdIsGenerated_WhenApiRemoveAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.DeleteAsync($"/api/Fee/v1/fees/{testFeeId}");
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var data = reports?["data"]?.AsObject();
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeEmpty();
            successful?.GetValue<bool>().Should().BeTrue();
            data.Should().BeNull();
        }
        [Fact, Order(5)]
        public async Task GivenValidRequestAndFeeIdIsNotExists_WhenApiRemoveAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.DeleteAsync($"/api/Fee/v1/fees/{Guid.NewGuid()}");
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
        #endregion
    }
}
