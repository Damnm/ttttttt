using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.AspNetCore.Mvc;

namespace EPAY.ETC.Core.API.Controllers.Payment
{
    public class PaymentController : ControllerBase
    {
        #region Variables
        private readonly ILogger<PaymentController> _logger;
        private readonly Core.Interfaces.Services.Payment.IPaymentService _paymentService;
        #endregion
        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="paymentStatusService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public PaymentController(ILogger<PaymentController> logger,
            Core.Interfaces.Services.Payment.IPaymentService paymentStatusService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _paymentService = paymentStatusService ?? throw new ArgumentNullException(nameof(paymentStatusService));
        }
        #endregion
        #region AddAsync
        /// <summary>
        /// Create new fusion
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("api/payment/v1/payments")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddAsync([FromBody] PaymentAddRequestModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(AddAsync)}...");


                var paymentStatusResult = await _paymentService.AddAsync(request);

                if (!paymentStatusResult.Succeeded && paymentStatusResult.Errors.Any(x => x.Code == StatusCodes.Status409Conflict))
                {
                    return Conflict(paymentStatusResult);
                }

                return new ObjectResult(paymentStatusResult)
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
        [HttpGet("api/payment/v1/payments/{paymentid}")]
        public async Task<IActionResult> GetByIdAsync(Guid paymentId)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetByIdAsync)}...");
                var result = await _paymentService.GetByIdAsync(paymentId);

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
        [HttpDelete("api/payment/v1/payments/{paymentid}")]
        public async Task<IActionResult> RemoveAsync(Guid paymentId)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(RemoveAsync)}...");
                var data = await _paymentService.RemoveAsync(paymentId);

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
        [HttpPut("api/payment/v1/payments/{paymentid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync(Guid paymentId, [FromBody] PaymentUpdateRequestModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(UpdateAsync)}...");


                var data = await _paymentService.UpdateAsync(paymentId, request);

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
