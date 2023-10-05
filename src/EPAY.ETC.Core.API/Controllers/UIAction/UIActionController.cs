using AutoMapper;
using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.UIActions;
using EPAY.ETC.Core.API.Core.Models.Vehicle.ReconcileVehicle;
using EPAY.ETC.Core.API.Models.Configs;
using EPAY.ETC.Core.API.Services;
using EPAY.ETC.Core.Models.Validation;
using EPAY.ETC.Core.Publisher.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nest;
using System.Text.Json;

namespace EPAY.ETC.Core.API.Controllers.UIAction
{
    [ApiController]
    [Route("~/api/[controller]")]
    public class UIActionController: ControllerBase
    {
        #region Variables
        private readonly ILogger<UIActionController> _logger;
        private readonly IUIActionService _uiActionService;
        private readonly IPublisherService _publisherService;
        private readonly List<PublisherConfigurationOption> _publisherOptions;
        private readonly IRabbitMQPublisherService _rabbitMQPublisherService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="uiActionService"></param>
        /// <param name="publisherService"></param>
        /// <param name="publisherOptions"></param>
        /// <param name="mapper"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public UIActionController(
            ILogger<UIActionController> logger,
            IUIActionService uiActionService,
            IPublisherService publisherService,
            IOptions<List<PublisherConfigurationOption>> publisherOptions,
            IMapper mapper, IRabbitMQPublisherService rabbitMQPublisherService)
        {
            if (publisherOptions is null)
            {
                throw new ArgumentNullException(nameof(publisherOptions));
            }

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _uiActionService = uiActionService ?? throw new ArgumentNullException(nameof(uiActionService));
            _publisherService = publisherService ?? throw new ArgumentNullException(nameof(publisherService));
            _publisherOptions = publisherOptions.Value;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _rabbitMQPublisherService = rabbitMQPublisherService;
        }
        #endregion

        #region ReconcileVehicleInfoAsync
        [HttpPost("v1/actions/reconcilevehicleinfo")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReconcileVehicleInfoAsync([FromBody] ReconcileVehicleInfoModel reconcileVehicleInfo)
        {
            List<ValidationError> validationErrors = new();

            try
            {
                _logger.LogInformation($"Executing {nameof(ReconcileVehicleInfoAsync)}...");

                var reconcileVehicleInfoResult = await _uiActionService.ReconcileVehicleInfoAsync(reconcileVehicleInfo);
                if (reconcileVehicleInfoResult.Succeeded && reconcileVehicleInfoResult.Data != null)
                {
                    _rabbitMQPublisherService.SendMessage(JsonSerializer.Serialize(reconcileVehicleInfoResult.Data), ETC.Core.Models.Enums.PublisherTargetEnum.Fee);
                    return Ok();
                }
                else
                {
                    validationErrors.Add(ValidationError.InternalServerError);
                    return StatusCode(StatusCodes.Status500InternalServerError, ValidationResult.Failed("", validationErrors));
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred when calling {nameof(ReconcileVehicleInfoAsync)} method: {ex.Message}. InnerException : {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";
                _logger.LogError(errorMessage);
                validationErrors.Add(ValidationError.InternalServerError);
                return StatusCode(StatusCodes.Status500InternalServerError, ValidationResult.Failed(errorMessage, validationErrors));
            }
        }
        #endregion
    }
}
