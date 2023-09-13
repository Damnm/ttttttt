using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Models.TimeBlockFees;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.TimeBlockFees
{
    public class TimeBlockFeeFormulaRepository : ITimeBlockFeeFormulaRepository
    {
        private readonly ILogger<TimeBlockFeeFormulaRepository> _logger;
        private readonly CoreDbContext _dbContext;

        public TimeBlockFeeFormulaRepository(ILogger<TimeBlockFeeFormulaRepository> logger, CoreDbContext dbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Task<IEnumerable<TimeBlockFeeFormulaModel>> GetAllAsync(Expression<Func<TimeBlockFeeFormulaModel, bool>>? expression = null)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetAllAsync)} method...");
                IEnumerable<TimeBlockFeeFormulaModel> result;

                if (expression != null)
                    result = _dbContext.TimeBlockFeeFormulas.AsNoTracking()
                        .Include(x => x.CustomVehicleType)
                        .Where(expression);
                else
                    result = _dbContext.TimeBlockFeeFormulas.AsNoTracking()
                        .Include(x => x.CustomVehicleType);

                return Task.FromResult(result);
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetAllAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public Task<TimeBlockFeeFormulaModel?> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");

                TimeBlockFeeFormulaModel? result = _dbContext.TimeBlockFeeFormulas.AsNoTracking()
                        .Include(x => x.CustomVehicleType)
                        .FirstOrDefault(x => x.Id == id);

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
