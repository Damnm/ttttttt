using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.ETCCheckouts;
using EPAY.ETC.Core.API.Core.Models.ETCCheckOuts;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.AspNetCore.Mvc;

namespace EPAY.ETC.Core.API.Controllers.ETCCheckouts
{
    /// <summary>
    /// API CRUD for ETC checkout
    /// </summary>
    [ApiController]
    [Route("~/api/[controller]")]
    public class ETCCheckoutController : ControllerBase
    {
        private readonly ILogger<ETCCheckoutController> _logger;
        private readonly IETCCheckoutService _service;

        /// <summary>
        /// Initial constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="etcCheckOutService"></param>
        public ETCCheckoutController(ILogger<ETCCheckoutController> logger, IETCCheckoutService etcCheckOutService)
        {
            _logger = logger;
            _service = etcCheckOutService;
        }

        /// <summary>
        /// Create new ETC checkout
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("v1/etcCheckouts")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddAsync(ETCCheckoutAddUpdateRequestModel input)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(AddAsync)}...");

                var result = await _service.AddAsync(input);

                if (!result.Succeeded)
                {
                    if (result.Errors.Any(x => x.Code == StatusCodes.Status409Conflict))
                        return Conflict(result);

                    if (result.Errors.Any(x => x.Code == StatusCodes.Status400BadRequest))
                        return BadRequest(result);
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

        /// <summary>
        /// Update ETC checkout by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("v1/etcCheckouts/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync(Guid id, ETCCheckoutAddUpdateRequestModel input)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(UpdateAsync)}...");

                var result = await _service.UpdateAsync(id, input);

                if (!result.Succeeded)
                {
                    if (result.Errors.Any(x => x.Code == StatusCodes.Status409Conflict))
                        return Conflict(result);

                    if (result.Errors.Any(x => x.Code == StatusCodes.Status404NotFound))
                        return NotFound(result);

                    if (result.Errors.Any(x => x.Code == StatusCodes.Status400BadRequest))
                        return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                List<ValidationError> validationErrors = new();
                string errorMessage = $"An error occurred when calling {nameof(UpdateAsync)} method: {ex.Message}. InnerException : {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";

                _logger.LogError(errorMessage);
                validationErrors.Add(ValidationError.InternalServerError);

                return new ObjectResult(ValidationResult.Failed(errorMessage, validationErrors))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        /// Remove one ETC checkout by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("v1/etcCheckouts/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveAsync(Guid id)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(RemoveAsync)}...");

                var result = await _service.RemoveAsync(id);

                if (!result.Succeeded && result.Errors.Any(x => x.Code == StatusCodes.Status404NotFound))
                {
                    return NotFound(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                List<ValidationError> validationErrors = new();
                string errorMessage = $"An error occurred when calling {nameof(RemoveAsync)} method: {ex.Message}. InnerException : {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";

                _logger.LogError(errorMessage);
                validationErrors.Add(ValidationError.InternalServerError);

                return new ObjectResult(ValidationResult.Failed(errorMessage, validationErrors))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        /// Get one ETC checkout by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("v1/etcCheckouts/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetByIdAsync)}...");

                var result = await _service.GetByIdAsync(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                List<ValidationError> validationErrors = new();
                string errorMessage = $"An error occurred when calling {nameof(GetByIdAsync)} method: {ex.Message}. InnerException : {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";

                _logger.LogError(errorMessage);
                validationErrors.Add(ValidationError.InternalServerError);

                return new ObjectResult(ValidationResult.Failed(errorMessage, validationErrors))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        /// Get all ECT checkout
        /// </summary>
        /// <returns></returns>
        [HttpGet("v1/etcCheckouts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetAllAsync)}...");

                var result = await _service.GetAllAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                List<ValidationError> validationErrors = new();
                string errorMessage = $"An error occurred when calling {nameof(GetAllAsync)} method: {ex.Message}. InnerException : {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";

                _logger.LogError(errorMessage);
                validationErrors.Add(ValidationError.InternalServerError);

                return new ObjectResult(ValidationResult.Failed(errorMessage, validationErrors))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        /// Get all ECT checkout by condition and paging
        /// </summary>
        /// <returns></returns>
        [HttpPost("v1/etcCheckouts/advance-filter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllByConditionAsync(ETCCheckoutFilterModel? filter = null)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetAllByConditionAsync)}...");

                var result = await _service.GetAllByConditionAsync(filter);

                return Ok(result);
            }
            catch (Exception ex)
            {
                List<ValidationError> validationErrors = new();
                string errorMessage = $"An error occurred when calling {nameof(GetAllByConditionAsync)} method: {ex.Message}. InnerException : {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";

                _logger.LogError(errorMessage);
                validationErrors.Add(ValidationError.InternalServerError);

                return new ObjectResult(ValidationResult.Failed(errorMessage, validationErrors))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    }
}
