﻿using AutoMapper;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Vehicles;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Vehicle;
using EPAY.ETC.Core.Models;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Services.Vehicles
{
    public class VehicleService : IVehicleService
    {

        #region Variables   -
        private readonly ILogger<VehicleService> _logger;
        private readonly IVehicleRepository _repository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public VehicleService(ILogger<VehicleService> logger, IVehicleRepository vehicleRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        #endregion

        public async Task<ValidationResult<VehicleModel>> AddAsync(VehicleRequestModel input)
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
                var entity = _mapper.Map<VehicleModel>(input);
                var result = await _repository.AddAsync(entity);
                return ValidationResult.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(AddAsync)} method. Error: {ex.Message}");
                throw;
            }
        }

        public async Task<ValidationResult<VehicleInfoModel>> GetVehicleByRFIDAsync(string rfid)
        {
            _logger.LogInformation($"Executing {nameof(GetVehicleByRFIDAsync)} method...");
            try
            {
                Expression<Func<VehicleModel, bool>> expression = s => !string.IsNullOrEmpty(s.RFID) && s.RFID.Equals(rfid.Trim());
                var vehicles = await _repository.GetAllAsync(expression);

                var vehicle = vehicles.FirstOrDefault();
                if (vehicle == null)
                {
                    return ValidationResult.Failed<VehicleInfoModel>(null, new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });
                }

                return ValidationResult.Success(_mapper.Map<VehicleInfoModel>(vehicle));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(GetVehicleByRFIDAsync)} method. Error: {ex.Message}");
                throw;
            }
        }

        public async Task<ValidationResult<VehicleModel>> RemoveAsync(Guid id)
        {
            _logger.LogInformation($"Executing {nameof(RemoveAsync)} method...");
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

                await _repository.RemoveAsync(result);
                return ValidationResult.Success<VehicleModel>(null!);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(RemoveAsync)} method. Error: {ex.Message}");
                throw;
            }
        }

        public async Task<ValidationResult<VehicleModel>> UpdateAsync(Guid id, VehicleRequestModel input)
        {
            _logger.LogInformation($"Executing {nameof(UpdateAsync)} method...");
            try
            {
                var oldRecord = await _repository.GetByIdAsync(id);
                if (oldRecord == null)
                {
                    return ValidationResult.Failed<VehicleModel>(null, new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });
                }
                if (oldRecord.Id != id)
                {
                    return ValidationResult.Failed<VehicleModel>(new List<ValidationError>()
                    {
                        new ValidationError("Giá trị đã có trên hệ thống", ValidationError.Conflict.Code)
                    });
                }

                _mapper.Map(input, oldRecord);
                oldRecord.Id = id;

                //oldRecord.Id = input.Id;
                //oldRecord.CreatedDate = input.CreatedDate;
                //oldRecord.RFID = input.RFID;
                //oldRecord.PlateNumber = input.PlateNumber;
                //oldRecord.PlateColor = input.PlateColor;
                //oldRecord.Make = input.Make;
                //oldRecord.Seat = input.Seat;
                //oldRecord.Weight = input.Weight;
                //oldRecord.VehicleType = input.VehicleType;

                await _repository.UpdateAsync(oldRecord);
                return ValidationResult.Success(oldRecord);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(UpdateAsync)} method. Error: {ex.Message}");
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

        #region Private method
        async Task<bool> CheckExistVehicleInfo(VehicleRequestModel input)
        {
            Expression<Func<VehicleModel, bool>> expression = s =>
                s.RFID == input.RFID || s.PlateNumber == input.PlateNumber;

            var result = await _repository.GetAllAsync(expression);

            return result.Any();
        }
        #endregion
    }
}
