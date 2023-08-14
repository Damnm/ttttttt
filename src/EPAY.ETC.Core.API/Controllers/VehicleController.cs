using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Interfaces.Services;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Core.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPAY.ETC.Core.API.Controllers
{
    [ApiController]
    [Route("~/api/[controller]")]

    public class VehicleController : ControllerBase
    {
        #region Variables
        private readonly ILogger<VehicleController> _logger;
        private readonly IVehicleService  _vehicleService;
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
        /// <param name="stationId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("v1/vehicles")]
        [ProducesResponseType(StatusCodes.Status201Created)]
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

        
    }
}
