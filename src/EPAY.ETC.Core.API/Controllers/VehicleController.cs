using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Interfaces.Services;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Core.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace EPAY.ETC.Core.API.Controllers
{
    [ApiController]
    [Route("~/api/[controller]")]

    public class VehicleController : ControllerBase
    {
        #region Variables
        private readonly ILogger<VehicleController> _logger;
        private readonly IVehicleService _vehicleService;
        #endregion

        #region Constructor
        public VehicleController(ILogger<VehicleController> logger,
            IVehicleService vehicleService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));
        }

        #endregion

        #region CreateVehicleAsync
        /// <summary>
        /// Create new employee
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("v1/vehicles")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddVehiclesAsync(VehicleModel input)
        {
            _logger.LogInformation($"Executing {nameof(AddVehiclesAsync)} method...");
            try
            {
                var vehicle = await _vehicleService.AddAsync(input);

                // Return 201
                return new ObjectResult(vehicle)
                {
                    StatusCode = StatusCodes.Status201Created
                };
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred when calling {nameof(AddVehiclesAsync)} method: {ex.Message}. InnerException : {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";
                _logger.LogError(errorMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, ValidationResult.Failed(errorMessage, new List<ValidationError>() { ValidationError.InternalServerError }));
            }
        }
        #endregion
        #region GetVehiclesDetailAsync
        [HttpGet("v1/vehicles/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVehicleByIdAsync(Guid id)
        {
            _logger.LogInformation($"Executing {nameof(GetVehicleByIdAsync)} method...");
            try
            {
                var vehicle = await _vehicleService.GetByIdAsync(id);

                if (vehicle == null)
                {
                    return NotFound();
                }

                return Ok(vehicle);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred when calling {nameof(GetVehicleByIdAsync)} method: {ex.Message}. InnerException: {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";
                _logger.LogError(errorMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, ValidationResult.Failed(errorMessage, new List<ValidationError>() { ValidationError.InternalServerError }));
            }
        }
        #endregion
        #region GetAllVehiclesAsync
        [HttpGet("v1/vehicles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllVehiclesAsync()
        {
            _logger.LogInformation($"Executing {nameof(GetAllVehiclesAsync)} method...");

            try
            {
                var vehiclesResult = await _vehicleService.GetAllVehicleAsync();

                if (vehiclesResult.Succeeded)
                {
                    return Ok(vehiclesResult.Data);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, vehiclesResult);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred when calling {nameof(GetAllVehiclesAsync)} method: {ex.Message}. InnerException: {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";
                _logger.LogError(errorMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, ValidationResult.Failed(errorMessage, new List<ValidationError>() { ValidationError.InternalServerError }));
            }
        }
        #endregion

        #region UpdateAsync
        [HttpPut("v1/vehicles/{vehicleId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateVehicleAsync(Guid Id, VehicleModel updatedVehicle)
        {
            try
            {
                var existingVehicle = await _vehicleService.GetByIdAsync(Id);
                if (existingVehicle == null)
                {
                    return NotFound();
                }

                // Update the properties of the existing vehicle
                existingVehicle.Data.RFID = updatedVehicle.RFID;
                existingVehicle.Data.PlateNumber = updatedVehicle.PlateNumber;
                existingVehicle.Data.PlateColor = updatedVehicle.PlateColor;
                existingVehicle.Data.Make = updatedVehicle.Make;
                existingVehicle.Data.Seat = updatedVehicle.Seat;
                existingVehicle.Data.Weight = updatedVehicle.Weight;
                existingVehicle.Data.VehicleType = updatedVehicle.VehicleType;
                // ... Update other properties as needed ...

                var updatedResult = await _vehicleService.UpdateAsync(Id, existingVehicle.Data);


                if (updatedResult.Succeeded)
                {
                    return Ok(updatedResult.Data);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, updatedResult);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred when calling {nameof(UpdateVehicleAsync)} method: {ex.Message}. InnerException: {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";
                _logger.LogError(errorMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, ValidationResult.Failed(errorMessage, new List<ValidationError>() { ValidationError.InternalServerError }));
            }
        }
            #endregion
        }
}
