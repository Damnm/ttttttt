using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Fees;
using EPAY.ETC.Core.API.Core.Validation;
using EPAY.ETC.Core.Models.VehicleFee;
using Microsoft.AspNetCore.Mvc;

namespace EPAY.ETC.Core.API.Controllers.Fees
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("~/api/[controller]")]
    public class FeeCalculationController : ControllerBase
    {
        private readonly ILogger<FeeCalculationController> _logger;
        private readonly IFeeCalculationService _feeCalculationService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="feeCalculationService"></param>
        public FeeCalculationController(ILogger<FeeCalculationController> logger, IFeeCalculationService feeCalculationService)
        {
            _logger = logger;
            _feeCalculationService = feeCalculationService;
        }

        #region CalculateFeeAsync
        /// <summary>
        /// Fee calculation 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("v1/calculate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CalculateFeeAsync([FromBody] FeeCalculationRequestModel input)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(CalculateFeeAsync)}...");

                ValidationResult<VehicleFeeModel>? result;

                if (input.CustomVehicleType == null)
                    result = await _feeCalculationService.CalculateFeeAsync(input.RFID, input.PlateNumber, input.CheckInDateEpoch, input.CheckOutDateEpoch);
                else
                    result = await _feeCalculationService.CalculateFeeAsync(input.PlateNumber, input.CustomVehicleType, input.CheckInDateEpoch, input.CheckOutDateEpoch);

                return Ok(result);
            }
            catch (Exception ex)
            {
                List<ValidationError> validationErrors = new();
                string errorMessage = $"An error occurred when calling {nameof(CalculateFeeAsync)} method: {ex.Message}. InnerException : {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";
                _logger.LogError(errorMessage);
                validationErrors.Add(ValidationError.InternalServerError);
                return StatusCode(StatusCodes.Status500InternalServerError, ValidationResult.Failed(errorMessage, validationErrors));
            }
        }
        #endregion
    }
}
