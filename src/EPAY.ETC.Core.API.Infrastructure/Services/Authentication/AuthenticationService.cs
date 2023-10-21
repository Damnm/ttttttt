using EPAY.ETC.Core.API.Core.Interfaces.Services.Authentication;
using EPAY.ETC.Core.API.Core.Interfaces.Services.UIActions;
using EPAY.ETC.Core.API.Core.Models.Employees;
using EPAY.ETC.Core.API.Infrastructure.Models.Configs;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.UI;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace EPAY.ETC.Core.API.Infrastructure.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Variables   -
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IPasswordService _passwordService;
        private readonly IUIActionService _uIActionService;
        private readonly IOptions<JWTSettingsConfig> _jwtSettingsOption;
        #endregion

        #region Constructor
        public AuthenticationService(ILogger<AuthenticationService> logger, IPasswordService passwordService, IUIActionService uIActionService, IOptions<JWTSettingsConfig> jwtSettingsOption)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _passwordService = passwordService ?? throw new ArgumentNullException(nameof(passwordService));
            _uIActionService = uIActionService ?? throw new ArgumentNullException(nameof(uIActionService));
            _jwtSettingsOption = jwtSettingsOption ?? throw new ArgumentNullException(nameof(jwtSettingsOption));
        }

        public async Task<ValidationResult<AuthenticatedEmployeeResponseModel>> AuthenticateAsync(EmployeeLoginRequest input)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(AuthenticateAsync)} method...");

                var employees = await UseStreamReaderWithSystemTextJson();
                var employee = employees?.FirstOrDefault(x => x.Id == input.EmployeeId);

                // Thực hiện xác thực dựa trên thông tin từ input
                if (employee == null || !_passwordService.IsMatched(input.Password, employee.Password, employee.Salt).Data)
                    return ValidationResult.Failed<AuthenticatedEmployeeResponseModel>(new List<ValidationError>()
                    {
                        new ValidationError("Lỗi đăng nhập, vui lòng kiểm tra lại thông tin đăng nhập.", (int)HttpStatusCode.Unauthorized)
                    });

                //Generate jwt token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSettingsOption.Value.SecretKey);
                List<Claim> claimsIdentities = new List<Claim>(){
                    new Claim(ClaimTypes.NameIdentifier, input.EmployeeId.ToString()),
                    new Claim(ClaimTypes.Name,  employee.FullName.ToString()),
                    new Claim(ClaimTypes.Email, employee.Email == null ? "" : employee.Email.ToString())
                };

                DateTimeOffset expiredDate = DateTimeOffset.Now
                    .AddDays(_jwtSettingsOption.Value.ExpiresInDays)
                    .AddHours(_jwtSettingsOption.Value.ExpiresInHours)
                    .AddMinutes(_jwtSettingsOption.Value.ExpiresInMinutes);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claimsIdentities),
                    Expires = expiredDate.DateTime,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _jwtSettingsOption.Value.Issuer,
                    Audience = _jwtSettingsOption.Value.Audience,
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var authenticatedEmployee = new AuthenticatedEmployeeResponseModel
                {
                    EmployeeId = input.EmployeeId,
                    ExpiredAuthTokenEpoch = expiredDate.ToUnixTimeSeconds(),
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Username = employee.UserName,
                    JwtToken = tokenHandler.WriteToken(token),
                    AuthStatus = AuthStatusEnum.Loggedin
                };

                await _uIActionService.LoadCurrentUIAsync(authenticatedEmployee);

                return ValidationResult.Success(authenticatedEmployee);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(AuthenticateAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ValidationResult<AuthenticatedEmployeeResponseModel>> AutoAuthenticateAsync(EmployeeAutoLoginRequest request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(AutoAuthenticateAsync)} method...");

                var employees = await UseStreamReaderWithSystemTextJson();
                var employee = employees?.FirstOrDefault(x => x.Id == request.EmployeeId);

                // Thực hiện xác thực dựa trên thông tin từ input
                if (employee == null || request.ActionCode != LogonStatusEnum.Login.ToString())
                    return ValidationResult.Failed<AuthenticatedEmployeeResponseModel>(new List<ValidationError>()
                    {
                        new ValidationError("Lỗi đăng nhập, vui lòng kiểm tra lại thông tin đăng nhập.", (int)HttpStatusCode.Unauthorized)
                    });

                //Generate jwt token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSettingsOption.Value.SecretKey);
                List<Claim> claimsIdentities = new List<Claim>(){
                    new Claim(ClaimTypes.NameIdentifier, request.EmployeeId.ToString()),
                    new Claim(ClaimTypes.Name,  employee.FullName.ToString()),
                    new Claim(ClaimTypes.Email, employee.Email == null ? "" : employee.Email.ToString())
                };

                DateTimeOffset expiredDate = DateTimeOffset.Now
                    .AddDays(_jwtSettingsOption.Value.ExpiresInDays)
                    .AddHours(_jwtSettingsOption.Value.ExpiresInHours)
                    .AddMinutes(_jwtSettingsOption.Value.ExpiresInMinutes);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claimsIdentities),
                    Expires = DateTime.Now.AddDays(_jwtSettingsOption.Value.ExpiresInDays),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _jwtSettingsOption.Value.Issuer,
                    Audience = _jwtSettingsOption.Value.Audience,
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var authenticatedEmployee = new AuthenticatedEmployeeResponseModel
                {
                    EmployeeId = request.EmployeeId,
                    ExpiredAuthTokenEpoch = expiredDate.ToUnixTimeSeconds(),
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Username = employee.UserName,
                    JwtToken = tokenHandler.WriteToken(token),
                    AuthStatus = AuthStatusEnum.Loggedin
                };

                return ValidationResult.Success(authenticatedEmployee);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(AutoAuthenticateAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ValidationResult<UIModel?>> LogoutAsync()
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(LogoutAsync)} method...");

                var uiResult = await _uIActionService.LoadCurrentUIAsync();
                UIModel? uiModel = null;

                if (uiResult.Succeeded)
                {
                    uiModel = uiResult.Data;

                    if (uiModel != null)
                    {
                        if (uiModel.Command == null)
                            uiModel.Command = new ETC.Core.Models.UI.Command.CommandModel();
                        if (uiModel.Command.Logon == null)
                            uiModel.Command.Logon = new ETC.Core.Models.UI.Command.LogonModel();
                        uiModel.Command.Logon.Action = LogonStatusEnum.Logout;

                        uiModel.Authentication = new AuthenticatedEmployeeResponseModel()
                        {
                            AuthStatus = AuthStatusEnum.Loggedout
                        };

                        if (uiModel.Header == null)
                            uiModel.Header = new HeaderModel();
                        uiModel.Header.EmployeeName = string.Empty;

                        await _uIActionService.AddOrUpdateCurrentUIAsync(uiModel);
                    }
                }

                return ValidationResult.Success(uiModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(LogoutAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        #endregion

        #region Private method
        private async Task<List<EmployeeModel>?> UseStreamReaderWithSystemTextJson()
        {
            using StreamReader streamReader = new($"{AppDomain.CurrentDomain.BaseDirectory}/Persistence/StaticData/EmployeeData.json");
            var json = await streamReader.ReadToEndAsync();
            List<EmployeeModel>? etcs = JsonConvert.DeserializeObject<List<EmployeeModel>>(json);
            return etcs;
        }
        #endregion
    }
}
