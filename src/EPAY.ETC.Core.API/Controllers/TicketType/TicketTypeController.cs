using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.TicketType;
using EPAY.ETC.Core.API.Services;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.AspNetCore.Mvc;

namespace EPAY.ETC.Core.API.Controllers.TicketType
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("~/api/[controller]")]
    public class TicketTypeController : ControllerBase
    {
        #region Variables
        private readonly ILogger<TicketTypeController> _logger;
        private readonly ITicketTypeService _ticketTypeService;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="printLogService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public TicketTypeController(ILogger<TicketTypeController> logger,
            ITicketTypeService printLogService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _ticketTypeService = printLogService ?? throw new ArgumentNullException(nameof(printLogService));
        }

        #endregion

        #region GetByCodeAsync
        /// <summary>
        /// Get Vehicle Detail
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("v1/tickettypes/{code}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByCodeAsync(string code)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetByCodeAsync)}...");

                var codeResponse = await _ticketTypeService.GetByCodeAsync(code);

                return Ok(codeResponse);
            }
            catch (Exception ex)
            {
                List<ValidationError> validationErrors = new();
                string errorMessage = $"An error occurred when calling {nameof(GetByCodeAsync)} method: {ex.Message}. InnerException : {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";
                _logger.LogError(errorMessage);
                validationErrors.Add(ValidationError.InternalServerError);
                return StatusCode(StatusCodes.Status500InternalServerError, ValidationResult.Failed(errorMessage, validationErrors));
            }
        }
        #endregion
    }
}
