using AutoMapper;
using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Barcode;
using EPAY.ETC.Core.API.Core.Models.Barcode;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Controllers.Barcode
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("~/api/[controller]")]
    public class BarcodeController: ControllerBase
    {
        #region Variables
        private readonly ILogger<BarcodeController> _logger;
        private readonly IBarcodeService _barcodeService;
        private readonly IMapper _mapper;
        #endregion
        #region Constructor
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="barcodeService"></param>
        /// <param name="mapper"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public BarcodeController(
            ILogger<BarcodeController> logger,
            IBarcodeService barcodeService,
            IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _barcodeService = barcodeService ?? throw new ArgumentNullException(nameof(barcodeService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        #endregion

        #region AddAsync
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("v1/barcodes")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddAsync([FromBody] BarcodeAddOrUpdateRequestModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(AddAsync)}...");


                var barcodeResult = await _barcodeService.AddAsync(request);

                if (!barcodeResult.Succeeded && barcodeResult.Errors.Any(x => x.Code == StatusCodes.Status409Conflict))
                {
                    return Conflict(barcodeResult);
                }

                return new ObjectResult(barcodeResult)
                {
                    StatusCode = StatusCodes.Status201Created
                };
            }
            catch (Exception ex)
            {
                List<ValidationError> validationErrors = new();
                string errorMessage = $"An error occurred when calling {nameof(AddAsync)} method: {ex.Message}. InnerException : {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";
                _logger.LogError(errorMessage);
                validationErrors.Add(ValidationError.InternalServerError);
                return StatusCode(StatusCodes.Status500InternalServerError, ValidationResult.Failed(errorMessage, validationErrors));
            }
        }
        #endregion
        #region GetByIdAsync
        /// <summary>
        /// Get Payment status Detail
        /// </summary>
        [HttpGet("v1/barcodes/{barcodeId}")]
        public async Task<IActionResult> GetByIdAsync(string barcodeId)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetByIdAsync)}...");
                Guid.TryParse(barcodeId, out var guid);
                var result = await _barcodeService.GetByIdAsync(guid);

                return Ok(result);
            }
            catch (Exception ex)
            {
                List<ValidationError> validationErrors = new();
                string errorMessage = $"An error occurred when calling {nameof(GetByIdAsync)} method: {ex.Message}. InnerException : {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";
                _logger.LogError(errorMessage);
                validationErrors.Add(ValidationError.InternalServerError);
                return StatusCode(StatusCodes.Status500InternalServerError, ValidationResult.Failed(errorMessage, validationErrors));
            }
        }
        #endregion
        #region RemoveAsync
        /// <summary>
        /// Remove Payment status
        /// </summary>
        [HttpDelete("v1/barcodes/{barcodeId}")]
        public async Task<IActionResult> RemoveAsync(string barcodeId)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(RemoveAsync)}...");
                Guid.TryParse(barcodeId, out var guid);
                var data = await _barcodeService.RemoveAsync(guid);

                if (!data.Succeeded && data.Errors.Any(x => x.Code == StatusCodes.Status404NotFound))
                {
                    return NotFound(data);
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                List<ValidationError> validationErrors = new();
                string errorMessage = $"An error occurred when calling {nameof(RemoveAsync)} method: {ex.Message}. InnerException : {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";
                _logger.LogError(errorMessage);
                validationErrors.Add(ValidationError.InternalServerError);
                return StatusCode(StatusCodes.Status500InternalServerError, ValidationResult.Failed(errorMessage, validationErrors));
            }
        }
        #endregion
        #region UpdateAsync
        /// <summary>
        /// Update Payment status Detail
        /// </summary>
        [HttpPut("v1/barcodes/{barcodeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync(string barcodeId, [FromBody] BarcodeAddOrUpdateRequestModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(UpdateAsync)}...");

                Guid.TryParse(barcodeId, out var guid);
                var data = await _barcodeService.UpdateAsync(guid, request);

                if (!data.Succeeded)
                {
                    if (data.Errors.Any(x => x.Code == StatusCodes.Status404NotFound))
                    {
                        return NotFound(data);
                    }
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                List<ValidationError> validationErrors = new();
                string errorMessage = $"An error occurred when calling {nameof(UpdateAsync)} method: {ex.Message}. InnerException : {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";
                _logger.LogError(errorMessage);
                validationErrors.Add(ValidationError.InternalServerError);
                return StatusCode(StatusCodes.Status500InternalServerError, ValidationResult.Failed(errorMessage, validationErrors));
            }
        }
        #endregion
        #region UpdateAsync
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("v1/barcodes/filter")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetListAsync([FromBody] BarcodeFilterRequestModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetListAsync)}...");

                Expression<Func<BarcodeModel, bool>> expression = s =>
                (!string.IsNullOrEmpty(request.EmployeeId) ? !string.IsNullOrEmpty(s.EmployeeId) && s.EmployeeId.Equals(request.EmployeeId) : true)
                && (!string.IsNullOrEmpty(request.ActionCode) ? !string.IsNullOrEmpty(s.ActionCode) && s.ActionCode.Equals(request.ActionCode) : true)
                && (request.BarcodeAction != null ? s.BarcodeAction == request.BarcodeAction : true);

                var barcodeResult = await _barcodeService.GetListAsync(expression);

                return Ok(barcodeResult);
            }
            catch (Exception ex)
            {
                List<ValidationError> validationErrors = new();
                string errorMessage = $"An error occurred when calling {nameof(GetListAsync)} method: {ex.Message}. InnerException : {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}";
                _logger.LogError(errorMessage);
                validationErrors.Add(ValidationError.InternalServerError);
                return StatusCode(StatusCodes.Status500InternalServerError, ValidationResult.Failed(errorMessage, validationErrors));
            }
        }
        #endregion
    }
}
