using EPAY.ETC.Core.API.Core.Models.VehicleCategories;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.VehicleCategories
{
    public class VehicleCategoryRepository : IVehicleCategoryRepository
    {
        private readonly ILogger<VehicleCategoryRepository> _logger;
        private readonly CoreDbContext _dbContext;

        public VehicleCategoryRepository(ILogger<VehicleCategoryRepository> logger, CoreDbContext dbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Task<IEnumerable<VehicleCategoryModel>> GetAllAsync(Expression<Func<VehicleCategoryModel, bool>>? expression = null)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetAllAsync)} method...");
                IEnumerable<VehicleCategoryModel> result;

                if (expression != null)
                    result = _dbContext.VehicleCategories.AsNoTracking()
                        .Where(expression);
                else
                    result = _dbContext.VehicleCategories.AsNoTracking();

                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetAllAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public Task<VehicleCategoryModel?> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");

                VehicleCategoryModel? result = _dbContext.VehicleCategories.AsNoTracking()
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
