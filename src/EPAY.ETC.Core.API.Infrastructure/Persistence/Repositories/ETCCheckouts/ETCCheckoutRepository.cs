using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Models.ETCCheckOuts;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using EPAY.ETC.Core.Models.Request;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using ETCCheckoutFilterResultDto = EPAY.ETC.Core.API.Core.DtoModels.ETCCheckOuts.ETCCheckoutFilterResultDto;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ETCCheckouts
{
    public class ETCCheckoutRepository : IETCCheckoutRepository
    {
        private readonly ILogger<ETCCheckoutRepository> _logger;
        private readonly CoreDbContext _dbContext;

        public ETCCheckoutRepository(ILogger<ETCCheckoutRepository> logger, CoreDbContext dbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<ETCCheckoutDataModel> AddAsync(ETCCheckoutDataModel entity)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(AddAsync)} method...");

                await _dbContext.ETCCheckOuts.AddAsync(entity);
                await _dbContext.SaveChangesAsync();

                return entity;
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(AddAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public Task<IEnumerable<ETCCheckoutDataModel>> GetAllAsync(Expression<Func<ETCCheckoutDataModel, bool>>? expression = null)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetAllAsync)} method...");

                IEnumerable<ETCCheckoutDataModel> entities;

                if (expression != null)
                {
                    entities = _dbContext.ETCCheckOuts.AsNoTracking().Where(expression);
                }
                else
                {
                    entities = _dbContext.ETCCheckOuts.AsNoTracking();
                }

                return Task.FromResult(entities);
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetAllAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public Task<ETCCheckoutFilterResultDto> GetAllByConditionAsync(ETCCheckoutFilterModel? filter = null)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetAllByConditionAsync)} method...");

                ETCCheckoutFilterResultDto result = new ETCCheckoutFilterResultDto();

                var entities = _dbContext.ETCCheckOuts.AsNoTracking().Include(x => x.Payment).Include(x => x.Payment.Fee).AsQueryable();

                if (filter != null)
                {
                    entities = entities.Where(s => (filter.PaymentId != null && filter.PaymentId != Guid.Empty ? s.PaymentId == filter.PaymentId : true)
                        && (filter.ServiceProvider != null ? s.ServiceProvider == filter.ServiceProvider : true)
                        && (filter.TransactionStatus != null ? s.TransactionStatus == filter.TransactionStatus : true)
                        && (filter.Amount != null ? s.Amount == filter.Amount : true)
                        && (!string.IsNullOrEmpty(filter.TransactionId) ? s.TransactionId.Contains(filter.TransactionId) : true)
                        && (!string.IsNullOrEmpty(filter.RFID) ? !string.IsNullOrEmpty(s.RFID) && s.RFID.Contains(filter.RFID) : true)
                        && (!string.IsNullOrEmpty(filter.PlateNumber) ? !string.IsNullOrEmpty(s.PlateNumber) && s.PlateNumber.Contains(filter.PlateNumber) : true)
                    );

                    result.TotalItems = entities.Count();

                    if (filter.Take > 0)
                    {
                        entities = entities.Skip(filter.Skip).Take(filter.Take);
                    }
                }
                else
                    result.TotalItems = entities.Count();

                result.Items = entities;

                return Task.FromResult(result);
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetAllByConditionAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public Task<ETCCheckoutDataModel?> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");

                ETCCheckoutDataModel? entity = _dbContext.ETCCheckOuts.AsNoTracking().FirstOrDefault(x => x.Id == id);

                return Task.FromResult(entity);
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetByIdAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task RemoveAsync(ETCCheckoutDataModel entity)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(RemoveAsync)} method...");

                _dbContext.ETCCheckOuts.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(RemoveAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task UpdateAsync(ETCCheckoutDataModel entity)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(UpdateAsync)} method...");

                _dbContext.ETCCheckOuts.Update(entity);
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
