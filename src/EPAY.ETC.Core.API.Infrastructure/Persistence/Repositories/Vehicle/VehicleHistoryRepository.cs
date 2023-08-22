using EPAY.ETC.Core.API.Core.Models.Common;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Vehicle
{
    public class VehicleHistoryRepository: IVehicleHistoryRepository
    {
        private readonly ILogger<VehicleHistoryRepository> _logger;
        private readonly CoreDbContext _dbContext;

        public VehicleHistoryRepository(ILogger<VehicleHistoryRepository> logger, CoreDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<VehicleHistoryModel> AddAsync(VehicleHistoryModel entity)
        {
            _logger.LogInformation($"Executing {nameof(AddAsync)} method...");
            try
            {
                var res = await _dbContext.VehicleHistories.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(AddAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        public async Task AddRangeAsync(List<VehicleHistoryModel> entities)
        {
            _logger.LogInformation($"Executing {nameof(AddRangeAsync)} method...");
            try
            {
                await _dbContext.VehicleHistories.AddRangeAsync(entities);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(AddRangeAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        public async Task<IEnumerable<VehicleHistoryModel>> GetAllAsync(Expression<Func<VehicleHistoryModel, bool>>? expression = null)
        {
            _logger.LogInformation("Executing GetAllAsync method...");
            try
            {
                if (expression == null)
                {
                    return _dbContext.VehicleHistories.AsNoTracking();
                }
                else
                {
                    return _dbContext.VehicleHistories.AsNoTracking().Where(expression);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling GetAllAsync method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        public async Task<VehicleHistoryModel?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");
            try
            {
                return _dbContext.VehicleHistories.AsNoTracking().FirstOrDefault(s => s.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetByIdAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
