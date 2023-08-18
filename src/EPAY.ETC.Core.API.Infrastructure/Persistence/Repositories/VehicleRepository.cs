using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Interfaces.Repositories;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

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

        #region Addsync
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
        #endregion

        #region GetAsync
        public async Task<IEnumerable<VehicleModel>> GetAllAsync()
        {
            _logger.LogInformation($"Executing {nameof(GetAllAsync)} method...");

            try
            {
                var vehicles = await _dbContext.Vehicles.ToListAsync();
                return vehicles;
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetAllAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<VehicleModel?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");

            try
            {
                var vehicle = await _dbContext.Vehicles.FindAsync(id);
                return vehicle;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetByIdAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        #endregion
        #region UpdateAsync
        public async Task<VehicleModel> UpdateAsync(VehicleModel entity)
        {
            _logger.LogInformation($"Executing {nameof(UpdateAsync)} method...");

            try
            {
                _dbContext.Vehicles.Update(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(UpdateAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        #endregion
    }
}
