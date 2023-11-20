using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Models.TransactionLog;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.TransactionLog
{
    public class LaneInCameraTransactionLogRepository : ILaneInCameraTransactionLogRepository
    {

        #region Variables
        private readonly ILogger<LaneInCameraTransactionLogRepository> _logger;
        private readonly CoreDbContext _dbContext;
        #endregion
        #region Constructor
        public LaneInCameraTransactionLogRepository(ILogger<LaneInCameraTransactionLogRepository> logger, CoreDbContext dbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        #endregion
        public async Task<LaneInCameraTransactionLog> AddAsync(LaneInCameraTransactionLog entity)
        {
            _logger.LogInformation($"Executing {nameof(AddAsync)} method...");
            try
            {
                var res = await _dbContext.LaneInCameraTransactionLogs.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(AddAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<LaneInCameraTransactionLog>> GetAllAsync(Expression<Func<LaneInCameraTransactionLog, bool>>? expression = null)
        {
            _logger.LogInformation($"Executing {nameof(GetAllAsync)} method...");
            try
            {
                if (expression == null)
                {
                    return await Task.FromResult(_dbContext.LaneInCameraTransactionLogs.AsNoTracking().AsEnumerable());
                }

                return await Task.FromResult(_dbContext.LaneInCameraTransactionLogs.AsNoTracking().Where(expression).AsEnumerable());
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetAllAsync)} method. Detail: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public Task<LaneInCameraTransactionLog?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(LaneInCameraTransactionLog entity)
        {
            _logger.LogInformation($"Executing {nameof(UpdateAsync)} method...");

            try
            {
                _dbContext.LaneInCameraTransactionLogs.Update(entity);
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
