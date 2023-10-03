using EPAY.ETC.Core.API.Core.Interfaces.Services.ETCCheckouts;
using EPAY.ETC.Core.API.Core.Models.ETCCheckOuts;
using EPAY.ETC.Core.API.Core.Models.Payment;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using EPAY.ETC.Core.API.IntegrationTests.Common;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.Request;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using XUnitPriorityOrderer;

namespace EPAY.ETC.Core.API.IntegrationTests.Services.ETCCheckouts
{
    [TestCaseOrderer(CasePriorityOrderer.TypeName, CasePriorityOrderer.AssembyName)]
    public class ETCCheckoutServiceTests : IntegrationTestBase
    {
        private IETCCheckoutService _service;

        #region Init test data
        private static Guid testETCCheckOutId = Guid.Parse("97F81D1C-D34C-4117-9A66-BB075D84D997");
        private static Guid etcCheckOutId = Guid.Parse("76215302-11B7-4EDA-BE91-9743BFE1051F");
        private static Guid testPaymentId = Guid.Parse("CF3FAD2C-EAF5-4D24-8F5C-ED19DD833C6D");
        private static Guid paymentId = Guid.Parse("5AC73756-DAA9-4A11-8B43-102536D09471");

        private ETCCheckoutAddUpdateRequestModel request = new ETCCheckoutAddUpdateRequestModel()
        {
            TransactionId = "02134568618165211",
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
                    Id = paymentId,
                    Amount = 10000,
                    Duration = 1800
                },
                new PaymentModel()
                {
                    Id = testPaymentId,
                    Amount = 10000,
                    Duration = 1800
                }
            });

            await _dbContext.SaveChangesAsync();
        }
        #endregion

        #region AddAsync
        [Fact, Order(1)]
        public async Task GivenRequestIsValid_WhenAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _service = scope.ServiceProvider.GetRequiredService<IETCCheckoutService>();

            // Arrange

            // Act
            var result = await _service.AddAsync(request);

            etcCheckOutId = result.Data!.Id;
            var expected = await _service.GetByIdAsync(etcCheckOutId);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            expected.Should().NotBeNull();
            expected.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull().And.BeEquivalentTo(expected.Data);
        }

        [Fact, Order(2)]
        public async Task GivenRequestIsValidAndAddDataTest_WhenAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _service = scope.ServiceProvider.GetRequiredService<IETCCheckoutService>();

            // Arrange
            request.PaymentId = testPaymentId;
            request.TransactionId = "0686354968465160280";

            // Act
            var result = await _service.AddAsync(request);

            testETCCheckOutId = result.Data!.Id;
            var expected = await _service.GetByIdAsync(testETCCheckOutId);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            expected.Should().NotBeNull();
            expected.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull().And.BeEquivalentTo(expected.Data);

        }

        [Fact, Order(3)]
        public async Task GivenRequestIsValidAndObjectIdIsExists_WhenAddAsyncIsCalled_ThenReturnConflict()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _service = scope.ServiceProvider.GetRequiredService<IETCCheckoutService>();

            // Arrange

            // Act
            var result = await _service.AddAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Errors.Count(x => x.Code == StatusCodes.Status409Conflict).Should().BeGreaterThan(0);
        }
        #endregion

        #region UpdateAsync
        [Fact, Order(4)]
        public async Task GivenRequestIsValid_WhenUpdateAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _service = scope.ServiceProvider.GetRequiredService<IETCCheckoutService>();

            // Arrange
            request.Amount = 10000;

            // Act
            var result = await _service.UpdateAsync(etcCheckOutId, request);
            var expected = await _service.GetByIdAsync(etcCheckOutId);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            expected.Should().NotBeNull();
            expected.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull().And.BeEquivalentTo(expected.Data);
        }

        [Fact, Order(5)]
        public async Task GivenRequestIsValidAndObjectIdIsExists_WhenUpdateAsyncIsCalled_ThenReturnConflict()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _service = scope.ServiceProvider.GetRequiredService<IETCCheckoutService>();

            // Arrange

            // Act
            var result = await _service.UpdateAsync(testETCCheckOutId, request);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Errors.Count(x => x.Code == StatusCodes.Status409Conflict).Should().BeGreaterThan(0);
        }

        [Fact, Order(6)]
        public async Task GivenRequestIsValidAndETCCheckOutIdIsNotExists_WhenUpdateAsyncIsCalled_ThenReturnNotFound()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _service = scope.ServiceProvider.GetRequiredService<IETCCheckoutService>();

            // Arrange

            // Act
            var result = await _service.UpdateAsync(Guid.NewGuid(), request);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Errors.Count(x => x.Code == StatusCodes.Status404NotFound).Should().BeGreaterThan(0);
        }
        #endregion

        #region GetAllAsync
        [Fact, Order(7)]
        public async Task GivenRequestIsValid_WhenGetAllAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _service = scope.ServiceProvider.GetRequiredService<IETCCheckoutService>();

            // Arrange

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
        }

        [Fact, Order(8)]
        public async Task GivenRequestIsValidAndExpressionAlreadyExists_WhenGetAllAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _service = scope.ServiceProvider.GetRequiredService<IETCCheckoutService>();

            // Arrange

            // Act
            var result = await _service.GetAllAsync((Expression<Func<ETCCheckoutDataModel, bool>>)(s => s.Id == etcCheckOutId));

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull().And.HaveCount(1);
        }
        #endregion

        #region GetByIdAsync
        [Fact, Order(9)]
        public async Task GivenRequestIsValid_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _service = scope.ServiceProvider.GetRequiredService<IETCCheckoutService>();

            // Arrange

            // Act
            var result = await _service.GetByIdAsync(etcCheckOutId);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data?.Id.Should().Be(etcCheckOutId);
        }

        [Fact, Order(10)]
        public async Task GivenRequestIsValidAndETCCheckOutIdIsNotExists_WhenGetByIdAsyncIsCalled_ThenReturnNull()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _service = scope.ServiceProvider.GetRequiredService<IETCCheckoutService>();

            // Arrange

            // Act
            var result = await _service.GetByIdAsync(Guid.NewGuid());

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeNull();
        }
        #endregion

        #region RemoveAsync
        [Fact, Order(11)]
        public async Task GivenRequestIsValid_WhenRemoveAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _service = scope.ServiceProvider.GetRequiredService<IETCCheckoutService>();

            // Arrange

            // Act
            var result = await _service.RemoveAsync(etcCheckOutId);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeNull();
        }

        [Fact, Order(12)]
        public async Task GivenRequestIsValidAndETCCheckOutIdIsNotExists_WhenRemoveAsyncIsCalled_ThenReturnNull()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _service = scope.ServiceProvider.GetRequiredService<IETCCheckoutService>();

            // Arrange

            // Act
            var result = await _service.RemoveAsync(Guid.NewGuid());

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Data.Should().BeNull();
        }
        #endregion

        #region RemoveTest
        [Fact, Order(13)]
        public async Task RemoveTestData()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<CoreDbContext>();

            var etcCheckouts = _dbContext.ETCCheckOuts.AsNoTracking().FirstOrDefault(x => x.Id == testETCCheckOutId);
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
