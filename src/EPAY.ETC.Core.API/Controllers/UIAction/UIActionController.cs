using AutoMapper;
using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.UIActions;
using EPAY.ETC.Core.API.Models.Configs;
using EPAY.ETC.Core.API.Services;
using EPAY.ETC.Core.Models.Devices;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.Validation;
using EPAY.ETC.Core.Publisher.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace EPAY.ETC.Core.API.Controllers.UIAction
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("~/api/[controller]")]
    public class UIActionController : ControllerBase
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
        /// <param name="rabbitMQPublisherService"></param>
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

        #region LoadCurrentUIAsync
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("v1/ui")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadCurrentUIAsync()
        {
            List<ValidationError> validationErrors = new();

            try
            {
                _logger.LogInformation($"Executing {nameof(LoadCurrentUIAsync)}...");

                var result = await _uiActionService.LoadCurrentUIAsync();
                if (result.Succeeded && result.Data != null)
                {
                    return Ok(result);
                }
                else
                {
                    validationErrors.Add(ValidationError.InternalServerError);
                    return StatusCode(StatusCodes.Status500InternalServerError, ValidationResult.Failed("", validationErrors));
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred when calling {nameof(LoadCurrentUIAsync)} method: {ex.Message}. InnerException : {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";
                _logger.LogError(errorMessage);
                validationErrors.Add(ValidationError.InternalServerError);
                return StatusCode(StatusCodes.Status500InternalServerError, ValidationResult.Failed(errorMessage, validationErrors));
            }
        }
        #endregion
        #region RemoveCurrentTransaction
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        [HttpPost("v1/add-remove-transaction")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrRemoveCurrentTransaction(ActionEnum? action)
        {
            List<ValidationError> validationErrors = new();

            try
            {
                _logger.LogInformation($"Executing {nameof(AddOrRemoveCurrentTransaction)}...");

                if (action == null)
                    return BadRequest("Action is required");

                FusionStatusModel fusionStatus;

                switch (action)
                {
                    case ActionEnum.Insert:
                        fusionStatus = new FusionStatusModel()
                        {
                            ActionEnum = action.Value,
                            ObjectId = Guid.NewGuid()
                        };
                        _rabbitMQPublisherService.SendMessage(JsonSerializer.Serialize(fusionStatus), PublisherTargetEnum.FusionStatus);
                        break;

                    case ActionEnum.Delete:
                        var result = await _uiActionService.GetFeeProcessing();

                        if (!string.IsNullOrEmpty(result))
                        {
                            fusionStatus = new FusionStatusModel()
                            {
                                ActionEnum = action.Value,
                                ObjectId = Guid.TryParse(result, out Guid guidValue) ? guidValue : Guid.NewGuid()
                            };
                            _rabbitMQPublisherService.SendMessage(JsonSerializer.Serialize(fusionStatus), PublisherTargetEnum.FusionStatus);
                        }
                        break;
                }

                return Ok();
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred when calling {nameof(AddOrRemoveCurrentTransaction)} method: {ex.Message}. InnerException : {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";
                _logger.LogError(errorMessage);
                validationErrors.Add(ValidationError.InternalServerError);
                return StatusCode(StatusCodes.Status500InternalServerError, ValidationResult.Failed(errorMessage, validationErrors));
            }
        }
        #endregion
    }
}
