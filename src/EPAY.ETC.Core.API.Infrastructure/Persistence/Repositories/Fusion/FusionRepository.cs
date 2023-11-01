using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Models.Fusion;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Fusion
{
    public class FusionRepository : IFusionRepository
    {
        #region Variables
        private readonly ILogger<FusionRepository> _logger;
        private readonly CoreDbContext _dbContext;
        #endregion
        #region Constructor
        public FusionRepository(ILogger<FusionRepository> logger, CoreDbContext dbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        #endregion
        #region AddAsync
        public async Task<FusionModel> AddAsync(FusionModel entity)
        {
            _logger.LogInformation($"Executing {nameof(AddAsync)} method...");
            try
            {
                var res = await _dbContext.Fusions.AddAsync(entity);
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
        public Task<IEnumerable<FusionModel>> GetAllAsync(Expression<Func<FusionModel, bool>>? expression = null)
        {
            _logger.LogInformation($"Executing {nameof(GetAllAsync)} method...");
            try
            {
                if (expression == null)
                {
                    return Task.FromResult<IEnumerable<FusionModel>>(_dbContext.Fusions.AsNoTracking());
                }

                return Task.FromResult<IEnumerable<FusionModel>>(_dbContext.Fusions.AsNoTracking().Where(expression));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetAllAsync)} method. Detail: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        #endregion
        #region GetByIdAsync
        public Task<FusionModel?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");

            try
            {
                var fusion = _dbContext.Fusions.AsNoTracking().FirstOrDefault(x => x.Id == id);
                return Task.FromResult(fusion);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetByIdAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        #endregion
        #region RemoveAsync
        public async Task RemoveAsync(FusionModel entity)
        {
            _logger.LogInformation($"Executing {nameof(RemoveAsync)} method...");
            try
            {
                _dbContext.Fusions.Remove(entity);
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
        public async Task UpdateAsync(FusionModel entity)
        {
            _logger.LogInformation($"Executing {nameof(UpdateAsync)} method...");

            try
            {
                _dbContext.Fusions.Update(entity);
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
