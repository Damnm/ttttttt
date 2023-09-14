using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Fusion;
using EPAY.ETC.Core.API.Core.Models.Fusion;
using EPAY.ETC.Core.API.Core.Validation;
using Microsoft.AspNetCore.Mvc;

namespace EPAY.ETC.Core.API.Controllers.Fusion
{
    /// <summary>
    /// 
    /// </summary>
    public class FusionController : ControllerBase
    {
        #region Variables
        private readonly ILogger<FusionController> _logger;
        private readonly IFusionService _fusionService;
        #endregion
        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="fusionService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public FusionController(ILogger<FusionController> logger,
            IFusionService fusionService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _fusionService = fusionService ?? throw new ArgumentNullException(nameof(fusionService));
        }
        #endregion
        #region AddAsync
        /// <summary>
        /// Create new fusion
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("api/Fusion/v1/fusions")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddAsync([FromBody] FusionAddRequestModel request)
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
        #region GetByIdAsync
        /// <summary>
        /// Get Fusion Detail
        /// </summary>
        [HttpGet("api/Fusion/v1/fusions/{objectId}")]
        public async Task<IActionResult> GetByIdAsync(Guid objectId)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetByIdAsync)}...");
                var result = await _fusionService.GetByIdAsync(objectId);

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
        /// Remove Fusion
        /// </summary>
        [HttpDelete("api/Fusion/v1/fusions/{objectId}")]
        public async Task<IActionResult> RemoveAsync(Guid objectId)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(RemoveAsync)}...");
                var data = await _fusionService.RemoveAsync(objectId);

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
        /// Update Fusion Detail
        /// </summary>
        [HttpPut("api/Fusion/v1/fusions/{objectId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync(Guid objectId, [FromBody] FusionUpdateRequestModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(UpdateAsync)}...");


                var data = await _fusionService.UpdateAsync(objectId, request);

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
