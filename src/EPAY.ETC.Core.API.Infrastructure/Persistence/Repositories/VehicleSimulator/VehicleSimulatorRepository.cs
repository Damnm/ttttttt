using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Models.Simulator;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Vehicle;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.VehicleSimulator
{
    public class VehicleSimulatorRepository : IVehicleSimulatorRepository
    {
        #region Variables
        private readonly ILogger<VehicleSimulatorRepository> _logger;
        private readonly CoreDbContext _dbContext;
        #endregion
        #region Constructor
        public VehicleSimulatorRepository(ILogger<VehicleSimulatorRepository> logger, CoreDbContext dbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        #endregion
        #region Addsync
        public async Task<VehicleSimulatorModel> AddAsync(VehicleSimulatorModel entity)
        {
            _logger.LogInformation($"Executing {nameof(AddAsync)} method...");
            try
            {
                var res = await _dbContext.VehicleSimulatorModels.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(AddAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        #endregion

        #region GetAsync
        public Task<IEnumerable<VehicleSimulatorModel>> GetAllAsync(Expression<Func<VehicleSimulatorModel, bool>>? expression = null)
        {
            _logger.LogInformation($"Executing {nameof(GetAllAsync)} method...");
            try
            {
                if (expression == null)
                {
                    return Task.FromResult(_dbContext.VehicleSimulatorModels.AsNoTracking().AsEnumerable());
                }

                return Task.FromResult(_dbContext.VehicleSimulatorModels.AsNoTracking().Where(expression).AsEnumerable());
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetAllAsync)} method. Detail: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        #endregion
        #region GetByIdAsync
        public Task<VehicleSimulatorModel?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");

            try
            {
                var vehicle = _dbContext.VehicleSimulatorModels.AsNoTracking().FirstOrDefault(x => x.Id == id);
                return Task.FromResult(vehicle);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetByIdAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        #endregion
        #region RemoveAsync
        public async Task RemoveAsync(VehicleSimulatorModel entity)
        {
            _logger.LogInformation($"Executing {nameof(RemoveAsync)} method...");
            try
            {
                _dbContext.VehicleSimulatorModels.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(RemoveAsync)} method. Detail: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        #endregion
        #region UpdateAsync
        public async Task UpdateAsync(VehicleSimulatorModel entity)
        {
            _logger.LogInformation($"Executing {nameof(UpdateAsync)} method...");

            try
            {
                _dbContext.VehicleSimulatorModels.Update(entity);
                await _dbContext.SaveChangesAsync();
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
