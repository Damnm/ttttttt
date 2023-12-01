using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.UIActions;
using EPAY.ETC.Core.API.Services;
using EPAY.ETC.Core.Models.Devices;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EPAY.ETC.Core.API.Controllers.Devices
{
    /// <summary>
    /// Device control
    /// </summary>
    [ApiController]
    [Route("~/api/[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly ILogger<DeviceController> _logger;
        private readonly IUIActionService _uiActionService;
        private readonly IRabbitMQPublisherService _rabbitMQPublisherService;

        /// <summary>
        /// Constructure
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="uiActionService"></param>
        /// <param name="rabbitMQPublisherService"></param>
        public DeviceController(ILogger<DeviceController> logger,
                                IUIActionService uiActionService,
                                IRabbitMQPublisherService rabbitMQPublisherService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _uiActionService = uiActionService ?? throw new ArgumentNullException(nameof(uiActionService));
            _rabbitMQPublisherService = rabbitMQPublisherService;
        }

        #region ManipulateBarrier
        /// <summary>
        /// Save barrier status to redis and main DB
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("v1/barrier")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ManipulateBarrier([FromBody] BarrierRequestModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(ManipulateBarrier)}...");
                var result = await _uiActionService.ManipulateBarrierAsync(request);

                if (result.Succeeded)
                {
                    string message = string.Empty;

                    if (result.Data?.Payment != null)
                    {
                        message = JsonSerializer.Serialize(result.Data?.Payment);
                        _rabbitMQPublisherService.SendMessage(message, ETC.Core.Models.Enums.PublisherTargetEnum.Payment);
                    }

                    if (result.Data?.Fee != null)
                    {
                        message = JsonSerializer.Serialize(result.Data?.Fee);
                        _rabbitMQPublisherService.SendMessage(message, ETC.Core.Models.Enums.PublisherTargetEnum.Fee);
                    }

                    if (result.Data?.UI != null)
                    {
                        if (result.Data.UI.Header == null) result.Data.UI.Header = new ETC.Core.Models.UI.HeaderModel();
                        if (result.Data.UI.Header.Sender == null) result.Data.UI.Header.Sender = new ETC.Core.Models.UI.SenderModel();
                        result.Data.UI.Header.Sender.ModuleName = ETC.Core.Models.UI.ModuleNameEnum.Core_API;
                        result.Data.UI.Header.Sender.FunctionName = "In case: Endpoint v1/barrier";

                        message = JsonSerializer.Serialize(result.Data?.UI);
                        _rabbitMQPublisherService.SendMessage(message, ETC.Core.Models.Enums.PublisherTargetEnum.UI);
                    }

                    var barrierRequest = new BarrierModel()
                    {
                        Action = result.Data?.BarrierOpenStatus?.Status ?? ETC.Core.Models.Enums.BarrierActionEnum.Close
                    };

                    message = JsonSerializer.Serialize(barrierRequest);
                    _rabbitMQPublisherService.SendMessage(message, ETC.Core.Models.Enums.PublisherTargetEnum.Barrier);
                }

                return Ok(result);
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
