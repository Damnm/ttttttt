using AutoMapper;
using EPAY.ETC.Core.API.Core.Extensions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Fusion;
using EPAY.ETC.Core.API.Core.Models.Fusion;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Core.Validation;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Fusion;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Vehicle;
using EPAY.ETC.Core.API.Infrastructure.Services.Vehicles;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Infrastructure.Services.Fusion
{
    public class FusionService : IFusionService
    {
        #region Variables   -
        private readonly ILogger<FusionService> _logger;
        private readonly IFusionRepository _repository;
        private readonly IMapper _mapper;
        #endregion
        #region Constructor
        public FusionService(ILogger<FusionService> logger, IFusionRepository fusionRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = fusionRepository ?? throw new ArgumentNullException(nameof(fusionRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        #endregion
        #region AddAsync
        public async Task<ValidationResult<FusionModel>> AddAsync(FusionAddRequestModel input)
        {
            _logger.LogInformation($"Executing {nameof(AddAsync)} method...");
            try
            {
                var existingRecords = await GetExistingRecordAsync(input);
                if (existingRecords)
                {
                    return ValidationResult.Failed<FusionModel>(new List<ValidationError>()
                    {
                        new ValidationError("Gía trị đã có trên hệ thống", ValidationError.Conflict.Code)
                    });
                }
                var entity = _mapper.Map<FusionModel>(input);

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
        public async Task<ValidationResult<FusionModel>> GetByIdAsync(Guid id)
        {
            _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");
            try
            {
                var result = await _repository.GetByIdAsync(id);
                if (result == null)
                {
                    return ValidationResult.Failed<FusionModel>(null, new List<ValidationError>()
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
        public async Task<ValidationResult<FusionModel>> RemoveAsync(Guid id)
        {
            _logger.LogInformation($"Executing {nameof(RemoveAsync)} method...");
            try
            {
                var result = await _repository.GetByIdAsync(id);
                if (result == null)
                {
                    return ValidationResult.Failed<FusionModel>(null, new List<ValidationError>()
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
        public async Task<ValidationResult<FusionModel>> UpdateAsync(Guid id, FusionUpdateRequestModel request)
        {
            _logger.LogInformation($"Executing {nameof(UpdateAsync)} method...");
            try
            {
                var oldRecord = await _repository.GetByIdAsync(id);
                if (oldRecord == null)
                {
                    return ValidationResult.Failed<FusionModel>(null, new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });
                }
                if (oldRecord.Id != id)
                {
                    return ValidationResult.Failed<FusionModel>(new List<ValidationError>()
                    {
                        new ValidationError("Giá trị đã có trên hệ thống", ValidationError.Conflict.Code)
                    });
                }
                //oldRecord.Id = request.ObjectId;

                _mapper.Map(request, oldRecord);
                oldRecord.Id = id;

                //oldRecord.Epoch = request.Epoch;
                //oldRecord.Loop1 = request.Loop1;
                //oldRecord.RFID = request.RFID;
                //oldRecord.Cam1 = request.Cam1;
                //oldRecord.Loop2 = request.Loop2;
                //oldRecord.Cam2 = request.Cam2;
                //oldRecord.Loop3 = request.Loop3;
                //oldRecord.ReversedLoop1 = request.ReversedLoop1;
                //oldRecord.ReversedLoop2 = request.ReversedLoop2;

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
        async Task<bool> GetExistingRecordAsync(FusionAddRequestModel input, string? id = null)
        {
            Expression<Func<FusionModel, bool>> expression = s =>
                s.Id == input.ObjectId;

            var result = await _repository.GetAllAsync(expression);

            return result.Any(x => !x.Id.ToString().Equals(id));
        }
        #endregion
    }
}

