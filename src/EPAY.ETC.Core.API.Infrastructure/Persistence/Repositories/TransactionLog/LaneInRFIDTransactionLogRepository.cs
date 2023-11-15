using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Models.TransactionLog;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.TransactionLog
{
    public class LaneInRFIDTransactionLogRepository : ILaneInRFIDTransactionLogRepository
    {

        #region Variables
        private readonly ILogger<LaneInRFIDTransactionLogRepository> _logger;
        private readonly CoreDbContext _dbContext;
        #endregion
        #region Constructor
        public LaneInRFIDTransactionLogRepository(ILogger<LaneInRFIDTransactionLogRepository> logger, CoreDbContext dbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        #endregion
        public async Task<LaneInRFIDTransactionLog> AddAsync(LaneInRFIDTransactionLog entity)
        {
            _logger.LogInformation($"Executing {nameof(AddAsync)} method...");
            try
            {
                var res = await _dbContext.LaneInRFIDTransactionLogs.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(AddAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<LaneInRFIDTransactionLog>> GetAllAsync(Expression<Func<LaneInRFIDTransactionLog, bool>>? expression = null)
        {
            _logger.LogInformation($"Executing {nameof(GetAllAsync)} method...");
            try
            {
                if (expression == null)
                {
                    return await Task.FromResult(_dbContext.LaneInRFIDTransactionLogs.AsNoTracking().AsEnumerable());
                }

                return await Task.FromResult(_dbContext.LaneInRFIDTransactionLogs.AsNoTracking().Where(expression).AsEnumerable());
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetAllAsync)} method. Detail: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<LaneInRFIDTransactionLog?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");
            try
            {
                return await Task.FromResult(_dbContext.LaneInRFIDTransactionLogs.AsNoTracking().FirstOrDefault(x => x.Id == id));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetByIdAsync)} method. Detail: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task UpdateAsync(LaneInRFIDTransactionLog entity)
        {
            _logger.LogInformation($"Executing {nameof(UpdateAsync)} method...");

            try
            {
                _dbContext.LaneInRFIDTransactionLogs.Update(entity);
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
