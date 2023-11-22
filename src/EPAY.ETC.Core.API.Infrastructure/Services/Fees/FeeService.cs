using AutoMapper;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Fees;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Fees;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.TicketType;
using EPAY.ETC.Core.Models.Constants;
using EPAY.ETC.Core.Models.Devices;
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
        private readonly ITicketTypeRepository _ticketTypeRepository;
        private readonly IMapper _mapper;
        private readonly IDatabase _redisDB;

        public FeeService(ILogger<FeeService> logger,
                          IFeeRepository repository,
                          ITicketTypeRepository ticketTypeRepository,
                          IMapper mapper,
                          IDatabase redisDB)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _ticketTypeRepository = ticketTypeRepository ?? throw new ArgumentNullException(nameof(ticketTypeRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _redisDB = redisDB ?? throw new ArgumentNullException(nameof(redisDB));
        }

        public async Task<ValidationResult<FeeModel>> AddAsync(CoreModel.FeeModel input)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(AddAsync)} method...");

                var existFees = await GetByIdAsync(input.FeeId ?? Guid.NewGuid());
                if (existFees?.Data != null)
                {
                    _logger.LogWarning($"Executing {nameof(AddAsync)} method, existed object {JsonSerializer.Serialize(input)} in database.");

                    return await UpdateAsync(existFees.Data!.Id, input);
                }

                var entity = _mapper.Map<FeeModel>(input);

                if (string.IsNullOrEmpty(entity.LaneOutId))
                {
                    entity.LaneOutId = Environment.GetEnvironmentVariable(CoreConstant.ENVIRONMENT_LANE_OUT) ?? "1";
                }
                if (!string.IsNullOrEmpty(input.Payment?.TicketTypeId))
                {
                    var ticketTypes = await _ticketTypeRepository.GetAllAsync(s => s.Code.Equals(input.Payment.TicketTypeId ?? string.Empty));
                    if (ticketTypes.Any())
                        entity.TicketTypeId = ticketTypes.FirstOrDefault()?.Id;
                }

                _logger.LogWarning($"Executing {nameof(AddAsync)} method, add new object {JsonSerializer.Serialize(input)} to database.");
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
                    _logger.LogWarning($"Executing {nameof(UpdateAsync)} method, not found object {JsonSerializer.Serialize(request)} in database.");
                    return ValidationResult.Failed<FeeModel>(new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });
                }

                var entity = _mapper.Map<FeeModel>(request);
                entity.Id = id;

                if (string.IsNullOrEmpty(entity.LaneOutId))
                {
                    entity.LaneOutId = Environment.GetEnvironmentVariable(CoreConstant.ENVIRONMENT_LANE_OUT) ?? "1";
                }
                _logger.LogWarning($"Executing {nameof(UpdateAsync)} method, found object {JsonSerializer.Serialize(request)} in database and updates its.");
                await _repository.UpdateAsync(entity);

                return ValidationResult.Success(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(UpdateAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public ValidationResult<List<LaneInVehicleModel>> FindVehicle(string inputVehicle)
        {
            _logger.LogInformation($"Executing {nameof(FindVehicle)} method...");

            try
            {
                if (string.IsNullOrEmpty(inputVehicle)) return ValidationResult.Failed<List<LaneInVehicleModel>>(new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });

                List<LaneInVehicleModel> result = new List<LaneInVehicleModel>();
                if (inputVehicle.Length >= 15)
                {
                    var keyRFIDVehicles = _redisDB.Execute("keys", RedisConstant.RFIDInKey($"*{inputVehicle}*"));
                    var redisKeys = (RedisKey[]?)keyRFIDVehicles;
                    if (redisKeys != null)
                    {
                        var laneInRFIDVehicle = _redisDB.StringGet(redisKeys);
                        List<Core.Models.Devices.RFID.RFIDModel> resultData = laneInRFIDVehicle.ToList()
                            .Select(s => JsonSerializer.Deserialize<Core.Models.Devices.RFID.RFIDModel>(s.ToString()) ?? new Core.Models.Devices.RFID.RFIDModel()).ToList();
                        if (resultData != null && resultData.Count > 0)
                        {
                            result = _mapper.Map<List<Core.Models.Devices.RFID.RFIDModel>, List<LaneInVehicleModel>>(resultData);
                            return ValidationResult.Success(result);
                        }
                    }
                }
                else if (inputVehicle.Length < 15)
                {
                    var keyCAMVehicles = _redisDB.Execute("keys", RedisConstant.CameraInKey($"*{inputVehicle}*"));
                    var redisKeys = (RedisKey[]?)keyCAMVehicles;
                    if (redisKeys != null)
                    {
                        var laneInCAMVehicle = _redisDB.StringGet(redisKeys);
                        List<ANPRCameraModel> resultData = laneInCAMVehicle.ToList()
                            .Select(s => JsonSerializer.Deserialize<ANPRCameraModel>(s.ToString()) ?? new ANPRCameraModel()).ToList();
                        if (resultData != null && resultData.Count > 0)
                        {
                            result = _mapper.Map<List<ANPRCameraModel>, List<LaneInVehicleModel>>(resultData);
                            return ValidationResult.Success(result);
                        }
                    }
                }

                return ValidationResult.Failed<List<LaneInVehicleModel>>(new List<ValidationError>() { ValidationError.NotFound });
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(FindVehicle)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
