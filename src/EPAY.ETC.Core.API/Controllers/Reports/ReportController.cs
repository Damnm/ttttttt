using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.UIActions;
using EPAY.ETC.Core.Models.Receipt.SessionReports;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.AspNetCore.Mvc;

namespace EPAY.ETC.Core.API.Controllers.Reports
{
    /// <summary>
    /// Report control
    /// </summary>
    [ApiController]
    [Route("~/api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly ILogger<ReportController> _logger;
        private readonly IUIActionService _uiActionService;

        /// <summary>
        /// Constructure Report control
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="uIActionService"></param>
        public ReportController(ILogger<ReportController> logger, IUIActionService uIActionService)
        {
            _logger = logger;
            _uiActionService = uIActionService;
        }

        #region PrintLaneSessionReport
        /// <summary>
        /// Print lane sesstion report
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("/api/Report/v1/reports/lane-session")]
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
