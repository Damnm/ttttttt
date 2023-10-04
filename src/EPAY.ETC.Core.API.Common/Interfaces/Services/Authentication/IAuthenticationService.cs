using EPAY.ETC.Core.Models.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.Authentication
{
    public interface IAuthenticationService<T>
    {
        Task<ValidationResult<T>> AuthenticateAsync(string employeeId, string pwd);
        Task<ValidationResult<T>> AutoAuthenticateAsync(string employeeId, string actionCode);
    }
}
