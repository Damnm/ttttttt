using EPAY.ETC.Core.API.Core.Models.Payment;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using EPAY.ETC.Core.API.IntegrationTests.Common;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.Request;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using XUnitPriorityOrderer;

namespace EPAY.ETC.Core.API.IntegrationTests.Controllers.ETCCheckouts
{
    [TestCaseOrderer(CasePriorityOrderer.TypeName, CasePriorityOrderer.AssembyName)]
    public class ETCCheckoutControllerTests : IntegrationTestBase
    {
        #region Init test data
        private static Guid testETCCheckoutId = Guid.Parse("51B74325-BCA9-4775-B056-F5535935155E");
        private static Guid etcCheckoutId = Guid.Parse("893BF772-FE6D-4B76-94FF-3EE2A883365F");
        private static Guid testPaymentId = Guid.Parse("2D2B0083-FAB1-4814-93BA-4C60AC00378E");
        private static Guid paymentId = Guid.Parse("9698D267-36DE-4ACE-BABB-CF9BBD4D12B4");

        private ETCCheckoutAddUpdateRequestModel request = new ETCCheckoutAddUpdateRequestModel()
        {
            TransactionId = "06516250651216513521",
            PaymentId = paymentId,
            PlateNumber = "Some Plate",
            Amount = 7000,
            RFID = "Some RFID",
            ServiceProvider = ETCServiceProviderEnum.VETC,
            TransactionStatus = TransactionStatusEnum.CheckOut
        };
        #endregion

        #region Add test data
        [Fact, Order(0)]
        public async Task AddDataBeforeTest()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<CoreDbContext>();

            await _dbContext.Payments.AddRangeAsync(new List<PaymentModel>
            {
                new PaymentModel()
                {
                    Id =paymentId,
                    Amount = 10000,
                    Duration = 1800
                },
                new PaymentModel()
                {
                    Id =testPaymentId,
                    Amount = 10000,
                    Duration = 1800
                }
            });

            await _dbContext.SaveChangesAsync();
        }
        #endregion

        #region AddAsync
        [Fact, Order(1)]
        public async Task GivenValidRequest_WhenApiAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.PostAsJsonAsync($"/api/ETCCheckout/v1/etcCheckouts", request);
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var data = reports?["data"]?.AsObject();
            var id = reports?["data"]?["id"]?.AsValue();
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.Created);
            content.Should().NotBeEmpty();
            successful?.GetValue<bool>().Should().BeTrue();
            data.Should().NotBeNull();

            etcCheckoutId = id?.GetValue<Guid>() ?? Guid.NewGuid();
        }

        [Fact, Order(2)]
        public async Task GivenValidRequestAndNewETCCheckoutIdIsGenerated_WhenApiAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            request.PaymentId = testPaymentId;
            request.TransactionId = "2196810352468543205";

            // Act
            var result = await HttpClient.PostAsJsonAsync($"/api/ETCCheckout/v1/etcCheckouts", request);
            var content = await result.Content.ReadAsStringAsync();

            var reports = JsonNode.Parse(content);
            var data = reports?["data"]?.AsObject();
            var id = reports?["data"]?["id"]?.AsValue();
            var successful = reports?["succeeded"]?.AsValue();

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.Created);
            content.Should().NotBeEmpty();
            successful?.GetValue<bool>().Should().BeTrue();
            data.Should().NotBeNull();

            testETCCheckoutId = id?.GetValue<Guid>() ?? Guid.NewGuid();
        }

        [Fact, Order(3)]
        public async Task GivenRequestIsValidAndObjectIdAlreadyExists_WhenAddAsyncIsCalled_ThenReturnConflict()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.PostAsJsonAsync($"/api/ETCCheckout/v1/etcCheckouts", request);
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
        public async Task GivenRequestIsInValid_WhenAddAsyncIsCalled_ThenReturnBadRequest()
        {
            // Arrange
            ETCCheckoutAddUpdateRequestModel request = new ETCCheckoutAddUpdateRequestModel();
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.PostAsJsonAsync($"/api/ETCCheckout/v1/etcCheckouts", request);
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
        [Fact, Order(4)]
        public async Task GivenValidRequest_WhenApiUpdateAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);
            request.Amount = 10000;

            // Act
            var result = await HttpClient.PutAsJsonAsync($"/api/ETCCheckout/v1/etcCheckouts/{etcCheckoutId}", request);
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
        [Fact, Order(5)]
        public async Task GivenValidRequestAndETCCheckoutIdIsNotExists_WhenApiUpdateAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.PutAsJsonAsync($"/api/ETCCheckout/v1/etcCheckouts/{Guid.NewGuid()}", request);
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
        public async Task GivenRequestIsValidAndObjectIdAlreadyExists_WhenUpdateAsyncIsCalled_ThenReturnConflict()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.PutAsJsonAsync($"/api/ETCCheckout/v1/etcCheckouts/{testETCCheckoutId}", request);
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

        [Fact, Order(5)]
        public async Task GivenRequestIsInValid_WhenUpdateAsyncIsCalled_ThenReturnBadRequest()
        {
            // Arrange
            ETCCheckoutAddUpdateRequestModel request = new ETCCheckoutAddUpdateRequestModel();
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.PutAsJsonAsync($"/api/ETCCheckout/v1/etcCheckouts/{etcCheckoutId}", request);
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
        [Fact, Order(6)]
        public async Task GivenValidRequest_WhenApiGetAllAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.GetAsync($"/api/ETCCheckout/v1/etcCheckouts");
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
        [Fact, Order(6)]
        public async Task GivenValidRequest_WhenApiGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.GetAsync($"/api/ETCCheckout/v1/etcCheckouts/{etcCheckoutId}");
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
        [Fact, Order(7)]
        public async Task GivenValidRequest_WhenApiRemoveAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.DeleteAsync($"/api/ETCCheckout/v1/etcCheckouts/{etcCheckoutId}");
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
        [Fact, Order(7)]
        public async Task GivenValidRequestAndETCCheckoutIdIsNotExists_WhenApiRemoveAsyncIsCalled_ThenReturnNotFound()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            // Act
            var result = await HttpClient.DeleteAsync($"/api/ETCCheckout/v1/etcCheckouts/{Guid.NewGuid()}");
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

        #region RemoveTest
        [Fact, Order(13)]
        public async Task RemoveTestData()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<CoreDbContext>();

            var etcCheckouts = _dbContext.ETCCheckOuts.AsNoTracking().FirstOrDefault(x => x.Id == testETCCheckoutId);
            if (etcCheckouts != null)
                _dbContext.ETCCheckOuts.Remove(etcCheckouts);


            var paymentIds = new List<Guid>() { paymentId, testPaymentId };
            var payments = _dbContext.Payments.AsNoTracking().Where(x => paymentIds.Any(p => p == x.Id));
            if (payments.Any())
                _dbContext.Payments.RemoveRange(payments);

            await _dbContext.SaveChangesAsync();
        }
        #endregion
    }
}
