using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Models.ManualBarrierControl;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ManualBarrierControls
{
    public class ManualBarrierControlRepository : IManualBarrierControlRepository
    {
        #region Variables
        private readonly ILogger<ManualBarrierControlRepository> _logger;
        private readonly CoreDbContext _dbContext;
        #endregion
        #region Constructor
        public ManualBarrierControlRepository(ILogger<ManualBarrierControlRepository> logger, CoreDbContext dbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        #endregion
        #region AddAsync
        public async Task<ManualBarrierControlModel> AddAsync(ManualBarrierControlModel entity)
        {
            _logger.LogInformation($"Executing {nameof(AddAsync)} method...");
            try
            {
                var res = await _dbContext.ManualBarrierControls.AddAsync(entity);
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
        public Task<IEnumerable<ManualBarrierControlModel>> GetAllAsync(Expression<Func<ManualBarrierControlModel, bool>>? expression = null)
        {
            _logger.LogInformation($"Executing {nameof(GetAllAsync)} method...");
            try
            {
                if (expression == null)
                {
                    return Task.FromResult<IEnumerable<ManualBarrierControlModel>>(_dbContext.ManualBarrierControls.AsNoTracking());
                }

                return Task.FromResult<IEnumerable<ManualBarrierControlModel>>(_dbContext.ManualBarrierControls.AsNoTracking().Where(expression));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetAllAsync)} method. Detail: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        #endregion
        #region GetByIdAsync
        public Task<ManualBarrierControlModel?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");

            try
            {
                var manualbarriercontrol = _dbContext.ManualBarrierControls.AsNoTracking().FirstOrDefault(x => x.Id == id);
                return Task.FromResult(manualbarriercontrol);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetByIdAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        #endregion
        #region RemoveAsync
        public async Task RemoveAsync(ManualBarrierControlModel entity)
        {
            _logger.LogInformation($"Executing {nameof(RemoveAsync)} method...");
            try
            {
                _dbContext.ManualBarrierControls.Remove(entity);
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
        public async Task UpdateAsync(ManualBarrierControlModel entity)
        {
            _logger.LogInformation($"Executing {nameof(UpdateAsync)} method...");

            try
            {
                _dbContext.ManualBarrierControls.Update(entity);
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
