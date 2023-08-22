using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Vehicles;
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
        public async Task<IActionResult> AddAsync(VehicleModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(AddAsync)}...");


                var vehicleResult = await _vehicleService.AddAsync(request);

                if (!vehicleResult.Succeeded && vehicleResult.Errors.Any(x => x.Code == StatusCodes.Status409Conflict))
                {
                    return Conflict(vehicleResult);
                }

                return new ObjectResult(vehicleResult)
                {
                    StatusCode = StatusCodes.Status201Created
                };
            }
            catch (Exception ex)
            {
                List<ValidationError> validationErrors = new();
                string errorMessage = $"An error occurred when calling {nameof(AddAsync)} method: {ex.Message}. InnerException : {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";
                _logger.LogError(errorMessage);
                validationErrors.Add(ValidationError.InternalServerError);
                return StatusCode(StatusCodes.Status500InternalServerError, ValidationResult.Failed(errorMessage, validationErrors));
            }
        }
        #endregion
        #region GetVehiclesDetailAsync
        [HttpGet("v1/vehicles/{vehicleId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(string vehicleId)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetByIdAsync)}...");

                Guid _vehicleId;
                if (!Guid.TryParse(vehicleId, out _vehicleId))
                {
                    return BadRequest(ValidationResult.Failed<VehicleModel>(null, new List<ValidationError>() { ValidationError.BadRequest }));
                }

                var priorityVehicleResult = await _vehicleService.GetByIdAsync(_vehicleId);

                return Ok(priorityVehicleResult);
            }
            catch (Exception ex)
            {
                List<ValidationError> validationErrors = new();
                string errorMessage = $"An error occurred when calling {nameof(GetByIdAsync)} method: {ex.Message}. InnerException : {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";
                _logger.LogError(errorMessage);
                validationErrors.Add(ValidationError.InternalServerError);
                return StatusCode(StatusCodes.Status500InternalServerError, ValidationResult.Failed(errorMessage, validationErrors));
            }
        }
        #endregion
        #region DeleteVehiclesAsync
        [HttpDelete("v1/vehicles/{vehicleId}")]
        public async Task<IActionResult> RemoveAsync(Guid vehicleId)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(RemoveAsync)}...");

                var data = await _vehicleService.RemoveAsync(vehicleId);

                if (!data.Succeeded && data.Errors.Any(x => x.Code == StatusCodes.Status404NotFound))
                {
                    return NotFound(data);
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                List<ValidationError> validationErrors = new();
                string errorMessage = $"An error occurred when calling {nameof(RemoveAsync)} method: {ex.Message}. InnerException : {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";
                _logger.LogError(errorMessage);
                validationErrors.Add(ValidationError.InternalServerError);
                return StatusCode(StatusCodes.Status500InternalServerError, ValidationResult.Failed(errorMessage, validationErrors));
            }
        }
        #endregion
        #region UpdateAsync
        [HttpPut("v1/vehicles/{vehicleId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync(string vehicleId, VehicleModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(UpdateAsync)}...");
               

                Guid _vehicleId;
                if (!Guid.TryParse(vehicleId, out _vehicleId))
                {
                    return BadRequest(ValidationResult.Failed<VehicleModel>(null, new List<ValidationError>() { ValidationError.BadRequest }));
                }

                request.Id = _vehicleId;
                var data = await _vehicleService.UpdateAsync(request);

                if (!data.Succeeded)
                {
                    if (data.Errors.Any(x => x.Code == StatusCodes.Status404NotFound))
                    {
                        return NotFound(data);
                    }

                    if (data.Errors.Any(x => x.Code == StatusCodes.Status409Conflict))
                    {
                        return Conflict(data);
                    }
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                List<ValidationError> validationErrors = new();
                string errorMessage = $"An error occurred when calling {nameof(UpdateAsync)} method: {ex.Message}. InnerException : {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";
                _logger.LogError(errorMessage);
                validationErrors.Add(ValidationError.InternalServerError);
                return StatusCode(StatusCodes.Status500InternalServerError, ValidationResult.Failed(errorMessage, validationErrors));
            }            
        }
        #endregion
    }
}
