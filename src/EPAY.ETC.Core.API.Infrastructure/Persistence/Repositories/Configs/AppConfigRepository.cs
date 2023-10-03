using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Models.Configs;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ETCCheckouts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.AppConfigs
{
    public class AppConfigRepository : IAppConfigRepository
    {
        private readonly ILogger<AppConfigRepository> _logger;
        private readonly CoreDbContext _dbContext;

        public AppConfigRepository(ILogger<AppConfigRepository> logger, CoreDbContext dbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Task<IEnumerable<AppConfigModel>> GetAllAsync(Expression<Func<AppConfigModel, bool>>? expression = null)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetAllAsync)} method...");

                IEnumerable<AppConfigModel> entities;

                if (expression != null)
                {
                    entities = _dbContext.AppConfigs.AsNoTracking().Where(expression);
                }
                else
                {
                    entities = _dbContext.AppConfigs.AsNoTracking();
                }

                return Task.FromResult(entities);
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetAllAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public Task<AppConfigModel?> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");

                AppConfigModel? entity = _dbContext.AppConfigs.AsNoTracking().FirstOrDefault(x => x.Id == id);

                return Task.FromResult(entity);
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetByIdAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
