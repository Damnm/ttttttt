using AutoMapper;
using EPAY.ETC.Core.API.Core.Interfaces.Services.ErrorResponse;
using EPAY.ETC.Core.API.Core.Models.ErrorResponse;
using EPAY.ETC.Core.API.Core.Models.ETCCheckOuts;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ErrorResponse;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Fusion;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Services.ErrorResponse
{
    public class ErrorResponseService : IErrorResponseService
    {
        #region Variables   -
        private readonly ILogger<ErrorResponseService> _logger;
        private readonly IErrorResponseRepository _repository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public ErrorResponseService(ILogger<ErrorResponseService> logger, IErrorResponseRepository fusionRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = fusionRepository ?? throw new ArgumentNullException(nameof(fusionRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

               public async Task<ValidationResult<IEnumerable<ErrorResponseModel>>> GetAllAsync(Expression<Func<ErrorResponseModel, bool>>? expressison = null)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetAllAsync)} method...");

                var result = await _repository.GetAllAsync(expressison);

                return ValidationResult.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetAllAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        #endregion

    }
}

