using EPAY.ETC.Core.API.Controllers.Payment;
using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Transaction;
using EPAY.ETC.Core.API.Infrastructure.Services.TransactionLog;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.AspNetCore.Mvc;

namespace EPAY.ETC.Core.API.Controllers.TransactionLog
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("~/api/[controller]")]
    public class LaneInCameraTransactionLogController : ControllerBase
    {

        #region Variables
        private readonly ILogger<LaneInCameraTransactionLogController> _logger;
        private readonly ILaneInCameraTransactionLogService _laneInCameraTransactionLogService;
        #endregion
        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="paymentStatusService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public LaneInCameraTransactionLogController(ILogger<LaneInCameraTransactionLogController> logger,
            ILaneInCameraTransactionLogService laneInCameraTransactionLogService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _laneInCameraTransactionLogService = laneInCameraTransactionLogService ?? throw new ArgumentNullException(nameof(laneInCameraTransactionLogService));
        }
        #endregion
        #region AddAsync
        /// <summary>
        /// Create new Payment
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("v1/LaneInCameraTransactionLogs")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateInsertAsync([FromBody] LaneInCameraTransactionLogRequest request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(UpdateInsertAsync)}...");


                var paymentStatusResult = await _laneInCameraTransactionLogService.UpdateInsertAsync(request);

                return new ObjectResult(paymentStatusResult)
                {
                    StatusCode = StatusCodes.Status201Created
                };
            }
            catch (Exception ex)
            {
                List<ValidationError> validationErrors = new();
                string errorMessage = $"An error occurred when calling {nameof(UpdateInsertAsync)} method: {ex.Message}. InnerException : {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";
                _logger.LogError(errorMessage);
                validationErrors.Add(ValidationError.InternalServerError);
                return StatusCode(StatusCodes.Status500InternalServerError, ValidationResult.Failed(errorMessage, validationErrors));
            }
        }
        #endregion
    }
}
