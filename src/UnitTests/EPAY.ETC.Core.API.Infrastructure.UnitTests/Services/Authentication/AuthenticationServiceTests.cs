using EPAY.ETC.Core.API.Core.Interfaces.Services.Authentication;
using EPAY.ETC.Core.API.Infrastructure.Models.Configs;
using EPAY.ETC.Core.API.Infrastructure.Services.Authentication;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Common;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Helpers;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.UI;
using EPAY.ETC.Core.Models.Validation;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Services.Authentication
{
    public class AuthenticationServiceTests : AutoMapperTestBase
    {
        #region Init mock instance
        private readonly Exception _exception = null!;
        private readonly Mock<ILogger<AuthenticationService>> _loggerMock = new();
        private readonly Mock<IPasswordService> _passwordServiceMock = new();
        private readonly IOptions<JWTSettingsConfig> jwtSettingsOption = Options.Create(new JWTSettingsConfig()
        {
            Audience = "Some",
            ExpiresInDays = 1,
            ExpiresInHours = 1,
            ExpiresInMinutes = 1,
            Issuer = "Some",
            SecretKey = "L%PdPk!F7lZ0pd6s2Mi32G#ib%wdo*Lm"
        });
        #endregion
        #region Init test data
        private AuthenticatedEmployeeResponseModel response = new AuthenticatedEmployeeResponseModel()
        {
            EmployeeId = "030001",
            Username = "030001",
            FirstName = "Lưu Trần",
            LastName = "Anh Tuấn"
        };
        private EmployeeLoginRequest login = new EmployeeLoginRequest()
        {
            EmployeeId = "030001",
            Password = "Abc@123"
        };
        private EmployeeAutoLoginRequest autoLogin = new EmployeeAutoLoginRequest()
        {
            EmployeeId = "123456",
            ActionCode = "Login",
        };
        #endregion

        #region AuthenticateAsync
        [Fact]
        public async Task GivenValidRequest_WhenAuthenticateAsyncIsCalled_ThenAuthorizedAndReturnCorrectResult()
        {
            // Arrange
            _passwordServiceMock.Setup(x => x.IsMatched(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(ValidationResult.Success(true));

            // Act
            var service = new AuthenticationService(_loggerMock.Object, _passwordServiceMock.Object, jwtSettingsOption);
            var result = await service.AuthenticateAsync(login);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data?.EmployeeId.Should().Be(response.EmployeeId);
            result.Data?.Username.Should().Be(response.Username);
            result.Data?.FirstName.Should().Be(response.FirstName);
            result.Data?.LastName.Should().Be(response.LastName);
            result.Data?.JwtToken.Should().NotBeEmpty();

            _passwordServiceMock.Verify(x => x.IsMatched(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.AuthenticateAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.AuthenticateAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndPasswordIsInValidOrEmployeeIdIsInValid_WhenAuthenticateAsyncIsCalled_ThenReturnUnauthorize()
        {
            // Arrange
            _passwordServiceMock.Setup(x => x.IsMatched(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(ValidationResult.Success(false));

            // Act
            var service = new AuthenticationService(_loggerMock.Object, _passwordServiceMock.Object, jwtSettingsOption);
            var result = await service.AuthenticateAsync(login);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();

            _passwordServiceMock.Verify(x => x.IsMatched(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.AuthenticateAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.AuthenticateAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestPasswordServiceIsDown_WhenAuthenticateAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var someEx = new Exception("Some ex");
            _passwordServiceMock.Setup(x => x.IsMatched(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws(someEx);

            // Act
            var service = new AuthenticationService(_loggerMock.Object, _passwordServiceMock.Object, jwtSettingsOption);
            Func<Task> func = async () => await service.AuthenticateAsync(login);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => func());
            ex.Should().NotBeNull();
            ex.Should().BeEquivalentTo(someEx);

            _passwordServiceMock.Verify(x => x.IsMatched(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.AuthenticateAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.AuthenticateAsync)} method", Times.Once, _exception);
        }
        #endregion

        #region AutoAuthenticateAsync
        [Fact]
        public async Task GivenValidRequest_WhenAutoAuthenticateAsyncIsCalled_ThenAuthorizedAndReturnCorrectResult()
        {
            // Arrange
            EmployeeAutoLoginRequest login = new EmployeeAutoLoginRequest()
            {
                EmployeeId = "030001",
                ActionCode = LogonStatusEnum.Login.ToString()
            };

            // Act
            var service = new AuthenticationService(_loggerMock.Object, _passwordServiceMock.Object, jwtSettingsOption);
            var result = await service.AutoAuthenticateAsync(login);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data?.EmployeeId.Should().Be(response.EmployeeId);
            result.Data?.Username.Should().Be(response.Username);
            result.Data?.FirstName.Should().Be(response.FirstName);
            result.Data?.LastName.Should().Be(response.LastName);
            result.Data?.JwtToken.Should().NotBeEmpty();

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.AutoAuthenticateAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.AutoAuthenticateAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenValidRequestAndEmployeeIdIsInValid_WhenAutoAuthenticateAsyncIsCalled_ThenReturnUnauthorize()
        {
            // Arrange
            EmployeeAutoLoginRequest login = new EmployeeAutoLoginRequest()
            {
                EmployeeId = "030001",
                ActionCode = "asdasd"
            };

            // Act
            var service = new AuthenticationService(_loggerMock.Object, _passwordServiceMock.Object, jwtSettingsOption);
            var result = await service.AutoAuthenticateAsync(login);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.AutoAuthenticateAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.AutoAuthenticateAsync)} method", Times.Never, _exception);
        }
        #endregion
    }
}
