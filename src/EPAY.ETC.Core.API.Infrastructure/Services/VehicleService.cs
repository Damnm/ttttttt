using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Interfaces.Services;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Core.Validation;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Services
{
    internal class VehicleService : IVehicleService
    {

        #region Variables   -
        private readonly ILogger<VehicleService> _logger;
        private readonly IVehicleRepository _vehicleRepository;
        #endregion

        #region Constructor
        public VehicleService(ILogger<VehicleService> logger, IVehicleRepository vehicleRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
        }
        #endregion
        public async Task<ValidationResult<VehicleModel>> AddAsync(VehicleModel entity)
        {
            _logger.LogInformation($"Executing {nameof(AddAsync)} method...");
            try
            {
                entity.Id = Guid.NewGuid();
                var res = await _vehicleRepository.AddAsync(entity);
                return ValidationResult.Success(res);
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
                var vehicle = await _vehicleRepository.GetByIdAsync(id);

                if (vehicle == null)
                {
                    return ValidationResult.Failed<VehicleModel>(ValidationError.NotFound);
                }

                return ValidationResult.Success(vehicle);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetByIdAsync)} method. Error: {ex.Message}");
                throw;
            }
        }
        public async Task<ValidationResult<IEnumerable<VehicleModel>>> GetAllVehicleAsync()
        {
            _logger.LogInformation($"Executing {nameof(GetAllVehicleAsync)} method...");

            try
            {
                var vehicles = await _vehicleRepository.GetAllAsync();
                return ValidationResult.Success(vehicles);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred when calling {nameof(GetAllVehicleAsync)} method: {ex.Message}. InnerException: {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";
                throw;
            }
        }

        public async Task<ValidationResult<VehicleModel>> UpdateAsync(Guid id, VehicleModel updatedVehicle)
        {
            _logger.LogInformation($"Executing {nameof(UpdateAsync)} method...");

            try
            {
                var existingVehicle = await _vehicleRepository.GetByIdAsync(id);

                if (existingVehicle == null)
                {
                    return ValidationResult.Failed<VehicleModel>("Vehicle not found.");
                }

                // Update the properties of the existing vehicle
                existingVehicle.RFID = updatedVehicle.RFID;
                existingVehicle.PlateNumber = updatedVehicle.PlateNumber;
                existingVehicle.PlateColor = updatedVehicle.PlateColor;
                existingVehicle.Make = updatedVehicle.Make;
                existingVehicle.Seat = updatedVehicle.Seat;
                existingVehicle.Weight = updatedVehicle.Weight;
                existingVehicle.VehicleType = updatedVehicle.VehicleType;
                // ... Update other properties as needed ...

                var updatedResult = await _vehicleRepository.UpdateAsync(existingVehicle);
                return ValidationResult.Success(updatedResult);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred when calling {nameof(UpdateAsync)} method: {ex.Message}. InnerException: {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";
                throw;
            }
        }
    }
}
