using AutoMapper;
using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.UIActions;
using EPAY.ETC.Core.API.Models.Configs;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using EPAY.ETC.Core.Publisher.Common.Options;
using EPAY.ETC.Core.Publisher.Interface;
using EPAY.ETC.Core.RabbitMQ.Common.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace EPAY.ETC.Core.API.Controllers.PaymentStatus
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("~/api/[controller]")]
    public class PaymentStatusController : ControllerBase
    {
        #region Variables
        private readonly ILogger<PaymentStatusController> _logger;
        private readonly Core.Interfaces.Services.PaymentStatus.IPaymentStatusService _paymentStatusService;
        private readonly IUIActionService _uiActionService;
        private readonly IPublisherService _publisherService;
        private readonly List<PublisherConfigurationOption> _publisherOptions;
        private readonly IMapper _mapper;
        #endregion
        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="paymentStatusService"></param>
        /// <param name="uiActionService"></param>
        /// <param name="publisherService"></param>
        /// <param name="publisherOptions"></param>
        /// <param name="mapper"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public PaymentStatusController(
            ILogger<PaymentStatusController> logger,
            Core.Interfaces.Services.PaymentStatus.IPaymentStatusService paymentStatusService,
            IUIActionService uiActionService,
            IPublisherService publisherService,
            IOptions<List<PublisherConfigurationOption>> publisherOptions,
            IMapper mapper)
        {
            if (publisherOptions is null)
            {
                throw new ArgumentNullException(nameof(publisherOptions));
            }

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _paymentStatusService = paymentStatusService ?? throw new ArgumentNullException(nameof(paymentStatusService));
            _uiActionService = uiActionService ?? throw new ArgumentNullException(nameof(uiActionService));
            _publisherService = publisherService ?? throw new ArgumentNullException(nameof(publisherService));
            _publisherOptions = publisherOptions.Value;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        #endregion
        #region AddAsync
        /// <summary>
        /// Create new Payment status
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("v1/paymentstatuses")]
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
        /// Get Payment status Detail
        /// </summary>
        [HttpGet("v1/paymentstatuses/{paymentstatusId}")]
        public async Task<IActionResult> GetByIdAsync(Guid paymentstatusId)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetByIdAsync)}...");
                var result = await _paymentStatusService.GetByIdAsync(paymentstatusId);

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
        /// Remove Payment status
        /// </summary>
        [HttpDelete("v1/paymentstatuses/{paymentstatusId}")]
        public async Task<IActionResult> RemoveAsync(Guid paymentstatusId)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(RemoveAsync)}...");
                var data = await _paymentStatusService.RemoveAsync(paymentstatusId);

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
        /// Update Payment status Detail
        /// </summary>
        [HttpPut("v1/paymentstatuses/{paymentstatusId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync(Guid paymentstatusId, [FromBody] PaymentStatusUpdateRequestModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(UpdateAsync)}...");


                var data = await _paymentStatusService.UpdateAsync(paymentstatusId, request);

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

        #region UpdatePaymentMethod
        /// <summary>
        /// Send payment status to queue after payment
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("v1/paymentstatuses/paymentMethod")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePaymentMethod([FromBody] PaymentStatusUIRequestModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(UpdatePaymentMethod)}...");

                var result = await _uiActionService.UpdatePaymentMethod(request);

                if (result.Succeeded)
                {
                    var publisherOption = _publisherOptions.FirstOrDefault(x => x.PublisherTarget == ETC.Core.Models.Enums.PublisherTargetEnum.PaymentStatus);
                    RabbitMessageOutbound message = new RabbitMessageOutbound()
                    {
                        Message = JsonSerializer.Serialize(result.Data)
                    };

                    _publisherService.SendMessage(message, _mapper.Map<PublisherOptions>(publisherOption));
                }

                return Ok();
            }
            catch (Exception ex)
            {
                List<ValidationError> validationErrors = new();
                string errorMessage = $"An error occurred when calling {nameof(UpdatePaymentMethod)} method: {ex.Message}. InnerException : {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";

                _logger.LogError(errorMessage);
                validationErrors.Add(ValidationError.InternalServerError);

                return new ObjectResult(ValidationResult.Failed(errorMessage, validationErrors))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
        #endregion

        #region GetPaymentStatusHistoryAsync
        /// <summary>
        /// Get Payment status Detail
        /// </summary>
        [HttpGet("v1/paymentstatuses/history/{paymentId}")]
        public async Task<IActionResult> GetPaymentStatusHistoryAsync(Guid paymentId)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetPaymentStatusHistoryAsync)}...");
                var result = await _paymentStatusService.GetPaymentStatusHistoryAsync(paymentId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                List<ValidationError> validationErrors = new();
                string errorMessage = $"An error occurred when calling {nameof(GetPaymentStatusHistoryAsync)} method: {ex.Message}. InnerException : {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";
                _logger.LogError(errorMessage);
                validationErrors.Add(ValidationError.InternalServerError);
                return StatusCode(StatusCodes.Status500InternalServerError, ValidationResult.Failed(errorMessage, validationErrors));
            }
        }
        #endregion


    }
}
