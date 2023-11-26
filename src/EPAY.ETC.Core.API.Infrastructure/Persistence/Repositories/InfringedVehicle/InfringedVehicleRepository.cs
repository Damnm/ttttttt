using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Models.InfringeredVehicle;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.InfringedVehicle
{
    public class InfringedVehicleRepository: IInfringedVehicleRepository
    {
        #region Variables
        private readonly ILogger<InfringedVehicleRepository> _logger;
        private readonly CoreDbContext _dbContext;
        #endregion

        #region Constructor
        public InfringedVehicleRepository(ILogger<InfringedVehicleRepository> logger, CoreDbContext dbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Task<IEnumerable<InfringedVehicleModel>> GetAllAsync(Expression<Func<InfringedVehicleModel, bool>>? expression = null)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetAllAsync)} method...");

                IEnumerable<InfringedVehicleModel> entities;

                if (expression != null)
                {
                    entities = _dbContext.InfringedVehicles.AsNoTracking().Where(expression);
                }
                else
                {
                    entities = _dbContext.InfringedVehicles.AsNoTracking();
                }

                return Task.FromResult(entities);
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetAllAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public Task<InfringedVehicleModel?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
