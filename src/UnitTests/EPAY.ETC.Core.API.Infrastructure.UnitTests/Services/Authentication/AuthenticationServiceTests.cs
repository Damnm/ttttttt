using Azure.Core;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Authentication;
using EPAY.ETC.Core.API.Core.Models.Authentication;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Fees;
using EPAY.ETC.Core.API.Infrastructure.Services.Authentication;
using EPAY.ETC.Core.API.Infrastructure.Services.Fees;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Common;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Helpers;
using EPAY.ETC.Core.Models.Enums;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Services.Authentication
{
    public class AuthenticationServiceTests : AutoMapperTestBase
    {
        #region Init mock instance
        private readonly Exception _exception = null!;
        private readonly Mock<ILogger<AuthenticationService>> _loggerMock = new();
        private readonly IAuthenticationService _authenticationService;
        private readonly Mock<IAuthenticationService> _authenticationServiceMock = new();
        #endregion
        #region Init test data
        private AuthenticatedEmployeeModel request = new AuthenticatedEmployeeModel()
        {
            Id = Guid.Parse("4fd5cc23-0d90-451b-a748-5755376d635e"),
            CreatedDate = DateTime.Now,
            Action = LogonStatusEnum.Login,
            EmployeeId = "123456",
            Username = "User",
            FirstName = "Khach",
            LastName = "Hang",
            JwtToken = "exampleJwtToken"
        };
        private EmployeeLoginRequest login = new EmployeeLoginRequest()
        {
            EmployeeId = "123456",
            Password = "password"
        };
        #endregion
        [Fact]
        public async Task GivenValidRequest_WhenAddAsyncIsCalled_ThenAddedRecordSuccessfulAndReturnCorrectResult()
        {
            // Arrange

            // Act
            var service = new AuthenticationService(_loggerMock.Object, _mapper);
            var result = await service.AuthenticateAsync(login);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.EmployeeId.Should().Be("123456");
            result.Data.Username.Should().Be("User");
            result.Data.FirstName.Should().Be("Khach");
            result.Data.LastName.Should().Be("Hang");
            result.Data.JwtToken.Should().Be("exampleJwtToken");
            result.Data.Action.Should().Be(LogonStatusEnum.Login);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(_authenticationService.AuthenticateAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(_authenticationService.AuthenticateAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task AuthenticateAsync_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var invalidInput = new EmployeeLoginRequest
            {
                EmployeeId = "invalid_id",
                Password = "invalid_password"
            };

            // Act
            var result = await _authenticationService.AuthenticateAsync(invalidInput);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }

    }
}
