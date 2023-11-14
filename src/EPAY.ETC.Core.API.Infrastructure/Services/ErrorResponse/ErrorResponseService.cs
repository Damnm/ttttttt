using AutoMapper;
using EPAY.ETC.Core.API.Core.Interfaces.Services.ErrorResponse;
using EPAY.ETC.Core.API.Core.Models.ErrorResponse;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ErrorResponse;
using EPAY.ETC.Core.Models.Constants;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Linq.Expressions;
using System.Text.Json;

namespace EPAY.ETC.Core.API.Infrastructure.Services.ErrorResponse
{
    public class ErrorResponseService : IErrorResponseService
    {
        #region Variables   -
        private readonly ILogger<ErrorResponseService> _logger;
        private readonly IErrorResponseRepository _repository;
        private readonly IMapper _mapper;
        private readonly IDatabase _redisDB;
        #endregion

        #region Constructor
        public ErrorResponseService(ILogger<ErrorResponseService> logger, IErrorResponseRepository fusionRepository, IMapper mapper, IDatabase redisDB)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = fusionRepository ?? throw new ArgumentNullException(nameof(fusionRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _redisDB = redisDB ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ValidationResult<List<ErrorResponseModel>>> GetErrorResponseBySourceAync(string source)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetErrorResponseBySourceAync)} method...");

                // Get from redis
                var resultDB = await _redisDB.StringGetAsync(RedisConstant.ErrorResponseKey(source));
                if (!string.IsNullOrEmpty(resultDB))
                {
                    var jsonErrorResponse = JsonSerializer.Deserialize<List<ErrorResponseModel>>(resultDB!, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                    return ValidationResult.Success(jsonErrorResponse ?? new List<ErrorResponseModel>());
                }
                else
                {
                    Expression<Func<ErrorResponseModel, bool>> expression = s => source != null ? s.Source == source : true;
                    var result = await _repository.GetAllAsync(expression);

                    var jsonString = JsonSerializer.Serialize(result);

                    if (result != null)
                    {
                        await _redisDB.StringSetAsync(RedisConstant.ErrorResponseKey(source), jsonString, new TimeSpan(1, 0, 0, 0));
                    }

                    return ValidationResult.Success(result?.ToList() ?? new List<ErrorResponseModel>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetErrorResponseBySourceAync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        #endregion

    }
}

