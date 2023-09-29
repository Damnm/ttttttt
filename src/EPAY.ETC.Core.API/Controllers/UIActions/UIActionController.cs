using AutoMapper;
using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.UIActions;
using EPAY.ETC.Core.API.Models.Configs;
using EPAY.ETC.Core.Models.Receipt.SessionReports;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using EPAY.ETC.Core.Publisher.Common.Options;
using EPAY.ETC.Core.Publisher.Interface;
using EPAY.ETC.Core.RabbitMQ.Common.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace EPAY.ETC.Core.API.Controllers.UIActions
{
    /// <summary>
    /// Control UI action
    /// </summary>
    [ApiController]
    [Route("~/api/[controller]")]
    public class UIActionController : ControllerBase
    {
        private readonly ILogger<UIActionController> _logger;
        private readonly IUIActionService _uiActionService;
        private readonly IPublisherService _publisherService;
        private readonly IMapper _mapper;
        private readonly List<PublisherConfigurationOption> _publisherOptions;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="uiActionService"></param>
        /// <param name="publisherService"></param>
        /// <param name="publisherOptions"></param>
        /// <param name="mapper"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public UIActionController(ILogger<UIActionController> logger, IUIActionService uiActionService, IPublisherService publisherService, IOptions<List<PublisherConfigurationOption>> publisherOptions, IMapper mapper)
        {
            if (publisherOptions is null)
            {
                throw new ArgumentNullException(nameof(publisherOptions));
            }

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _uiActionService = uiActionService ?? throw new ArgumentNullException(nameof(uiActionService));
            _publisherService = publisherService ?? throw new ArgumentNullException(nameof(publisherService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _publisherOptions = publisherOptions.Value;
        }

        #region PrintLaneSessionReport
        /// <summary>
        /// Print lane sesstion report
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("v1/printLaneSessionReport")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PrintLaneSessionReport([FromBody] SessionReportRequestModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(PrintLaneSessionReport)}...");

                var result = await _uiActionService.PrintLaneSessionReport(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                List<ValidationError> validationErrors = new();
                string errorMessage = $"An error occurred when calling {nameof(PrintLaneSessionReport)} method: {ex.Message}. InnerException : {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";

                _logger.LogError(errorMessage);
                validationErrors.Add(ValidationError.InternalServerError);

                return new ObjectResult(ValidationResult.Failed(errorMessage, validationErrors))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
        #endregion

        #region UpdatePaymentMethod
        /// <summary>
        /// Send payment status to queue after payment
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("v1/updatePaymentMethod")]
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

        #region ManipulateBarrier
        /// <summary>
        /// Save barrier status to redis and main DB
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("v1/manipulateBarrier")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ManipulateBarrier([FromBody] BarrierRequestModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(ManipulateBarrier)}...");

                var result = await _uiActionService.ManipulateBarrier(request);

                if (result.Succeeded)
                {
                    var publisherOption = _publisherOptions.FirstOrDefault(x => x.PublisherTarget == ETC.Core.Models.Enums.PublisherTargetEnum.Barrier);
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
                string errorMessage = $"An error occurred when calling {nameof(ManipulateBarrier)} method: {ex.Message}. InnerException : {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";

                _logger.LogError(errorMessage);
                validationErrors.Add(ValidationError.InternalServerError);

                return new ObjectResult(ValidationResult.Failed(errorMessage, validationErrors))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
        #endregion
    }
}
