using EPAY.ETC.Core.API.Core.Models.Fees;
using EPAY.ETC.Core.API.Core.Models.Payment;
using EPAY.ETC.Core.API.Core.Models.PaymentStatus;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.PaymentStatus;
using EPAY.ETC.Core.API.IntegrationTests.Common;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.Receipt.SessionReports;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using XUnitPriorityOrderer;

namespace EPAY.ETC.Core.API.IntegrationTests.Persistence.Repository.PaymentStatuss
{
    /// <summary>
    /// Must be run all to pass this test
    /// </summary>
    [TestCaseOrderer(CasePriorityOrderer.TypeName, CasePriorityOrderer.AssembyName)]
    public class PaymentStatusRepositoryTests : IntegrationTestBase
    {
        #region Init structure
        private IPaymentStatusRepository _paymentStatusRepository;
        #endregion

        #region Init test data
        private static Guid feeId = Guid.Parse("0F8448DF-270C-49B7-A0E1-D0E7776B38E0");
        private static Guid paymentId = Guid.Parse("02683538-F587-4D47-BB3D-0F74A62F11EE");
        private static Guid paymentStatusId = Guid.Parse("65D60A5F-A254-48C9-9AAC-27D431F7D20B");
        private PaymentStatusModel paymentStatus = new PaymentStatusModel()
        {
            PaymentId = paymentId,
            Amount = 10000,
            Currency = "VND",
            PaymentMethod = PaymentMethodEnum.Card,
            Status = PaymentStatusEnum.Paid,
            PaymentDate = new DateTime(2023, 9, 11, 15, 13, 39),
            Id = paymentStatusId,
            CreatedDate = new DateTime(2023, 9, 11),
            TransactionId = "254652068068420856584"
        };
        #endregion

        #region Add test data
        [Fact, Order(0)]
        public async Task AddDataBeforeTest()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<CoreDbContext>();

            await _dbContext.Fees.AddRangeAsync(new List<FeeModel>
            {
                new FeeModel()
                {
                    Id = feeId,
                    Amount = 10000,
                    Duration = 1800,
                    ConfidenceScore = (float)0.95,
                    ObjectId = Guid.NewGuid(),
                    LaneOutDate = new DateTime(2023, 9, 11, 15, 13, 39),
                }
            });

            await _dbContext.Payments.AddRangeAsync(new List<PaymentModel>
            {
                new PaymentModel()
                {
                    Id = paymentId,
                    Amount = 10000,
                    Duration = 1800,
                    FeeId = feeId
                }
            });

            await _dbContext.PaymentStatuses.AddRangeAsync(new List<PaymentStatusModel>
            {
                paymentStatus
            });

            await _dbContext.SaveChangesAsync();
        }
        #endregion

        #region GetAllWithNavigationAsync
        [Fact, Order(1)]
        public async Task GivenRequestIsValid_WhenGetAllWithNavigationAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _paymentStatusRepository = scope.ServiceProvider.GetRequiredService<IPaymentStatusRepository>();

            // Arrange
            SessionReportRequestModel request = new SessionReportRequestModel()
            {
                FromDate = new DateTime(2023, 9, 11),
                ToDate = new DateTime(2023, 9, 11, 23, 59, 59),
            };

            // Act
            var result = await _paymentStatusRepository.GetAllWithNavigationAsync(request);

            // Assert
            result.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
        }
        #endregion

        #region Remove test data
        [Fact, Order(10)]
        public async Task RemoveDataBeforeTest()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<CoreDbContext>();

            var fees = _dbContext.Fees.AsNoTracking().Where(x => x.Id == feeId);
            if (fees != null)
            {
                _dbContext.Fees.RemoveRange(fees);
            }

            var payments = _dbContext.Payments.AsNoTracking().Where(x => x.Id == paymentId);
            if (payments != null)
            {
                _dbContext.Payments.RemoveRange(payments);
            }

            var paymentStatuses = _dbContext.PaymentStatuses.AsNoTracking().Where(x => x.Id == paymentStatusId);
            if (paymentStatuses != null)
            {
                _dbContext.PaymentStatuses.RemoveRange(paymentStatuses);
            }

            await _dbContext.SaveChangesAsync();
        }
        #endregion

    }
}
