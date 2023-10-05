using EPAY.ETC.Core.API.Core.Models.Authentication;
using EPAY.ETC.Core.Models.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<ValidationResult<AuthenticatedEmployeeModel>> AuthenticateAsync(EmployeeLoginRequest input);
        Task<ValidationResult<AuthenticatedEmployeeModel>> AutoAuthenticateAsync(EmployeeAutoLoginRequest request);
    }
}
