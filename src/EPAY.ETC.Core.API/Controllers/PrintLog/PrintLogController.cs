using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.PrintLog;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.AspNetCore.Mvc;

namespace EPAY.ETC.Core.API.Controllers.PrintLog
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("~/api/[controller]")]
    public class PrintLogController : ControllerBase
    {
        #region Variables
        private readonly ILogger<PrintLogController> _logger;
        private readonly IPrintLogService _printLogService;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="printLogService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public PrintLogController(ILogger<PrintLogController> logger,
            IPrintLogService printLogService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _printLogService = printLogService ?? throw new ArgumentNullException(nameof(printLogService));
        }

        #endregion

        #region CreatePrintLogAsync
        /// <summary>
        /// Create new vehicle
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("v1/printlogs")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddAsync(PrintLogRequestModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(AddAsync)}...");


                var vehicleResult = await _printLogService.AddAsync(request);

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
        #region GetPrintLogDetailAsync
        /// <summary>
        /// Get Vehicle Detail
        /// </summary>
        /// <param name="printLogId"></param>
        /// <returns></returns>
        [HttpGet("v1/printlogs/{printLogId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(Guid printLogId)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetByIdAsync)}...");

                var priorityVehicleResult = await _printLogService.GetByIdAsync(printLogId);

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
       
        #region DeletePrintLogAsync
        /// <summary>
        /// Remove Fusion 
        /// </summary>
        [HttpDelete("v1/printlogs/{printLogId}")]
        public async Task<IActionResult> RemoveAsync(Guid printLogId)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(RemoveAsync)}...");
                var data = await _printLogService.RemoveAsync(printLogId);

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
        /// Update Vehicle Detail
        /// </summary>
        [HttpPut("v1/printlogs/{printLogId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync(Guid printLogId, PrintLogRequestModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(UpdateAsync)}...");


                var data = await _printLogService.UpdateAsync(printLogId, request);

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
