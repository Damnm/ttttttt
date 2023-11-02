using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Models.ErrorResponse;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ErrorResponse
{
    public class ErrorResponseRepository : IErrorResponseRepository
    {
        private readonly ILogger<ErrorResponseRepository> _logger;
        private readonly CoreDbContext _dbContext;

        public ErrorResponseRepository(ILogger<ErrorResponseRepository> logger, CoreDbContext dbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Task<IEnumerable<ErrorResponseModel>> GetAllAsync(Expression<Func<ErrorResponseModel, bool>>? expression = null)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetAllAsync)} method...");

                IEnumerable<ErrorResponseModel> result;

                if (expression == null)
                    result = _dbContext.ErrorResponses.AsNoTracking();
                else
                    result = _dbContext.ErrorResponses.AsNoTracking().Where(expression);

                return Task.FromResult(result);
            }
            catch (ETCEPAYCoreAPIException ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetAllAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public Task<ErrorResponseModel?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
