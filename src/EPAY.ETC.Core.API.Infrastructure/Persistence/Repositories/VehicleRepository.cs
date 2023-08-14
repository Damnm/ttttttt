using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Interfaces.Repositories;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using Microsoft.Extensions.Logging;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        #region Variables
        private readonly ILogger<VehicleRepository> _logger;
        private readonly CoreDbContext _dbContext;
        #endregion

        #region Constructor
        public VehicleRepository(ILogger<VehicleRepository> logger, CoreDbContext dbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        #endregion

        public async Task<VehicleModel> AddAsync(VehicleModel entity)
        {
            _logger.LogInformation($"Executing EmployeeRepository {nameof(AddAsync)} method...");
            try
            {
                var res = await _dbContext.Vehicles.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling EmployeeRepository {nameof(AddAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
