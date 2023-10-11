using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Fees;
using EPAY.ETC.Core.API.Core.Interfaces.Services.UIActions;
using EPAY.ETC.Core.API.Core.Models.Vehicle.ReconcileVehicle;
using EPAY.ETC.Core.API.Models.Configs;
using EPAY.ETC.Core.API.Services;
using EPAY.ETC.Core.Models.Fees;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace EPAY.ETC.Core.API.Controllers.Fees
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("~/api/[controller]")]
    public class FeeController : ControllerBase
    {
        private readonly ILogger<FeeController> _logger;
        private readonly IFeeService _service;
        private readonly IUIActionService _uiActionService;
        private readonly IRabbitMQPublisherService _rabbitMQPublisherService;
        private readonly List<PublisherConfigurationOption> _publisherOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="service"></param>
        /// <param name="uiActionService"></param>
        /// <param name="rabbitMQPublisherService"></param>
        /// <param name="publisherOptions"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public FeeController(ILogger<FeeController> logger,
                             IFeeService service,
                             IUIActionService uiActionService,
                             IRabbitMQPublisherService rabbitMQPublisherService,
                             IOptions<List<PublisherConfigurationOption>> publisherOptions)
        {
            if (publisherOptions is null)
            {
                throw new ArgumentNullException(nameof(publisherOptions));
            }

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _uiActionService = uiActionService ?? throw new ArgumentNullException(nameof(uiActionService));
            _rabbitMQPublisherService = rabbitMQPublisherService ?? throw new ArgumentNullException(nameof(rabbitMQPublisherService));
            _publisherOptions = publisherOptions.Value;
        }

        #region AddAsync
        /// <summary>
        /// Adding new fee object
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("v1/fees")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddAsync(FeeModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(AddAsync)}...");

                var result = await _service.AddAsync(request);

                if (!result.Succeeded && result.Errors.Any(x => x.Code == StatusCodes.Status409Conflict))
                {
                    return Conflict(result);
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
        #endregion

        #region UpdateAsync
        /// <summary>
        /// Update fee object
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("v1/fees/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync(Guid id, FeeModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(UpdateAsync)}...");

                var result = await _service.UpdateAsync(id, request);

                if (!result.Succeeded)
                {
                    if (result.Errors.Any(x => x.Code == StatusCodes.Status404NotFound))
                        return NotFound(result);

                    if (result.Errors.Any(x => x.Code == StatusCodes.Status409Conflict))
                        return Conflict(result);
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
        #endregion

        #region RemoveAsync
        /// <summary>
        /// Delete one fee object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("v1/fees/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveAsync(Guid id)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(RemoveAsync)}...");

                var result = await _service.RemoveAsync(id);

                if (!result.Succeeded)
                {
                    if (result.Errors.Any(x => x.Code == StatusCodes.Status404NotFound))
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
        #endregion

        #region GetByIdAsync
        /// <summary>
        /// Get one fee object by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("v1/fees/{id}")]
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
        #endregion

        #region GetByObjectIdAsync
        /// <summary>
        /// Get one fee object by Id
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        [HttpGet("v1/fees/objectId/{objectId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByObjectIdAsync(string objectId)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetByObjectIdAsync)}...");

                var result = await _service.GetByObjectIdAsync(objectId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                List<ValidationError> validationErrors = new();
                string errorMessage = $"An error occurred when calling {nameof(GetByObjectIdAsync)} method: {ex.Message}. InnerException : {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";

                _logger.LogError(errorMessage);
                validationErrors.Add(ValidationError.InternalServerError);

                return new ObjectResult(ValidationResult.Failed(errorMessage, validationErrors))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
        #endregion

        #region GetAllAsync
        /// <summary>
        /// Get all fee object
        /// </summary>
        /// <returns></returns>
        [HttpGet("v1/fees")]
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
        #endregion

        #region FindVehicleAsync

        [HttpGet("v1/vehicles/{rfidOrNumberPlate}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FindVehicleAsync(string rfidOrNumberPlate)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(FindVehicleAsync)}...");
                var result = await _service.FindVehicleAsync(rfidOrNumberPlate);

                return Ok(result);
            }
            catch (Exception ex)
            {
                List<ValidationError> validationErrors = new();
                string errorMessage = $"An error occurred when calling {nameof(FindVehicleAsync)} method: {ex.Message}. InnerException : {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";

                _logger.LogError(errorMessage);
                validationErrors.Add(ValidationError.InternalServerError);

                return new ObjectResult(ValidationResult.Failed(errorMessage, validationErrors))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
        #endregion

        #region ReconcileVehicleInfoAsync
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reconcileVehicleInfo"></param>
        /// <returns></returns>
        [HttpPost("v1/vehicles")]
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
                    if (reconcileVehicleInfoResult.Data.Fee != null)
                        _rabbitMQPublisherService.SendMessage(JsonSerializer.Serialize(reconcileVehicleInfoResult.Data.Fee), ETC.Core.Models.Enums.PublisherTargetEnum.Fee);
                    
                    if (reconcileVehicleInfoResult.Data.PaymentStatus != null)
                        _rabbitMQPublisherService.SendMessage(JsonSerializer.Serialize(reconcileVehicleInfoResult.Data.PaymentStatus), ETC.Core.Models.Enums.PublisherTargetEnum.PaymentStatus);

                    return Ok(ValidationResult.Success<string?>(null));
                }
                else
                {
                    validationErrors.Add(ValidationError.InternalServerError);
                    return StatusCode(StatusCodes.Status500InternalServerError, ValidationResult.Failed<string?>(validationErrors));
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
