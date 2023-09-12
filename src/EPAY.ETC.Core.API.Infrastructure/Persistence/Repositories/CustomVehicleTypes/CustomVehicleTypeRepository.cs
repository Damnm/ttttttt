using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Models.CustomVehicleTypes;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.CustomVehicleTypes
{
    public class CustomVehicleTypeRepository : ICustomVehicleTypeRepository
    {
        private readonly ILogger<CustomVehicleTypeRepository> _logger;
        private readonly CoreDbContext _dbContext;

        public CustomVehicleTypeRepository(ILogger<CustomVehicleTypeRepository> logger, CoreDbContext dbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public Task<IEnumerable<CustomVehicleTypeModel>> GetAllAsync(Expression<Func<CustomVehicleTypeModel, bool>>? expression = null)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetAllAsync)} method...");

                IEnumerable<CustomVehicleTypeModel> result;

                if (expression != null)
                    result = _dbContext.CustomVehicleTypes.AsNoTracking().Where(expression);
                else
                    result = _dbContext.CustomVehicleTypes.AsNoTracking();

                return Task.FromResult(result);
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetAllAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public Task<CustomVehicleTypeModel?> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");

                CustomVehicleTypeModel? result = _dbContext.CustomVehicleTypes.AsNoTracking().FirstOrDefault(x => x.Id == id);

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
