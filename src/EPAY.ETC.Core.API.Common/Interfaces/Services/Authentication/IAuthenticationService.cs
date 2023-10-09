using EPAY.ETC.Core.API.Core.Models.Authentication;
using EPAY.ETC.Core.Models.UI;
using EPAY.ETC.Core.Models.Validation;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<ValidationResult<AuthenticatedEmployeeResponseModel>> AuthenticateAsync(EmployeeLoginRequest input);
        Task<ValidationResult<AuthenticatedEmployeeResponseModel>> AutoAuthenticateAsync(EmployeeAutoLoginRequest request);
    }
}
