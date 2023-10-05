using AutoMapper;
using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Authentication;
using EPAY.ETC.Core.API.Core.Models.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace EPAY.ETC.Core.API.Controllers.Authentication
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("~/api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        #region Variables
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;
        #endregion
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="authenticationService"></param>
        /// <param name="mapper"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public AuthenticationController(
            ILogger<AuthenticationController> logger,
            IAuthenticationService authenticationService,
            IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        #endregion
        #region AddAsync
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("/api/Authentication/v1/employees/authenticate")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Authenticate([FromBody] EmployeeLoginRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request body.");
            }
            var validationResult = await _authenticationService.AuthenticateAsync(request);

            if (validationResult.Succeeded)
            {
                return Ok(validationResult.Data);
            }
            else
            {
                return BadRequest(validationResult.Errors);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("/api/Authentication/v1/employees/auto-authenticate")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AutoAuthenticate([FromBody] EmployeeAutoLoginRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request body.");
            }

            var validationResult = await _authenticationService.AutoAuthenticateAsync(request);

            if (validationResult.Succeeded)
            {
                return Ok(validationResult.Data);
            }
            else
            {
                return BadRequest(validationResult.Errors);
            }
            #endregion
        }
    }
}
