using EPAY.ETC.Core.API.Infrastructure.Services.Authentication;
using EPAY.ETC.Core.API.IntegrationTests.Common;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.UI;
using Microsoft.Extensions.Logging;
using Moq;
using XUnitPriorityOrderer;

namespace EPAY.ETC.Core.API.IntegrationTests.Services.Authentication
{
    [TestCaseOrderer(CasePriorityOrderer.TypeName, CasePriorityOrderer.AssembyName)]
    public class AuthenticationServiceTests : IntegrationTestBase
    {
        #region Init mock instance
        private readonly Exception _exception = null!;
        private readonly Mock<ILogger<AuthenticationService>> _loggerMock = new();
        private readonly Mock<AuthenticationService> _authenticationServiceMock = new();
        #endregion
        #region Init test data
        private AuthenticatedEmployeeResponseModel request = new AuthenticatedEmployeeResponseModel()
        {
            EmployeeId = "030001",
            Username = "030001",
            FirstName = "Lưu Trần",
            LastName = "Anh Tuấn"
        };
        private EmployeeLoginRequest login = new EmployeeLoginRequest()
        {
            EmployeeId = "123456",
            Password = "password"
        };
        private EmployeeAutoLoginRequest autoLogin = new EmployeeAutoLoginRequest()
        {
            EmployeeId = "123456",
            ActionCode = "Login",
        };
        #endregion
        //#region AuthenticationService
        //[Fact]
        //public async Task GivenValidRequest_WhenAddAsyncIsCalled_ThenAddedRecordSuccessfulAndReturnCorrectResult()
        //{
        //    // Arrange

        //    // Act
        //    var service = new AuthenticationService(_loggerMock.Object, _mapper);
        //    var result = await service.AuthenticateAsync(login);

        //    // Assert
        //    result.Should().NotBeNull();
        //    result.Data.Should().NotBeNull();
        //    result.Succeeded.Should().BeTrue();
        //    _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.AuthenticateAsync)} method", Times.Never, _exception);
        //}

        //[Fact]
        //public async Task GivenRequestIsValidAndEmployeeIdIsNotCorrect_WhenAsyncIsCalled_ThenThrowException()
        //{
        //    // Arrange
        //    var input = new EmployeeLoginRequest
        //    {
        //        EmployeeId = "id",
        //        Password = "password"
        //    };

        //    // Act
        //    var service = new AuthenticationService(_loggerMock.Object, _mapper);
        //    var result = await service.AuthenticateAsync(input);

        //    // Assert
        //    result.Should().NotBeNull();
        //    result.Data.Should().BeNull();
        //    result.Succeeded.Should().BeFalse();
        //    result.Errors.Should().NotBeEmpty();
        //    _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.AuthenticateAsync)} method", Times.Never, _exception);

        //}
        //#endregion

        //#region AutoAuthenticationService
        //[Fact]
        //public async Task GivenValidAutoRequest_WhenAddAsyncIsCalled_ThenAddedRecordSuccessfulAndReturnCorrectResult()
        //{
        //    // Arrange

        //    // Act
        //    var service = new AuthenticationService(_loggerMock.Object, _mapper);
        //    var result = await service.AutoAuthenticateAsync(autoLogin);

        //    // Assert
        //    result.Should().NotBeNull();
        //    result.Data.Should().NotBeNull();
        //    result.Succeeded.Should().BeTrue();
        //    _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.AuthenticateAsync)} method", Times.Never, _exception);
        //}
        //[Fact]
        //public async Task GivenRequestAutoIsValidAndEmployeeIdIsNotCorrect_WhenAsyncIsCalled_ThenThrowException()
        //{
        //    // Arrange
        //    var input = new EmployeeAutoLoginRequest
        //    {
        //        EmployeeId = "id",
        //        ActionCode = "Login"
        //    };

        //    // Act
        //    var service = new AuthenticationService(_loggerMock.Object, _mapper);
        //    var result = await service.AutoAuthenticateAsync(input);

        //    // Assert
        //    result.Should().NotBeNull();
        //    result.Data.Should().BeNull();
        //    result.Succeeded.Should().BeFalse();
        //    result.Errors.Should().NotBeEmpty();
        //    _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.AuthenticateAsync)} method", Times.Never, _exception);

        //}
        //#endregion
    }
}
