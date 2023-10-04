using AutoMapper;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Authentication;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Fusion;
using EPAY.ETC.Core.API.Infrastructure.Services.Fusion;
using EPAY.ETC.Core.Models.UI;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Infrastructure.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService<AuthenticatedEmployeeModel>
    {
        #region Variables   -
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IMapper _mapper;
        #endregion
        #region Constructor
        public AuthenticationService(ILogger<AuthenticationService> logger,IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<ValidationResult<AuthenticatedEmployeeModel>> AuthenticateAsync(string employeeId, string pwd)
        {
            if (employeeId == "123456" && pwd == "password") 
            {
                AuthenticatedEmployeeModel authenticatedEmployee = new AuthenticatedEmployeeModel
                {
                    EmployeeId = "123456",
                    Username = "User",
                    FirstName = "Khach",
                    LastName = "Hang",
                    JwtToken = "exampleJwtToken"
                };
                return Task.FromResult(ValidationResult.Success(authenticatedEmployee));
            }
            else
            {
                return Task.FromResult(ValidationResult.Failed<AuthenticatedEmployeeModel>(new List<ValidationError>()
                {
                    new ValidationError("Lỗi đăng nhập, vui lòng kiểm tra lại thông tin đăng nhập.", (int)HttpStatusCode.Unauthorized)
                }));
            }
        }

        public Task<ValidationResult<AuthenticatedEmployeeModel>> AutoAuthenticateAsync(string employeeId, string actionCode)
        {
            if (employeeId == "123456" && actionCode == "Login")
            {
                var authenticatedEmployee = new AuthenticatedEmployeeModel
                {
                    EmployeeId = "123456",
                    Username = "User",
                    FirstName = "Khach",
                    LastName = "Hang",
                    JwtToken = "exampleJwtToken"
                };
                return Task.FromResult(ValidationResult.Success(authenticatedEmployee));
            }
            else
            {
                return Task.FromResult(ValidationResult.Failed<AuthenticatedEmployeeModel>(new List<ValidationError>()
                {
                    new ValidationError("Lỗi đăng nhập, vui lòng đăng nhập lại thông tin.", (int)HttpStatusCode.Unauthorized)
                }));
            }
        }
        #endregion

    }
}
