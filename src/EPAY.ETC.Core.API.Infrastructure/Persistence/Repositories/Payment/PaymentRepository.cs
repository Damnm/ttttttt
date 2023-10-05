using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Utils;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using EPAY.ETC.Core.Models.Fees.PaidVehicleHistory;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
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

        public async Task<PaymentModel?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");

            try
            {
                 return _dbContext.Payments.AsNoTracking().FirstOrDefault(x => x.Id == id);
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

        public async Task<List<PaidVehicleHistoryModel>?> GetPaidVehicleHistoryAsync()
        {
            _logger.LogInformation($"Executing {nameof(GetPaidVehicleHistoryAsync)} method...");

            try
            {
                var result = _dbContext.PaymentStatuses
                   .Include(x => x.Payment)
                   .Include(x => x.Payment.Fee)
                   .Include(x => x.Payment.CustomVehicleType)
                   .AsNoTracking()
                   .Where(x => x.Status == Models.Enums.PaymentStatusEnum.Paid).OrderByDescending(x=> x.PaymentDate).Take(3)
                   .Select(x => new PaidVehicleHistoryModel()
                   {
                        PlateNumber = x.Payment.PlateNumber,
                        RFID = x.Payment.RFID,
                        PaymentMethod = x.PaymentMethod,
                        PaidDateTimeEpoch = x.PaymentDate.ToUnixTime(),
                        LaneinDateTimeEpoch = x.Payment.Fee.LaneInEpoch,
                        LaneoutDateTimeEpoch = x.Payment.Fee.LaneOutEpoch,
                        CustomVehicleType = x.Payment.CustomVehicleType.Name,
                        LaneinVehiclePhotoUrl = x.Payment.Fee.LaneInVehiclePhotoUrl,
                        LaneoutVehiclePhotoUrl = x.Payment.Fee.LaneOutVehiclePhotoUrl,
                   });
               
                return result?.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetPaidVehicleHistoryAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
