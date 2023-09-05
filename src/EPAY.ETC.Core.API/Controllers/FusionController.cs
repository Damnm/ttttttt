using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Fusion;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Vehicles;
using EPAY.ETC.Core.API.Core.Models.Common;
using EPAY.ETC.Core.API.Core.Models.Fusion;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Core.Validation;
using EPAY.ETC.Core.API.Infrastructure.Services.Vehicles;
using Microsoft.AspNetCore.Mvc;

namespace EPAY.ETC.Core.API.Controllers
{
    public class FusionController : ControllerBase
    {
        #region Variables
        private readonly ILogger<FusionController> _logger;
        private readonly IFusionService _fusionService;
        #endregion
        #region Constructor
        public FusionController(ILogger<FusionController> logger,
            IFusionService fusionService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _fusionService = fusionService ?? throw new ArgumentNullException(nameof(fusionService));
        }
        #endregion
        #region CreateVehicleAsync
        /// <summary>
        /// Create new employee
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("v1/fusion")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddAsync([FromBody]FusionRequestModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(AddAsync)}...");


                var fusionResult = await _fusionService.AddAsync(request);

                if (!fusionResult.Succeeded && fusionResult.Errors.Any(x => x.Code == StatusCodes.Status409Conflict))
                {
                    return Conflict(fusionResult);
                }

                return new ObjectResult(fusionResult)
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
    }
}
