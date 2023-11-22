using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Models.TicketType;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.TicketType;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.TicketTypes
{
    public class TicketTypeRepository : ITicketTypeRepository
    {
        private readonly ILogger<TicketTypeRepository> _logger;
        private readonly CoreDbContext _dbContext;

        public TicketTypeRepository(ILogger<TicketTypeRepository> logger, CoreDbContext dbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Task<IEnumerable<TicketTypeModel>> GetAllAsync(Expression<Func<TicketTypeModel, bool>>? expression = null)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetAllAsync)} method...");

                IEnumerable<TicketTypeModel> entities;

                if (expression != null)
                {
                    entities = _dbContext.TicketTypes.AsNoTracking().Where(expression);
                }
                else
                {
                    entities = _dbContext.TicketTypes.AsNoTracking();
                }

                return Task.FromResult(entities);
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetAllAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public Task<TicketTypeModel?> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");

                TicketTypeModel? entity = _dbContext.TicketTypes.AsNoTracking().FirstOrDefault(x => x.Id == id);

                return Task.FromResult(entity);
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetByIdAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        public Task<Guid?> GetByCodeAsync(string code)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetByCodeAsync)} method...");

                TicketTypeModel? entity = _dbContext.TicketTypes.AsNoTracking().FirstOrDefault(x => x.Code == code);

                return Task.FromResult(entity?.Id);
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetByCodeAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
