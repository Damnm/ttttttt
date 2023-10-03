using EPAY.ETC.Core.API.Controllers.Fees;
using EPAY.ETC.Core.API.Controllers.Payment;
using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Fees;
using EPAY.ETC.Core.API.Core.Interfaces.Services.ManualBarrierControls;
using EPAY.ETC.Core.API.Core.Models.ManualBarrierControl;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.AspNetCore.Mvc;

namespace EPAY.ETC.Core.API.Controllers.ManualBarrierControls
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("~/api/[controller]")]
    public class ManualBarrierControlController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ManualBarrierControlController> _logger;
        private readonly IManualBarrierControlsService _manualBarrierControlsService;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="service"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ManualBarrierControlController(ILogger<ManualBarrierControlController> logger, IManualBarrierControlsService service)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _manualBarrierControlsService = service ?? throw new ArgumentNullException(nameof(service));
        }
        #region AddAsync
        /// <summary>
        /// Adding new Manual Barrier Controls
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("v1/manualbarriercontrol")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddAsync(ManualBarrierControlAddOrUpdateRequestModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(AddAsync)}...");

                var result = await _manualBarrierControlsService.AddAsync(request);

                if (!result.Succeeded && result.Errors.Any(x => x.Code == StatusCodes.Status409Conflict))
                {
                    return Conflict(result);
                }

                return Created(nameof(AddAsync), result);
            }
            catch (Exception ex)
            {
                List<ValidationError> validationErrors = new();
                string errorMessage = $"An error occurred when calling {nameof(AddAsync)} method: {ex.Message}. InnerException : {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";

                _logger.LogError(errorMessage);
                validationErrors.Add(ValidationError.InternalServerError);

                return new ObjectResult(ValidationResult.Failed(errorMessage, validationErrors))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
        #endregion
        #region GetByIdAsync
        /// <summary>
        /// Get Manual Barrier Controls Detail
        /// </summary>
        [HttpGet("v1/manualbarriercontrols/{manualbarriercontrolsId}")]
        public async Task<IActionResult> GetByIdAsync(Guid manualbarriercontrolsId)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetByIdAsync)}...");
                var result = await _manualBarrierControlsService.GetByIdAsync(manualbarriercontrolsId);

                return Ok(result);
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
        #region RemoveAsync
        /// <summary>
        /// Remove Manual Barrier Controls
        /// </summary>
        [HttpDelete("v1/manualbarriercontrols/{manualbarriercontrolsId}")]
        public async Task<IActionResult> RemoveAsync(Guid manualbarriercontrolsId)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(RemoveAsync)}...");
                var data = await _manualBarrierControlsService.RemoveAsync(manualbarriercontrolsId);

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
        /// <summary>
        /// Update Manual Barrier Controls Detail
        /// </summary>
        [HttpPut("v1/manualbarriercontrols/{manualbarriercontrolsId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync(Guid manualbarriercontrolsId, [FromBody] ManualBarrierControlAddOrUpdateRequestModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(UpdateAsync)}...");


                var data = await _manualBarrierControlsService.UpdateAsync(manualbarriercontrolsId, request);

                if (!data.Succeeded)
                {
                    if (data.Errors.Any(x => x.Code == StatusCodes.Status404NotFound))
                    {
                        return NotFound(data);
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
