using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Models.Fusion;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.AspNetCore.Mvc;

namespace EPAY.ETC.Core.API.Controllers.PaymentStatus
{
    public class PaymentStatusController: ControllerBase
    {
        #region Variables
        private readonly ILogger<PaymentStatusController> _logger;
        private readonly Core.Interfaces.Services.PaymentStatus.IPaymentStatusService _paymentStatusService;
        #endregion
        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="paymentStatusService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public PaymentStatusController(ILogger<PaymentStatusController> logger,
            Core.Interfaces.Services.PaymentStatus.IPaymentStatusService paymentStatusService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _paymentStatusService = paymentStatusService ?? throw new ArgumentNullException(nameof(paymentStatusService));
        }
        #endregion
        #region AddAsync
        /// <summary>
        /// Create new fusion
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("api/paymentstatus/v1/paymentstatuses")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddAsync([FromBody] PaymentStatusAddRequestModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(AddAsync)}...");


                var paymentStatusResult = await _paymentStatusService.AddAsync(request);

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
        [HttpGet("api/paymentstatus/v1/paymentstatuses/{objectId}")]
        public async Task<IActionResult> GetByIdAsync(Guid objectId)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetByIdAsync)}...");
                var result = await _paymentStatusService.GetByIdAsync(objectId);

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
        [HttpDelete("api/paymentstatus/v1/paymentstatuses/{objectId}")]
        public async Task<IActionResult> RemoveAsync(Guid objectId)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(RemoveAsync)}...");
                var data = await _paymentStatusService.RemoveAsync(objectId);

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
        [HttpPut("api/paymentstatus/v1/paymentstatuses/{objectId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync(Guid objectId, [FromBody] PaymentStatusUpdateRequestModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(UpdateAsync)}...");


                var data = await _paymentStatusService.UpdateAsync(objectId, request);

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
