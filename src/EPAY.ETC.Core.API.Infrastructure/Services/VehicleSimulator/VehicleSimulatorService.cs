using AutoMapper;
using EPAY.ETC.Core.API.Core.Interfaces.Services.VehicleSimulator;
using EPAY.ETC.Core.API.Core.Models.Simulator;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Vehicle;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.VehicleSimulator;
using EPAY.ETC.Core.API.Infrastructure.Services.Vehicles;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Infrastructure.Services.VehicleSimulator
{
    public class VehicleSimulatorService : IVehicleSimulatorService
    {
        #region Variables   -
        private readonly ILogger<VehicleSimulatorService> _logger;
        private readonly IVehicleSimulatorRepository _repository;
        private readonly IMapper _mapper;
        #endregion
        #region Constructor
        public VehicleSimulatorService(ILogger<VehicleSimulatorService> logger, IVehicleSimulatorRepository vehicleSimulatorRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = vehicleSimulatorRepository ?? throw new ArgumentNullException(nameof(vehicleSimulatorRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        #endregion
        public async Task<ValidationResult<VehicleSimulatorModel>> AddAsync(VehicleSimulatorRequestModel input)
        {
            _logger.LogInformation($"Executing {nameof(AddAsync)} method...");
            try
            {
                var existVehicle = await CheckExistVehicleInfo(input);

                if (existVehicle)
                {
                    return ValidationResult.Failed<VehicleSimulatorModel>(new List<ValidationError>()
                    {
                        new ValidationError("Dữ liệu đã tồn tại trên hệ thống", ValidationError.Conflict.Code)
                    });
                }
                var entity = _mapper.Map<VehicleSimulatorModel>(input);
                var result = await _repository.AddAsync(entity);
                return ValidationResult.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(AddAsync)} method. Error: {ex.Message}");
                throw;
            }
        }

        public async Task<ValidationResult<VehicleSimulatorModel>> RemoveAsync(Guid id)
        {
            _logger.LogInformation($"Executing {nameof(RemoveAsync)} method...");
            try
            {
                var result = await _repository.GetByIdAsync(id);
                if (result == null)
                {
                    return ValidationResult.Failed<VehicleSimulatorModel>(null, new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });
                }

                await _repository.RemoveAsync(result);
                return ValidationResult.Success<VehicleSimulatorModel>(null!);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(RemoveAsync)} method. Error: {ex.Message}");
                throw;
            }
        }

        public async Task<ValidationResult<VehicleSimulatorModel>> UpdateAsync(Guid id, VehicleSimulatorRequestModel input)
        {
            _logger.LogInformation($"Executing {nameof(UpdateAsync)} method...");
            try
            {
                var oldRecord = await _repository.GetByIdAsync(id);
                if (oldRecord == null)
                {
                    return ValidationResult.Failed<VehicleSimulatorModel>(null, new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });
                }
                if (oldRecord.Id != id)
                {
                    return ValidationResult.Failed<VehicleSimulatorModel>(new List<ValidationError>()
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

        public async Task<ValidationResult<VehicleSimulatorModel>> GetByIdAsync(Guid id)
        {
            _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");
            try
            {
                var result = await _repository.GetByIdAsync(id);
                if (result == null)
                {
                    return ValidationResult.Failed<VehicleSimulatorModel>(null, new List<ValidationError>()
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
        #region Private method
        async Task<bool> CheckExistVehicleInfo(VehicleSimulatorRequestModel input)
        {
            Expression<Func<VehicleSimulatorModel, bool>> expression = s =>
                s.Etag == input.Etag || s.PlateNumber == input.PlateNumber;

            var result = await _repository.GetAllAsync(expression);

            return result.Any();
        }
        #endregion
    }
}
