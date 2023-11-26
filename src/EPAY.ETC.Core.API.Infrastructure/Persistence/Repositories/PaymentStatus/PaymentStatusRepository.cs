using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Models.Shifts;
using EPAY.ETC.Core.API.Infrastructure.Common.Constants;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.Receipt.SessionReports;
using EPAY.ETC.Core.Models.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using PaymentStatusModel = EPAY.ETC.Core.API.Core.Models.PaymentStatus.PaymentStatusModel;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.PaymentStatus
{
    public class PaymentStatusRepository : IPaymentStatusRepository
    {

        #region Variables
        private readonly ILogger<PaymentStatusRepository> _logger;
        private readonly CoreDbContext _dbContext;
        #endregion
        #region Constructor
        public PaymentStatusRepository(ILogger<PaymentStatusRepository> logger, CoreDbContext dbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        #endregion
        public async Task<Core.Models.PaymentStatus.PaymentStatusModel> AddAsync(PaymentStatusModel entity)
        {
            _logger.LogInformation($"Executing {nameof(AddAsync)} method...");
            try
            {
                var res = await _dbContext.PaymentStatuses.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(AddAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public Task<IEnumerable<PaymentStatusModel>> GetAllAsync(Expression<Func<PaymentStatusModel, bool>>? expression = null)
        {

            _logger.LogInformation($"Executing {nameof(GetAllAsync)} method...");
            try
            {
                if (expression == null)
                {
                    return Task.FromResult<IEnumerable<PaymentStatusModel>>(_dbContext.PaymentStatuses.AsNoTracking());
                }

                return Task.FromResult<IEnumerable<PaymentStatusModel>>(_dbContext.PaymentStatuses.AsNoTracking().Where(expression).OrderByDescending(x => x.PaymentDate).Take(3));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetAllAsync)} method. Detail: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        public Task<IQueryable<PaymentStatusModel>> GetPaymentStatusHistoryAsync(Expression<Func<PaymentStatusModel, bool>>? expression)
        {

            _logger.LogInformation($"Executing {nameof(GetPaymentStatusHistoryAsync)} method...");
            try
            {
                if (expression != null)
                    return Task.FromResult(_dbContext.PaymentStatuses.AsNoTracking().Where(expression).OrderByDescending(x => x.PaymentDate).AsQueryable());

                return Task.FromResult(_dbContext.PaymentStatuses.AsNoTracking().OrderByDescending(x => x.PaymentDate).AsQueryable());
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetPaymentStatusHistoryAsync)} method. Detail: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public Task<IEnumerable<PaymentStatusModel>> GetAllWithNavigationAsync(LaneSessionReportRequestModel request)
        {

            _logger.LogInformation($"Executing {nameof(GetAllWithNavigationAsync)} method...");
            try
            {
                DateTime fromDate = request.FromDateTimeEpoch.FromUnixTime();
                DateTime toDate = request.ToDateTimeEpoch.FromUnixTime();

                MockShiftModel? shift = null;

                if (!string.IsNullOrEmpty(request.ShiftId) && AppConstant.ShiftValuePairs.ContainsKey(request.ShiftId))
                {
                    shift = AppConstant.ShiftValuePairs[request.ShiftId];
                }

#pragma warning disable CS8602 // Disable warning nullable field
                var result = _dbContext.PaymentStatuses
                    .Include(x => x.Payment)
                    .ThenInclude(x => x.Fee)
                    .AsNoTracking()
                    .Where(x =>
                        request != null
                        ? (
                            (!string.IsNullOrEmpty(request.LaneOutId) ? request.LaneOutId.Equals(x.Payment.Fee.LaneOutId) : true)
                            && (!string.IsNullOrEmpty(request.EmployeeId) ? request.EmployeeId.Equals(x.Payment.Fee.EmployeeId) : true)
                            && (!string.IsNullOrEmpty(request.ShiftId) ? shift != null : true)
                            && x.PaymentDate >= fromDate && x.PaymentDate <= toDate
                            && x.Status == PaymentStatusEnum.Paid
                        )
                        : true
                    ).AsEnumerable();
#pragma warning restore CS8602 // Disable warning nullable field

                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetAllWithNavigationAsync)} method. Detail: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public Task<PaymentStatusModel?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");

            try
            {
                return Task.FromResult(_dbContext.PaymentStatuses.AsNoTracking().FirstOrDefault(x => x.Id == id));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetByIdAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task RemoveAsync(PaymentStatusModel entity)
        {
            _logger.LogInformation($"Executing {nameof(RemoveAsync)} method...");
            try
            {
                _dbContext.PaymentStatuses.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(RemoveAsync)} method. Detail: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task UpdateAsync(PaymentStatusModel entity)
        {
            _logger.LogInformation($"Executing {nameof(UpdateAsync)} method...");

            try
            {
                _dbContext.PaymentStatuses.Update(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(UpdateAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
