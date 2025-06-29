﻿using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.ErrorResponse;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.AspNetCore.Mvc;

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
        /// <param name="errorResponseService"></param>
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

                var result = await _service.GetErrorResponseBySourceAync(source);

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
