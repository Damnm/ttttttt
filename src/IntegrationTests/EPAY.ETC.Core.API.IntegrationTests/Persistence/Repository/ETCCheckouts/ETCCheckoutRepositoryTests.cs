using EPAY.ETC.Core.API.Core.Models.ETCCheckOuts;
using EPAY.ETC.Core.API.Core.Models.Payment;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ETCCheckouts;
using EPAY.ETC.Core.API.IntegrationTests.Common;
using EPAY.ETC.Core.Models.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using XUnitPriorityOrderer;

namespace EPAY.ETC.Core.API.IntegrationTests.Persistence.Repository.ETCCheckouts
{
    /// <summary>
    /// Must be run all to pass this test
    /// </summary>
    [TestCaseOrderer(CasePriorityOrderer.TypeName, CasePriorityOrderer.AssembyName)]
    public class ETCCheckoutRepositoryTests : IntegrationTestBase
    {
        #region Init structure
        private IETCCheckoutRepository _etcCheckoutRepository;
        #endregion

        #region Init test data
        private static Guid paymentId = Guid.Parse("C34C8485-AD1A-4852-B395-593FCAD749D2");

        public ETCCheckoutDataModel etcCheckout = new ETCCheckoutDataModel()
        {
            Id = Guid.Parse("14157FCF-1427-44F8-BEE0-31B99A493C0B"),
            CreatedDate = new DateTime(2023, 9, 11),
            PaymentId = paymentId,
            ServiceProvider = ETCServiceProviderEnum.VDTC,
            TransactionId = "234556415843210682",
            TransactionStatus = TransactionStatusEnum.CheckOut,
            Amount = 15000,
            RFID = "023156456956875",
            PlateNumber = "29A23541"
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
                }
            });

            await _dbContext.SaveChangesAsync();
        }
        #endregion

        [Fact, Order(1)]
        public async Task GivenRequestIsValid_WhenAddAsyncIsCalled_ThenRecordAddedSuccessfulAndReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _etcCheckoutRepository = scope.ServiceProvider.GetRequiredService<IETCCheckoutRepository>();

            // Arrange

            // Act
            var result = await _etcCheckoutRepository.AddAsync(etcCheckout);
            var expected = await _etcCheckoutRepository.GetByIdAsync(etcCheckout.Id);

            // Assert
            result.Should().NotBeNull();
            expected.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Fact, Order(2)]
        public async Task GivenRequestIsValid_WhenGetAllAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _etcCheckoutRepository = scope.ServiceProvider.GetRequiredService<IETCCheckoutRepository>();

            // Arrange

            // Act
            var result = await _etcCheckoutRepository.GetAllAsync();

            // Assert
            result.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
        }

        [Fact, Order(3)]
        public async Task GivenRequestIsValidAndExpressionIsExists_WhenGetAllAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _etcCheckoutRepository = scope.ServiceProvider.GetRequiredService<IETCCheckoutRepository>();

            // Arrange
            Expression<Func<ETCCheckoutDataModel, bool>> expression = s => s.PaymentId == etcCheckout.PaymentId;

            // Act
            var result = await _etcCheckoutRepository.GetAllAsync(expression);

            // Assert
            result.Should().NotBeNull().And.HaveCount(1);
        }

        [Fact, Order(4)]
        public async Task GivenRequestIsValid_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _etcCheckoutRepository = scope.ServiceProvider.GetRequiredService<IETCCheckoutRepository>();

            // Arrange

            // Act
            var result = await _etcCheckoutRepository.GetByIdAsync(etcCheckout.Id);

            // Assert
            result.Should().NotBeNull().And.BeEquivalentTo(etcCheckout);
        }

        [Fact, Order(5)]
        public async Task GivenRequestIsValid_WhenUpdateAsyncIsCalled_ThenRecordUpdatedSuccessful()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _etcCheckoutRepository = scope.ServiceProvider.GetRequiredService<IETCCheckoutRepository>();

            // Arrange
            etcCheckout.RFID = "123549816546";
            etcCheckout.Amount = 19500;

            // Act
            await _etcCheckoutRepository.UpdateAsync(etcCheckout);
            var expected = await _etcCheckoutRepository.GetByIdAsync(etcCheckout.Id);

            // Assert
            expected.Should().NotBeNull();
            expected.Should().BeEquivalentTo(etcCheckout);
        }

        [Fact, Order(6)]
        public async Task GivenRequestIsValid_WhenRemoveAsyncIsCalled_ThenRecordRemovedSuccessful()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _etcCheckoutRepository = scope.ServiceProvider.GetRequiredService<IETCCheckoutRepository>();

            // Arrange
            etcCheckout.RFID = "123549816546";
            etcCheckout.Amount = 19500;

            // Act
            await _etcCheckoutRepository.RemoveAsync(etcCheckout);
            var expected = await _etcCheckoutRepository.GetByIdAsync(etcCheckout.Id);

            // Assert
            expected.Should().BeNull();
        }


        #region RemoveTest
        [Fact, Order(13)]
        public async Task RemoveTestData()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<CoreDbContext>();

            var paymentIds = new List<Guid>() { paymentId };
            var payments = _dbContext.Payments.AsNoTracking().Where(x => paymentIds.Any(p => p == x.Id));
            if (payments.Any())
                _dbContext.Payments.RemoveRange(payments);

            await _dbContext.SaveChangesAsync();
        }
        #endregion
    }
}
