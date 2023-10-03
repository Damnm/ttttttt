using EPAY.ETC.Core.API.Core.Interfaces.Services.Fusion;
using EPAY.ETC.Core.API.Core.Models.Fusion;
using EPAY.ETC.Core.API.IntegrationTests.Common;
using EPAY.ETC.Core.Models.Validation;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using XUnitPriorityOrderer;

namespace EPAY.ETC.Core.API.IntegrationTests.Services.Fusions
{
    [TestCaseOrderer(CasePriorityOrderer.TypeName, CasePriorityOrderer.AssembyName)]
    public class FusionServiceTests : IntegrationTestBase
    {
        #region Init test data
        private IFusionService? fusionService;
        private static Guid newId = Guid.Empty;
        private FusionAddRequestModel addRequest = new FusionAddRequestModel()
        {
            Epoch = 10,
            Loop1 = true,
            RFID = true,
            Cam1 = "12A123456",
            Loop2 = true,
            Cam2 = "12A123456",
            Loop3 = true,
            ReversedLoop1 = true,
            ReversedLoop2 = true,

        };
        private FusionUpdateRequestModel updateRequest = new FusionUpdateRequestModel()
        {
            Epoch = 20,
            Loop1 = true,
            RFID = true,
            Cam1 = "12A123456",
            Loop2 = true,
            Cam2 = "12A123456",
            Loop3 = true,
            ReversedLoop1 = true,
            ReversedLoop2 = true,

        };
        #endregion
        #region AddAsync
        [Fact, Order(1)]
        public async void GivenValidRequest_WhenAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            fusionService = scope.ServiceProvider.GetService<IFusionService>()!;

            // Arrange

            // Act
            var result = await fusionService.AddAsync(addRequest);
            newId = result.Data!.Id!;
            var expected = await fusionService.GetByIdAsync(newId);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            expected.Should().NotBeNull();
            expected.Succeeded.Should().BeTrue();
            result.Data?.Id.Should().Be(expected!.Data!.Id);
            result.Data?.CreatedDate.Should().Be(expected!.Data!.CreatedDate);
            result.Data?.Epoch.Should().Be(expected!.Data!.Epoch);
            result.Data?.Loop1.Should().Be(expected!.Data!.Loop1);
            result.Data?.RFID.Should().Be(expected!.Data!.RFID);
            result.Data?.Cam1.Should().Be(expected!.Data!.Cam1);
            result.Data?.Loop2.Should().Be(expected!.Data!.Loop2);
            result.Data?.Cam2.Should().Be(expected!.Data!.Cam2);
            result.Data?.Loop3.Should().Be(expected!.Data!.Loop3);
            result.Data?.ReversedLoop1.Should().Be(expected!.Data!.ReversedLoop1);
            result.Data?.ReversedLoop1.Should().Be(expected!.Data!.ReversedLoop1);

        }
        #endregion
        #region GetByIdAsync
        [Fact, Order(2)]
        public async void GivenValidRequest_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            fusionService = scope.ServiceProvider.GetService<IFusionService>()!;

            // Arrange

            // Act
            var result = await fusionService.GetByIdAsync(newId);

            // Assert
            result.Should().NotBeNull();
            result.Data!.Id.Should().Be(newId);
            result.Succeeded.Should().BeTrue();
        }

        [Fact, Order(2)]
        public async void GivenValidRequestAndNonExistingRoleId_WhenGetByIdAsyncIsCalled_ThenReturnEmpty()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            fusionService = scope.ServiceProvider.GetService<IFusionService>()!;

            // Arrange
            Guid input = Guid.NewGuid();

            // Act
            var result = await fusionService.GetByIdAsync(input);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
        }
        #endregion
        #region RemoveAsync
        [Fact, Order(4)]
        public async void GivenValidRequest_WhenRemoveAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            fusionService = scope.ServiceProvider.GetService<IFusionService>()!;

            // Arrange

            // Act
            var result = await fusionService.RemoveAsync(newId);
            var expected = await fusionService.GetByIdAsync(newId);

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
            fusionService = scope.ServiceProvider.GetService<IFusionService>()!;

            // Arrange
            Guid newId = Guid.NewGuid();

            // Act
            var result = await fusionService.RemoveAsync(newId);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Count(x => x.Code == ValidationError.NotFound.Code).Should().Be(1);
        }
        #endregion
        #region UpdateAsync
        [Fact, Order(3)]
        public async void GivenValidRequest_WhenUpdateAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            fusionService = scope.ServiceProvider.GetService<IFusionService>()!;

            // Arrange

            // Act
            var result = await fusionService.UpdateAsync(newId, updateRequest);
            var expected = await fusionService.GetByIdAsync(newId);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            expected.Should().NotBeNull();
            expected.Succeeded.Should().BeTrue();
            result.Data?.Id.Should().Be(expected!.Data!.Id);
            result.Data?.Epoch.Should().Be(expected!.Data!.Epoch);
            result.Data?.Loop1.Should().Be(expected!.Data!.Loop1);
            result.Data?.RFID.Should().Be(expected!.Data!.RFID);
            result.Data?.Cam1.Should().Be(expected!.Data!.Cam1);
            result.Data?.Loop2.Should().Be(expected!.Data!.Loop2);
            result.Data?.Cam2.Should().Be(expected!.Data!.Cam2);
            result.Data?.Loop3.Should().Be(expected!.Data!.Loop3);
            result.Data?.ReversedLoop1.Should().Be(expected!.Data!.ReversedLoop1);
            result.Data?.ReversedLoop1.Should().Be(expected!.Data!.ReversedLoop1);
        }

        [Fact, Order(5)]
        public async void GivenValidRequestAndNonExistingRoleId_WhenUpdateAsyncIsCalled_ThenReturnNotFound()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            fusionService = scope.ServiceProvider.GetService<IFusionService>()!;

            // Arrange
            Guid newId = Guid.NewGuid();

            // Act
            var result = await fusionService.UpdateAsync(newId, updateRequest);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Count(x => x.Code == ValidationError.NotFound.Code).Should().Be(1);
        }
        #endregion
    }
}
