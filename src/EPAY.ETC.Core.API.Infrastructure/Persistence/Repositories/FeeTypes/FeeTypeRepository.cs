using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Models.FeeTypes;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.FeeTypes
{
    public class FeeTypeRepository : IFeeTypeRepository
    {
        private readonly ILogger<FeeTypeRepository> _logger;
        private readonly CoreDbContext _dbContext;

        public FeeTypeRepository(ILogger<FeeTypeRepository> logger, CoreDbContext dbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Task<IEnumerable<FeeTypeModel>> GetAllAsync(Expression<Func<FeeTypeModel, bool>>? expression = null)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetAllAsync)} method...");
                IEnumerable<FeeTypeModel> result;

                if (expression != null)
                    result = _dbContext.FeeTypes.AsNoTracking().Where(expression);
                else
                    result = _dbContext.FeeTypes.AsNoTracking();

                return Task.FromResult(result);
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetAllAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public Task<FeeTypeModel?> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");

                FeeTypeModel? result = _dbContext.FeeTypes.AsNoTracking().FirstOrDefault(x => x.Id == id);

                return Task.FromResult(result);
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetByIdAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
