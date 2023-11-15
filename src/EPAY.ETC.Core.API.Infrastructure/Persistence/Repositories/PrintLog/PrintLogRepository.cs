using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Models.PrintLog;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.PrintLog
{
    public class PrintLogRepository : IPrintLogRepository
    {
        #region Variables
        private readonly ILogger<PrintLogRepository> _logger;
        private readonly CoreDbContext _dbContext;
        #endregion
        #region Constructor
        public PrintLogRepository(ILogger<PrintLogRepository> logger, CoreDbContext dbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        #endregion
        #region Addsync
        public async Task<PrintLogModel> AddAsync(PrintLogModel entity)
        {
            _logger.LogInformation($"Executing {nameof(AddAsync)} method...");
            try
            {
                var res = await _dbContext.PrintLogs.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(AddAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        #endregion
        #region GetAsync
        public Task<IEnumerable<PrintLogModel>> GetAllAsync(Expression<Func<PrintLogModel, bool>>? expression = null)
        {
            _logger.LogInformation($"Executing {nameof(GetAllAsync)} method...");
            try
            {
                if (expression == null)
                {
                    return Task.FromResult(_dbContext.PrintLogs.AsNoTracking().AsEnumerable());
                }

                return Task.FromResult(_dbContext.PrintLogs.AsNoTracking().Where(expression).AsEnumerable());
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetAllAsync)} method. Detail: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        #endregion
        #region GetByIdAsync
        public Task<PrintLogModel?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");

            try
            {
                var vehicle = _dbContext.PrintLogs.AsNoTracking().FirstOrDefault(x => x.Id == id);
                return Task.FromResult(vehicle);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetByIdAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        #endregion
        #region RemoveAsync
        public async Task RemoveAsync(PrintLogModel entity)
        {
            _logger.LogInformation($"Executing {nameof(RemoveAsync)} method...");
            try
            {
                _dbContext.PrintLogs.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(RemoveAsync)} method. Detail: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        #endregion
        #region UpdateAsync
        public async Task UpdateAsync(PrintLogModel entity)
        {
            _logger.LogInformation($"Executing {nameof(UpdateAsync)} method...");

            try
            {
                _dbContext.PrintLogs.Update(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(UpdateAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        #endregion

    }
}
