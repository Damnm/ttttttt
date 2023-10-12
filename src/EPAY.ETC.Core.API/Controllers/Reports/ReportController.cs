using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.UIActions;
using EPAY.ETC.Core.API.Services;
using EPAY.ETC.Core.Models.Receipt.SessionReports;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
        private readonly IRabbitMQPublisherService _rabbitMQPublisherService;

        /// <summary>
        /// Constructure Report control
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="uIActionService"></param>
        /// <param name="rabbitMQPublisherService"></param>
        public ReportController(ILogger<ReportController> logger,
                                IUIActionService uIActionService,
                                IRabbitMQPublisherService rabbitMQPublisherService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _uiActionService = uIActionService ?? throw new ArgumentNullException(nameof(uIActionService));
            _rabbitMQPublisherService = rabbitMQPublisherService ?? throw new ArgumentNullException(nameof(rabbitMQPublisherService));
        }

        #region PrintLaneSessionReport
        /// <summary>
        /// Print lane sesstion report
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("v1/reports/lane-session")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PrintLaneSessionReport([FromBody] LaneSessionReportRequestModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(PrintLaneSessionReport)}...");

                var result = await _uiActionService.PrintLaneSessionReport(request);

                if (result.Succeeded)
                {
                    _rabbitMQPublisherService.SendMessage(JsonSerializer.Serialize(result.Data), ETC.Core.Models.Enums.PublisherTargetEnum.Printer);
                }
                
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
