using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Transaction;
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
    public class LaneInRFIDTransactionLogController : ControllerBase
    {

        #region Variables
        private readonly ILogger<LaneInRFIDTransactionLogController> _logger;
        private readonly ILaneInRFIDTransactionLogService _laneInRFIDTransactionLogService;
        #endregion
        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="laneInRFIDTransactionLogService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public LaneInRFIDTransactionLogController(ILogger<LaneInRFIDTransactionLogController> logger,
            ILaneInRFIDTransactionLogService laneInRFIDTransactionLogService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _laneInRFIDTransactionLogService = laneInRFIDTransactionLogService ?? throw new ArgumentNullException(nameof(laneInRFIDTransactionLogService));
        }
        #endregion
        #region AddAsync
        /// <summary>
        /// Create new Payment
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("v1/LaneInRFIDTransactionLogs")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrUpdateAsync([FromBody] LaneInRFIDTransactionLogRequestModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(AddOrUpdateAsync)}...");


                var paymentStatusResult = await _laneInRFIDTransactionLogService.AddOrUpdateAsync(request.Id, request);

                return new ObjectResult(paymentStatusResult)
                {
                    StatusCode = StatusCodes.Status201Created
                };
            }
            catch (Exception ex)
            {
                List<ValidationError> validationErrors = new();
                string errorMessage = $"An error occurred when calling {nameof(AddOrUpdateAsync)} method: {ex.Message}. InnerException : {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";
                _logger.LogError(errorMessage);
                validationErrors.Add(ValidationError.InternalServerError);
                return StatusCode(StatusCodes.Status500InternalServerError, ValidationResult.Failed(errorMessage, validationErrors));
            }
        }
        #endregion
    }
}
