using AutoMapper;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Authentication;
using EPAY.ETC.Core.API.Core.Models.Authentication;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Fusion;
using EPAY.ETC.Core.API.Infrastructure.Services.Fusion;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.UI;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AuthenticatedEmployeeModel = EPAY.ETC.Core.API.Core.Models.Authentication.AuthenticatedEmployeeModel;

namespace EPAY.ETC.Core.API.Infrastructure.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
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

        public async Task<ValidationResult<AuthenticatedEmployeeModel>> AuthenticateAsync(EmployeeLoginRequest input)
        {
            if (input.EmployeeId == "123456" && input.Username=="Admin" && input.Password == "password")
            {
                var authenticatedEmployee = new AuthenticatedEmployeeModel
                {
                    Id = Guid.Parse("4fd5cc23-0d90-451b-a748-5755376d635e"),
                    CreatedDate = DateTime.Now,
                    Action = LogonStatusEnum.Login,
                    EmployeeId = "123456",
                    Username = "Admin",
                    FirstName = "Khach",
                    LastName = "Hang",
                    JwtToken = "exampleJwtToken"
                };

                return ValidationResult.Success(authenticatedEmployee);
            }
            else
            {
                return ValidationResult.Failed<AuthenticatedEmployeeModel>(new List<ValidationError>()
                {
                    new ValidationError("Lỗi đăng nhập, vui lòng kiểm tra lại thông tin đăng nhập.", (int)HttpStatusCode.Unauthorized)
                });
            }
        }

        public async Task<ValidationResult<AuthenticatedEmployeeModel>> AutoAuthenticateAsync(EmployeeAutoLoginRequest request)
        {
            // Thực hiện tự động đăng nhập dựa trên thông tin từ request
            if (request.EmployeeId == "123456" && request.ActionCode == "Login")
            {
                var authenticatedEmployee = new AuthenticatedEmployeeModel
                {
                    Id = Guid.Parse("4fd5cc23-0d90-451b-a748-5755376d635e"),
                    CreatedDate = DateTime.Now,
                    Action = LogonStatusEnum.Login,
                    EmployeeId = "123456",
                    Username = "Admin",
                    FirstName = "Khach",
                    LastName = "Hang",
                    JwtToken = "exampleJwtToken"
                };

                return ValidationResult.Success(authenticatedEmployee);
            }
            else
            {
                return ValidationResult.Failed<AuthenticatedEmployeeModel>(new List<ValidationError>()
                {
                    new ValidationError("Lỗi đăng nhập, vui lòng đăng nhập lại thông tin.", (int)HttpStatusCode.Unauthorized)
                });
            }
        }
        #endregion

    }
}
