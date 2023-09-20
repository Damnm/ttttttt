using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Models.Fees;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Fees
{
    public class FeeRepository : IFeeRepository
    {
        private readonly ILogger<FeeRepository> _logger;
        private readonly CoreDbContext _dbContext;

        public FeeRepository(ILogger<FeeRepository> logger, CoreDbContext dbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<FeeModel> AddAsync(FeeModel entity)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(AddAsync)} method...");

                await _dbContext.Fees.AddAsync(entity);
                await _dbContext.SaveChangesAsync();

                return entity;
            }
            catch(ETCEPAYCoreAPIException ex) {
                _logger.LogError($"An error occurred when calling {nameof(AddAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public Task<IEnumerable<FeeModel>> GetAllAsync(Expression<Func<FeeModel, bool>>? expression = null)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetAllAsync)} method...");

                IEnumerable<FeeModel> result;

                if (expression == null)
                    result = _dbContext.Fees.AsNoTracking();
                else
                    result = _dbContext.Fees.AsNoTracking().Where(expression);

                return Task.FromResult(result);
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetAllAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public Task<FeeModel?> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");

                return Task.FromResult(_dbContext.Fees.AsNoTracking().Where(x => x.Id == id).FirstOrDefault());
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetByIdAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<FeeModel?> GetByObjectIdAsync(Guid objectId)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetByObjectIdAsync)} method...");

                return  await _dbContext.Fees.AsNoTracking().Where(x => x.ObjectId == objectId).FirstOrDefaultAsync();
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetByObjectIdAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }


        public async Task RemoveAsync(FeeModel entity)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(RemoveAsync)} method...");

                _dbContext.Fees.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(RemoveAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task UpdateAsync(FeeModel entity)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(UpdateAsync)} method...");

                _dbContext.Fees.Update(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(UpdateAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
