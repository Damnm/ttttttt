using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.ErrorResponse;
using EPAY.ETC.Core.API.Core.Interfaces.Services.ETCCheckouts;
using EPAY.ETC.Core.API.Core.Models.Barcode;
using EPAY.ETC.Core.API.Core.Models.ErrorResponse;
using EPAY.ETC.Core.API.Core.Models.ETCCheckOuts;
using EPAY.ETC.Core.API.Infrastructure.Services.ErrorResponse;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Controllers.ETCCheckouts
{
    /// <summary>
    /// API CRUD for ETC checkout
    /// </summary>
    [ApiController]
    [Route("~/api/[controller]")]
    public class ErrorResponseController : ControllerBase
    {
        private readonly ILogger<ErrorResponseController> _logger;
        private readonly IErrorResponseService _service;

        /// <summary>
        /// Initial constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="etcCheckOutService"></param>
        public ErrorResponseController(ILogger<ErrorResponseController> logger, IErrorResponseService errorResponseService)
        {
            _logger = logger;
            _service = errorResponseService;
        }

        /// <summary>
        /// Get all ECT checkout
        /// </summary>
        /// <returns></returns>
        [HttpGet("v1/ErrorResponses/{source}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync(string source)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetAllAsync)}...");

                Expression<Func<ErrorResponseModel, bool>> expression = s => source != null ? s.Source == source : true;

                var result = await _service.GetAllAsync(expression);

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

    }
}
