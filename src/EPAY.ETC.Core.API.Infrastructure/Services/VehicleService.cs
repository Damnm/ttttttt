using AutoMapper;
using EPAY.ETC.Core.API.Core.Extensions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Vehicles;
using EPAY.ETC.Core.API.Core.Models.Common;
using EPAY.ETC.Core.API.Core.Models.Enum;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Core.Validation;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Vehicle;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Services
{
    public class VehicleService : IVehicleService
    {

        #region Variables   -
        private readonly ILogger<VehicleService> _logger;
        private readonly IVehicleRepository _repository;
        private readonly IVehicleHistoryRepository _vehicleHistoryRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public VehicleService(ILogger<VehicleService> logger, IVehicleRepository vehicleRepository, IVehicleHistoryRepository vehicleHistoryRepository ,IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
            _vehicleHistoryRepository = vehicleHistoryRepository ?? throw new ArgumentNullException(nameof(vehicleHistoryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        #endregion
        public async Task<ValidationResult<VehicleModel>> AddAsync(VehicleModel input)
        {
            _logger.LogInformation($"Executing {nameof(AddAsync)} method...");
            try
            {
                var existVehicle = await CheckExistVehicleInfo(input);

                if (existVehicle)
                {
                    return ValidationResult.Failed<VehicleModel>(new List<ValidationError>()
                    {
                        new ValidationError("Dữ liệu đã tồn tại trên hệ thống", ValidationError.Conflict.Code)
                    });
                }

                input.CreatedDate = DateTime.Now.ConvertToAsianTime(DateTimeKind.Local);
                

                var result = await _repository.AddAsync(input);
                await AddHistory(result, ChangeActionEnum.Insert);

                return ValidationResult.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(AddAsync)} method. Error: {ex.Message}");
                throw;
            }
        }

        public async Task<ValidationResult<VehicleModel>> GetByIdAsync(Guid id)
        {
            _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");
            try
            {
                var result = await _repository.GetByIdAsync(id);
                if (result == null)
                {
                    return ValidationResult.Failed<VehicleModel>(null, new List<ValidationError>()
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

        public async Task<ValidationResult<Guid>> RemoveAsync(Guid id)
        {
            _logger.LogInformation($"Executing {nameof(RemoveAsync)} method...");
            try
            {
                var result = await _repository.GetByIdAsync(id);
                if (result == null)
                {
                    return ValidationResult.Failed<Guid>(Guid.Empty, new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });
                }

                await _repository.RemoveAsync(result);
                await AddHistory(result, ChangeActionEnum.Delete);

                return ValidationResult.Success(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(RemoveAsync)} method. Error: {ex.Message}");
                throw;
            }
        }

        public async Task<ValidationResult<VehicleModel>> UpdateAsync(VehicleModel input)
        {
            _logger.LogInformation($"Executing {nameof(UpdateAsync)} method...");
            try
            {
                var oldRecord = await _repository.GetByIdAsync(input.Id);
                if (oldRecord == null)
                {
                    return ValidationResult.Failed<VehicleModel>(null, new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });
                }

                input.CreatedDate = oldRecord.CreatedDate;
                

                await _repository.UpdateAsync(input);
                await AddHistory(input, ChangeActionEnum.Update);

                return ValidationResult.Success(input);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(UpdateAsync)} method. Error: {ex.Message}");
                throw;
            }
        }

        #region Private method
        async Task AddHistory(VehicleModel input, ChangeActionEnum action)
        {
            var history = _mapper.Map<VehicleHistoryModel>(input);
            history.Action = action;

            await _vehicleHistoryRepository.AddAsync(history);
        }
        async Task<bool> CheckExistVehicleInfo(VehicleModel input)
        {
            Expression<Func<VehicleModel, bool>> expression = s =>
                s.CreatedDate == input.CreatedDate
                && s.PlateNumber == input.PlateNumber
                && s.PlateColor == input.PlateColor
                && s.RFID == input.RFID
                && s.Seat == input.Seat
                && s.Make == input.Make
                && s.Weight == input.Weight;

            var result = await _repository.GetAllAsync(expression);

            return result.Any();
        }
        #endregion
    }
}
