using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Models.VehicleGroups;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.VehicleGroups
{
    public class VehicleGroupRepository : IVehicleGroupRepository
    {
        private readonly ILogger<VehicleGroupRepository> _logger;
        private readonly CoreDbContext _dbContext;

        public VehicleGroupRepository(ILogger<VehicleGroupRepository> logger, CoreDbContext dbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Task<IEnumerable<VehicleGroupModel>> GetAllAsync(Expression<Func<VehicleGroupModel, bool>>? expression = null)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetAllAsync)} method...");
                IEnumerable<VehicleGroupModel> result;

                if (expression != null)
                    result = _dbContext.VehicleGroups.AsNoTracking()
                        .Where(expression);
                else
                    result = _dbContext.VehicleGroups.AsNoTracking();

                return Task.FromResult(result);
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetAllAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public Task<VehicleGroupModel?> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");

                VehicleGroupModel? result = _dbContext.VehicleGroups.AsNoTracking()
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
