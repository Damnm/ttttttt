using EPAY.ETC.Core.API.Core.Models.TimeBlockFees;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.TimeBlockFees
{
    public class TimeBlockFeeRepository : ITimeBlockFeeRepository
    {
        private readonly ILogger<TimeBlockFeeRepository> _logger;
        private readonly CoreDbContext _dbContext;

        public TimeBlockFeeRepository(ILogger<TimeBlockFeeRepository> logger, CoreDbContext dbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Task<IEnumerable<TimeBlockFeeModel>> GetAllAsync(Expression<Func<TimeBlockFeeModel, bool>>? expression = null)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetAllAsync)} method...");
                IEnumerable<TimeBlockFeeModel> result;

                if (expression != null)
                    result = _dbContext.TimeBlockFees.AsNoTracking()
                        .Include(x => x.CustomVehicleType)
                        .Where(expression);
                else
                    result = _dbContext.TimeBlockFees.AsNoTracking()
                        .Include(x => x.CustomVehicleType);

                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetAllAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public Task<TimeBlockFeeModel?> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");

                TimeBlockFeeModel? result = _dbContext.TimeBlockFees.AsNoTracking()
                        .Include(x => x.CustomVehicleType)
                        .FirstOrDefault(x => x.Id == id);

                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetByIdAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
