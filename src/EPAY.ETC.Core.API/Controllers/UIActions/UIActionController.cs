using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.UIActions;
using EPAY.ETC.Core.Models.Receipt.SessionReports;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.AspNetCore.Mvc;

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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="uiActionService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public UIActionController(ILogger<UIActionController> logger, IUIActionService uiActionService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _uiActionService = uiActionService ?? throw new ArgumentNullException(nameof(uiActionService));
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
    }
}
