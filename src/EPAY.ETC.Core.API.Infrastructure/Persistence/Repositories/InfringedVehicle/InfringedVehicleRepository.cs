using EPAY.ETC.Core.API.Core.Models.InfringeredVehicle;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ManualBarrierControls;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Vehicle;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
        #endregion
        #region GetVehicleWithInfringementAsync
        public Task<List<InfringedVehicleModel>> GetInfringedVehicleAsync(Expression<Func<InfringedVehicleModel, bool>>? expression = null)
        {
            _logger.LogInformation($"Executing {nameof(GetInfringedVehicleAsync)} method...");
            try
            {
                if (expression == null)
                {
                    return Task.FromResult(_dbContext.InfringedVehicles.AsNoTracking().ToList());
                }

                return Task.FromResult(_dbContext.InfringedVehicles.AsNoTracking().Where(expression).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetInfringedVehicleAsync)} method. Detail: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        #endregion
    }
}
