using AutoMapper;
using EPAY.ETC.Core.API.Core.Interfaces.Services.PrintLog;
using EPAY.ETC.Core.API.Core.Models.PrintLog;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.PrintLog;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.Extensions.Logging;

namespace EPAY.ETC.Core.API.Infrastructure.Services.PrintLog
{
    public class PrintLogService : IPrintLogService
    {

        #region Variables   -
        private readonly ILogger<PrintLogService> _logger;
        private readonly IPrintLogRepository _repository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public PrintLogService(ILogger<PrintLogService> logger, IPrintLogRepository vehicleRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        #endregion

        public async Task<ValidationResult<PrintLogModel>> AddAsync(PrintLogRequestModel input)
        {
            _logger.LogInformation($"Executing {nameof(AddAsync)} method...");
            try
            {
                var entity = _mapper.Map<PrintLogModel>(input);
                var result = await _repository.AddAsync(entity);
                return ValidationResult.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(AddAsync)} method. Error: {ex.Message}");
                throw;
            }
        }

        public async Task<ValidationResult<PrintLogModel>> GetByIdAsync(Guid id)
        {
            _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");
            try
            {
                var result = await _repository.GetByIdAsync(id);
                if (result == null)
                {
                    return ValidationResult.Failed<PrintLogModel>(null, new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });
                }

                return ValidationResult.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(GetByIdAsync)} method. Error: {ex.Message}");
                throw;
            }
        }

        public async Task<ValidationResult<PrintLogModel>> RemoveAsync(Guid id)
        {
            _logger.LogInformation($"Executing {nameof(RemoveAsync)} method...");
            try
            {
                var result = await _repository.GetByIdAsync(id);
                if (result == null)
                {
                    return ValidationResult.Failed<PrintLogModel>(null, new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });
                }

                await _repository.RemoveAsync(result);
                return ValidationResult.Success<PrintLogModel>(null!);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(RemoveAsync)} method. Error: {ex.Message}");
                throw;
            }
        }

        public async Task<ValidationResult<PrintLogModel>> UpdateAsync(Guid id, PrintLogRequestModel input)
        {
            _logger.LogInformation($"Executing {nameof(UpdateAsync)} method...");
            try
            {
                var oldRecord = await _repository.GetByIdAsync(id);
                if (oldRecord == null)
                {
                    return ValidationResult.Failed<PrintLogModel>(null, new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });
                }
                if (oldRecord.Id != id)
                {
                    return ValidationResult.Failed<PrintLogModel>(new List<ValidationError>()
                    {
                        new ValidationError("Giá trị đã có trên hệ thống", ValidationError.Conflict.Code)
                    });
                }

                _mapper.Map(input, oldRecord);
                oldRecord.Id = id;

                await _repository.UpdateAsync(oldRecord);
                return ValidationResult.Success(oldRecord);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(UpdateAsync)} method. Error: {ex.Message}");
                throw;
            }
        }

        public async Task<ValidationResult<string?>> PrintAsync(PrintRequestModel request)
        {
            _logger.LogInformation($"Executing {nameof(PrintAsync)} method...");
            try
            {
                var result = await _repository.PrintAsync(request);
                return ValidationResult.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(PrintAsync)} method. Error: {ex.Message}");
                throw;
            }
        }
    }
}
