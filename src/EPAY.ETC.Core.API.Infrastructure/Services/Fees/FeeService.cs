using AutoMapper;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Fees;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Fees;
using EPAY.ETC.Core.Models.Constants;
using EPAY.ETC.Core.Models.Fees;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Linq.Expressions;
using System.Text.Json;
using CoreModel = EPAY.ETC.Core.Models.Fees;
using FeeModel = EPAY.ETC.Core.API.Core.Models.Fees.FeeModel;

namespace EPAY.ETC.Core.API.Infrastructure.Services.Fees
{
    public class FeeService : IFeeService
    {
        private readonly ILogger<FeeService> _logger;
        private readonly IFeeRepository _repository;
        private readonly IMapper _mapper;
        private readonly IDatabase _redisDB;

        public FeeService(
            ILogger<FeeService> logger, IFeeRepository repository, IMapper mapper, IDatabase redisDB)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper;
            _redisDB = redisDB;
        }

        public async Task<ValidationResult<FeeModel>> AddAsync(CoreModel.FeeModel input)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(AddAsync)} method...");

                bool existRecord = await CheckExistsRecordByObjectId(input.FeeId, input.ObjectId);
                if (existRecord)
                {
                    return ValidationResult.Failed<FeeModel>(new List<ValidationError>()
                    {
                        ValidationError.Conflict
                    });
                }

                var entity = _mapper.Map<FeeModel>(input);

                var result = await _repository.AddAsync(entity);

                return ValidationResult.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(AddAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ValidationResult<IEnumerable<FeeModel>>> GetAllAsync(Expression<Func<FeeModel, bool>>? expressison = null)
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

        public async Task<ValidationResult<FeeModel?>> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");

                var result = await _repository.GetByIdAsync(id);

                return ValidationResult.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetByIdAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ValidationResult<CoreModel.FeeModel?>> GetByObjectIdAsync(string objectId)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");

                Guid.TryParse(objectId, out Guid guid);
                var result = await _repository.GetByObjectIdAsync(guid);

                return ValidationResult.Success(_mapper.Map<CoreModel.FeeModel?>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetByIdAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ValidationResult<FeeModel?>> RemoveAsync(Guid id)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(RemoveAsync)} method...");

                var oldRecord = await _repository.GetByIdAsync(id);
                if (oldRecord == null)
                {
                    return ValidationResult.Failed<FeeModel?>(new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });
                }

                await _repository.RemoveAsync(oldRecord);

                return ValidationResult.Success<FeeModel?>(null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(RemoveAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ValidationResult<FeeModel>> UpdateAsync(Guid id, CoreModel.FeeModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(UpdateAsync)} method...");

                var oldRecord = await _repository.GetByIdAsync(id);
                if (oldRecord == null)
                {
                    return ValidationResult.Failed<FeeModel>(new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });
                }

                bool existRecord = await CheckExistsRecordByObjectId(id, request.ObjectId);
                if (existRecord)
                {
                    return ValidationResult.Failed<FeeModel>(new List<ValidationError>()
                    {
                        ValidationError.Conflict
                    });
                }

                var entity = _mapper.Map<FeeModel>(request);
                entity.Id = id;

                await _repository.UpdateAsync(entity);

                return ValidationResult.Success(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(UpdateAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        #region Private method
        private async Task<bool> CheckExistsRecordByObjectId(Guid? id, Guid objectId)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(CheckExistsRecordByObjectId)} method...");

                Expression<Func<FeeModel, bool>> expression = s => (id.HasValue ? s.Id != id : true) && s.ObjectId == objectId;

                var result = await _repository.GetAllAsync(expression);

                return result.Any();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(CheckExistsRecordByObjectId)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        #endregion

        public async Task<ValidationResult<List<LaneInVehicleModel>>> FindVehicleAsync(string inputVehicle)
        {
            _logger.LogInformation($"Executing {nameof(FindVehicleAsync)} method...");

            try
            {
                if (string.IsNullOrEmpty(inputVehicle)) return ValidationResult.Failed<List<LaneInVehicleModel>>(new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });

                List<LaneInVehicleModel> result = new List<LaneInVehicleModel>();
                if (inputVehicle.Length>=15)
                {
                    var keyRFIDVehicles = await _redisDB.ExecuteAsync("keys", RedisConstant.StringType_RFIDInKey($"*{inputVehicle}*"));
                    if (keyRFIDVehicles != null)
                    {
                        var laneInRFIDVehicle = await _redisDB.StringGetAsync((RedisKey[]?)keyRFIDVehicles);
                        List<Core.Models.Devices.RFID.RFIDModel?> resultData = laneInRFIDVehicle.ToList()
                            .Select(s => JsonSerializer.Deserialize<Core.Models.Devices.RFID.RFIDModel>(s.ToString())).ToList();
                        if (resultData != null && resultData.Count > 0)
                        {
                            result = _mapper.Map<List<Core.Models.Devices.RFID.RFIDModel>, List<LaneInVehicleModel>>(resultData);
                            return ValidationResult.Success(result);
                        }
                    }
                }
                else if (inputVehicle.Length < 15)
                {
                    var keyCAMVehicles = await _redisDB.ExecuteAsync("keys", RedisConstant.StringType_CameraInKey($"*{inputVehicle}*"));
                    if (keyCAMVehicles != null)
                    {
                        var laneInCAMVehicle = await _redisDB.StringGetAsync((RedisKey[]?)keyCAMVehicles);
                        List<Core.Models.Devices.Camera.CameraModel?> resultData = laneInCAMVehicle.ToList()
                            .Select(s => JsonSerializer.Deserialize<Core.Models.Devices.Camera.CameraModel>(s.ToString())).ToList();
                        if (resultData != null && resultData.Count > 0)
                        {
                            result = _mapper.Map<List<Core.Models.Devices.Camera.CameraModel>, List<LaneInVehicleModel>>(resultData);
                            return ValidationResult.Success(result);
                        }
                    }
                }

                return ValidationResult.Failed<List<LaneInVehicleModel>>(new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(FindVehicleAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
