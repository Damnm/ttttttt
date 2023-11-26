using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.Fees.PaidVehicleHistory;
using EPAY.ETC.Core.Models.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using PaymentModel = EPAY.ETC.Core.API.Core.Models.Payment.PaymentModel;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Payment
{
    public class PaymentRepository : IPaymentRepository
    {

        #region Variables
        private readonly ILogger<PaymentRepository> _logger;
        private readonly CoreDbContext _dbContext;
        #endregion

        #region Constructor
        public PaymentRepository(ILogger<PaymentRepository> logger, CoreDbContext dbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        #endregion

        public async Task<PaymentModel> AddAsync(PaymentModel entity)
        {
            _logger.LogInformation($"Executing {nameof(AddAsync)} method...");
            try
            {
                var res = await _dbContext.Payments.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(AddAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public Task<IEnumerable<PaymentModel>> GetAllAsync(Expression<Func<PaymentModel, bool>>? expression = null)
        {

            _logger.LogInformation($"Executing {nameof(GetAllAsync)} method...");
            try
            {
                if (expression == null)
                {
                    return Task.FromResult<IEnumerable<PaymentModel>>(_dbContext.Payments.AsNoTracking());
                }

                return Task.FromResult<IEnumerable<PaymentModel>>(_dbContext.Payments.AsNoTracking().Where(expression));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetAllAsync)} method. Detail: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public Task<PaymentModel?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");

            try
            {
                return Task.FromResult(_dbContext.Payments.AsNoTracking().FirstOrDefault(x => x.Id == id));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetByIdAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }



        public async Task RemoveAsync(PaymentModel entity)
        {
            _logger.LogInformation($"Executing {nameof(RemoveAsync)} method...");
            try
            {
                _dbContext.Payments.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(RemoveAsync)} method. Detail: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task UpdateAsync(PaymentModel entity)
        {
            _logger.LogInformation($"Executing {nameof(UpdateAsync)} method...");

            try
            {
                _dbContext.Payments.Update(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(UpdateAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public Task<List<PaidVehicleHistoryModel>?> GetPaidVehicleHistoryAsync(string? laneId = "1")
        {
            _logger.LogInformation($"Executing {nameof(GetPaidVehicleHistoryAsync)} method...");

            try
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                var result = _dbContext.PaymentStatuses.OrderByDescending(k => k.PaymentDate)
                   .Include(x => x.Payment)
                   .Include(x => x.Payment.Fee)
                   .Include(x => x.Payment.CustomVehicleType)
                   .AsNoTracking()
                   .Where(x => x.Status == PaymentStatusEnum.Paid && x.Payment.Fee.LaneOutId.Equals(laneId)).Take(3)
                   .Select(x => new PaidVehicleHistoryModel()
                   {
                       PlateNumber = x.Payment.PlateNumber,
                       RFID = x.Payment.RFID,
                       PaymentMethod = x.PaymentMethod,
                       PaidDateTimeEpoch = x.PaymentDate.ConvertToTimeZone(DateTimeKind.Utc, "SE Asia Standard Time").ToUnixTime(),
                       LaneinDateTimeEpoch = x.Payment.Fee.LaneInEpoch,
                       LaneoutDateTimeEpoch = x.Payment.Fee.LaneOutEpoch,
                       CustomVehicleType = x.Payment.CustomVehicleType.Name,
                       LaneinVehiclePhotoUrl = x.Payment.Fee.LaneInVehiclePhotoUrl,
                       LaneoutVehiclePhotoUrl = x.Payment.Fee.LaneOutVehiclePhotoUrl,
                   });
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                return Task.FromResult(result?.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetPaidVehicleHistoryAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
