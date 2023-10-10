using AutoMapper;
using EPAY.ETC.Core.API.Core.Interfaces.Services.ManualBarrierControls;
using EPAY.ETC.Core.API.Core.Models.ManualBarrierControl;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ManualBarrierControls;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Services.ManualBarrierControls
{
    public class ManualBarrierControlsService : IManualBarrierControlsService
    {
        #region Variables
        private readonly ILogger<ManualBarrierControlsService> _logger;
        private readonly IManualBarrierControlRepository _repository;
        private readonly IMapper _mapper;
        #endregion
        #region Constructor
        public ManualBarrierControlsService(ILogger<ManualBarrierControlsService> logger, IManualBarrierControlRepository manualBarrierControlsRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = manualBarrierControlsRepository ?? throw new ArgumentNullException(nameof(manualBarrierControlsRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        #endregion
        #region AddAsync
        public async Task<ValidationResult<ManualBarrierControlModel>> AddAsync(ManualBarrierControlAddOrUpdateRequestModel input)
        {
            _logger.LogInformation($"Executing {nameof(AddAsync)} method...");
            try
            {
                var existingRecords = await GetExistingRecordAsync(input);
                if (existingRecords)
                {
                    return ValidationResult.Failed<ManualBarrierControlModel>(new List<ValidationError>()
                    {
                        new ValidationError("Gía trị đã có trên hệ thống", ValidationError.Conflict.Code)
                    });
                }
                var entity = _mapper.Map<ManualBarrierControlModel>(input);

                var result = await _repository.AddAsync(entity);
                return ValidationResult.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(AddAsync)} method. Error: {ex.Message}");
                throw;
            }
        }
        #endregion
        #region GetIdAsync
        public async Task<ValidationResult<ManualBarrierControlModel>> GetByIdAsync(Guid id)
        {
            _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");
            try
            {
                var result = await _repository.GetByIdAsync(id);
                if (result == null)
                {
                    return ValidationResult.Failed<ManualBarrierControlModel>(null, new List<ValidationError>()
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
        #endregion
        #region RemoveAsync
        public async Task<ValidationResult<ManualBarrierControlModel>> RemoveAsync(Guid id)
        {
            _logger.LogInformation($"Executing {nameof(RemoveAsync)} method...");
            try
            {
                var result = await _repository.GetByIdAsync(id);
                if (result == null)
                {
                    return ValidationResult.Failed<ManualBarrierControlModel>(null, new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });
                }

                await _repository.RemoveAsync(result);

                return ValidationResult.Success(result = null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(RemoveAsync)} method. Error: {ex.Message}");
                throw;
            }
        }
        #endregion
        #region UpdateAsync
        public async Task<ValidationResult<ManualBarrierControlModel>> UpdateAsync(Guid id, ManualBarrierControlAddOrUpdateRequestModel request)
        {
            _logger.LogInformation($"Executing {nameof(UpdateAsync)} method...");
            try
            {
                var existingRecords = await GetExistingRecordAsync(request, id);
                if (existingRecords)
                {
                    return ValidationResult.Failed<ManualBarrierControlModel>(new List<ValidationError>()
                    {
                        new ValidationError("Gía trị đã có trên hệ thống", ValidationError.Conflict.Code)
                    });
                }

                var oldRecord = await _repository.GetByIdAsync(id);
                if (oldRecord == null)
                {
                    return ValidationResult.Failed<ManualBarrierControlModel>(null, new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });
                }
                if (oldRecord.Id != id)
                {
                    return ValidationResult.Failed<ManualBarrierControlModel>(new List<ValidationError>()
                    {
                        new ValidationError("Giá trị đã có trên hệ thống", ValidationError.Conflict.Code)
                    });
                }
                _mapper.Map(request, oldRecord);

                await _repository.UpdateAsync(oldRecord);
                return ValidationResult.Success(oldRecord);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(UpdateAsync)} method. Error: {ex.Message}");
                throw;
            }
        }
        #endregion
        #region Private method
        private async Task<bool> GetExistingRecordAsync(ManualBarrierControlAddOrUpdateRequestModel input, Guid? id = null)
        {
            Expression<Func<ManualBarrierControlModel, bool>> expression = s =>
                s.EmployeeId == input.EmployeeId;

            var result = await _repository.GetAllAsync(expression);

            return result.Any(x => !x.Id.Equals(id));
        }
        #endregion
    }
}
