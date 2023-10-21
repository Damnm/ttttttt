using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.UIActions;
using EPAY.ETC.Core.API.Core.Utils;
using EPAY.ETC.Core.API.Services;
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
                var result = await _uiActionService.ManipulateBarrier(request);

                if (result.Succeeded)
                {
                    string message = JsonSerializer.Serialize(new { result.Data?.Limit, Status = result.Data?.Status.ToString() });
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
