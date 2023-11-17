using EPAY.ETC.Core.API.Core.Interfaces.Services.PrintLog;
using EPAY.ETC.Core.API.IntegrationTests.Common;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using XUnitPriorityOrderer;

namespace EPAY.ETC.Core.API.IntegrationTests.Services.PrintLog
{
    [TestCaseOrderer(CasePriorityOrderer.TypeName, CasePriorityOrderer.AssembyName)]
    public class PrintLogServiceTests : IntegrationTestBase
    {
        #region Init test data
        private IPrintLogService? printLogService;
        private static Guid newId = Guid.NewGuid();
        private PrintLogRequestModel request = new PrintLogRequestModel()
        {
            RFID = "12s4adsv123sad",
            PlateNumber = "12A122345",
          
        };
        #endregion
        #region AddAsync
        [Fact, Order(1)]
        public async void GivenValidRequest_WhenAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            printLogService = scope.ServiceProvider.GetService<IPrintLogService>()!;

            // Arrange

            // Act
            var result = await printLogService.AddAsync(request);
            newId = result!.Data!.Id!;
            var expected = await printLogService.GetByIdAsync(newId);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            expected.Should().NotBeNull();
            expected.Succeeded.Should().BeTrue();
            result!.Data.Id.Should().Be(expected!.Data!.Id);
            result!.Data.PlateNumber.Should().Be(expected!.Data!.PlateNumber);
        }
        #endregion
        #region GetByIdAsync
        [Fact, Order(2)]
        public async void GivenValidRequest_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            printLogService = scope.ServiceProvider.GetService<IPrintLogService>()!;

            // Arrange

            // Act
            var result = await printLogService.GetByIdAsync(newId);

            // Assert
            result.Should().NotBeNull();
            result.Data!.Id.Should().Be(newId);
            result.Succeeded.Should().BeTrue();
        }

        [Fact, Order(2)]
        public async void GivenValidRequestAndNonExistingRoleId_WhenGetByIdAsyncIsCalled_ThenReturnEmpty()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            printLogService = scope.ServiceProvider.GetService<IPrintLogService>()!;

            // Arrange
            Guid input = Guid.NewGuid();

            // Act
            var result = await printLogService.GetByIdAsync(input);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
        }
        #endregion
        #region UpdateAsync
        [Fact, Order(3)]
        public async void GivenValidRequest_WhenUpdateAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            printLogService = scope.ServiceProvider.GetService<IPrintLogService>()!;

            // Arrange
            request.RFID = "New RFID";
            request.PlateNumber = "New PlateNumber";
           

            // Act
            var result = await printLogService.UpdateAsync(newId, request);
            var expected = await printLogService.GetByIdAsync(newId);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            expected.Should().NotBeNull();
            expected.Succeeded.Should().BeTrue();
            result!.Data?.Id.Should().Be(expected!.Data!.Id);
            result!.Data?.RFID.Should().Be(expected!.Data!.RFID);
            result!.Data?.PlateNumber.Should().Be(expected!.Data!.PlateNumber);
           
        }

        [Fact, Order(5)]
        public async void GivenValidRequestAndNonExistingRoleId_WhenUpdateAsyncIsCalled_ThenReturnNotFound()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            printLogService = scope.ServiceProvider.GetService<IPrintLogService>()!;

            // Arrange
            Guid newId = Guid.NewGuid();

            // Act
            var result = await printLogService.UpdateAsync(newId, request);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Count(x => x.Code == StatusCodes.Status404NotFound).Should().Be(1);
        }
        #endregion
        #region RemoveAsync
        [Fact, Order(4)]
        public async void GivenValidRequest_WhenRemoveAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            printLogService = scope.ServiceProvider.GetService<IPrintLogService>()!;

            // Arrange

            // Act
            var result = await printLogService.RemoveAsync(newId);
            var expected = await printLogService.GetByIdAsync(newId);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeTrue();
            expected.Should().NotBeNull();
            expected.Data.Should().BeNull();
            expected.Succeeded.Should().BeFalse();
            expected.Errors.Count(x => x.Code == ValidationError.NotFound.Code).Should().Be(1);
        }

        [Fact, Order(5)]
        public async void GivenValidRequestAndNonExistingRoleId_WhenRemoveAsyncIsCalled_ThenReturnNotFound()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            printLogService = scope.ServiceProvider.GetService<IPrintLogService>()!;

            // Arrange
            Guid newId = Guid.NewGuid();

            // Act
            var result = await printLogService.RemoveAsync(newId);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Count(x => x.Code == ValidationError.NotFound.Code).Should().Be(1);
        }
        #endregion
    }
}