using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Vehicle
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
            _logger.LogInformation($"Executing {nameof(AddAsync)} method...");
            try
            {
                var res = await _dbContext.Vehicles.AddAsync(entity);
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
        public Task<IEnumerable<VehicleModel>> GetAllAsync(Expression<Func<VehicleModel, bool>>? expression = null)
        {
            _logger.LogInformation($"Executing {nameof(GetAllAsync)} method...");
            try
            {
                if (expression == null)
                {
                    return Task.FromResult(_dbContext.Vehicles.AsNoTracking().AsEnumerable());
                }

                return Task.FromResult(_dbContext.Vehicles.AsNoTracking().Where(expression).AsEnumerable());
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetAllAsync)} method. Detail: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        #endregion
        #region GetByIdAsync
        public Task<VehicleModel?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");

            try
            {
                var vehicle = _dbContext.Vehicles.AsNoTracking().FirstOrDefault(x => x.Id == id);
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
        public async Task RemoveAsync(VehicleModel entity)
        {
            _logger.LogInformation($"Executing {nameof(RemoveAsync)} method...");
            try
            {
                _dbContext.Vehicles.Remove(entity);
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
        public async Task UpdateAsync(VehicleModel entity)
        {
            _logger.LogInformation($"Executing {nameof(UpdateAsync)} method...");

            try
            {
                _dbContext.Vehicles.Update(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(UpdateAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        #endregion

        #region GetVehicleWithInfringementAsync
        public Task<List<InfringedVehicleInfoModel>> GetVehicleWithInfringementAsync(Expression<Func<VehicleModel, bool>> expression, bool isRFID)
        {
            _logger.LogInformation($"Executing {nameof(GetVehicleWithInfringementAsync)} method...");
            try
            {
                if (isRFID)
                {
                    var rfidQuery =
                     from vehicle in _dbContext.Vehicles.Where(expression).AsNoTracking()
                     join InfringedVehicle in _dbContext.InfringedVehicles.AsNoTracking()
                     on vehicle.RFID equals InfringedVehicle.RFID into InfringedGroup
                     from item in InfringedGroup.DefaultIfEmpty()
                     select new InfringedVehicleInfoModel
                     {
                         InfringedPlateNumber = item.PlateNumber,
                         InfringedRFID = item.RFID,
                     };
                    return Task.FromResult(rfidQuery.ToList());

                }
                var plateNumberQuery =
                                      from vehicle in _dbContext.Vehicles.Where(expression).AsNoTracking()
                                      join InfringedVehicle in _dbContext.InfringedVehicles.AsNoTracking()
                                      on vehicle.PlateNumber equals InfringedVehicle.PlateNumber
                                      into InfringedGroup
                                      from item in InfringedGroup.DefaultIfEmpty()
                                      select new InfringedVehicleInfoModel
                                      {
                                          InfringedPlateNumber = item.PlateNumber,
                                          InfringedRFID = item.RFID,
                                      };
                return Task.FromResult(plateNumberQuery.ToList());


            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetVehicleWithInfringementAsync)} method. Detail: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        #endregion

    }
}
