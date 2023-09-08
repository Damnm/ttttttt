using EPAY.ETC.Core.API.Core.Models.FeeVehicleCategories;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.FeeVehicleCategories
{
    public class FeeVehicleCategoryRepository : IFeeVehicleCategoryRepository
    {
        private readonly ILogger<FeeVehicleCategoryRepository> _logger;
        private readonly CoreDbContext _dbContext;

        public FeeVehicleCategoryRepository(ILogger<FeeVehicleCategoryRepository> logger, CoreDbContext dbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Task<IEnumerable<FeeVehicleCategoryModel>> GetAllAsync(Expression<Func<FeeVehicleCategoryModel, bool>>? expression = null)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetAllAsync)} method...");
                IEnumerable<FeeVehicleCategoryModel> result;

                if (expression != null)
                    result = _dbContext.FeeVehicleCategories.AsNoTracking()
                        .Include(x => x.VehicleGroup)
                        .Include(x => x.CustomVehicleType)
                        .Include(x => x.FeeType)
                        .Include(x => x.VehicleCategory)
                        .Where(expression);
                else
                    result = _dbContext.FeeVehicleCategories.AsNoTracking()
                        .Include(x => x.VehicleGroup)
                        .Include(x => x.CustomVehicleType)
                        .Include(x => x.FeeType)
                        .Include(x => x.VehicleCategory);

                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetAllAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public Task<FeeVehicleCategoryModel?> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");

                FeeVehicleCategoryModel? result = _dbContext.FeeVehicleCategories.AsNoTracking()
                    .Include(x => x.VehicleGroup)
                    .Include(x => x.CustomVehicleType)
                    .Include(x => x.FeeType)
                    .Include(x => x.VehicleCategory)
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
