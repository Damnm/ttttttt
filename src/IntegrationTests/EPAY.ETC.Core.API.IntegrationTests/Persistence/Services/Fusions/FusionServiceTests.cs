using Azure.Core;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Fusion;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Vehicles;
using EPAY.ETC.Core.API.Core.Models.Common;
using EPAY.ETC.Core.API.Core.Validation;
using EPAY.ETC.Core.API.Infrastructure.Services.Vehicles;
using EPAY.ETC.Core.API.IntegrationTests.Common;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUnitPriorityOrderer;

namespace EPAY.ETC.Core.API.IntegrationTests.Persistence.Services.Fusions
{
    [TestCaseOrderer(CasePriorityOrderer.TypeName, CasePriorityOrderer.AssembyName)]
    public class FusionServiceTests: IntegrationTestBase
    {
        #region Init test data
        private IFusionService? fusionService;
        private static Guid newId = Guid.Empty;
        private FusionRequestModel request = new FusionRequestModel()
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
        #endregion
        #region AddAsync
        [Fact, Order(1)]
        public async void GivenValidRequest_WhenAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            fusionService = scope.ServiceProvider.GetService<IFusionService>()!;

            // Arrange

            // Act
            var result = await fusionService.AddAsync(request);
            newId = result!.Data!.Id!;
            var expected = await fusionService.GetByIdAsync(newId);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            expected.Should().NotBeNull();
            expected.Succeeded.Should().BeTrue();
            result!.Data.Id.Should().Be((Guid)expected!.Data!.Id);
            result!.Data.CreatedDate.Should().Be(expected!.Data!.CreatedDate);
            result!.Data.Epoch.Should().Be(expected!.Data!.Epoch);
            result!.Data.Loop1.Should().Be(expected!.Data!.Loop1);
            result!.Data.RFID.Should().Be(expected!.Data!.RFID);
            result!.Data.Cam1.Should().Be(expected!.Data!.Cam1);
            result!.Data.Loop2.Should().Be(expected!.Data!.Loop2);
            result!.Data.Cam2.Should().Be(expected!.Data!.Cam2);
            result!.Data.Loop3.Should().Be(expected!.Data!.Loop3);
            result!.Data.ReversedLoop1.Should().Be(expected!.Data!.ReversedLoop1);
            result!.Data.ReversedLoop1.Should().Be(expected!.Data!.ReversedLoop1);

        }
        #endregion
        #region GetByIdAsync
        [Fact, Order(2)]
        public async void GivenValidRequest_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            fusionService = scope.ServiceProvider.GetService<IFusionService>()!;

            // Arrange
            Guid input = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66bfa6");

            // Act
            var result = await fusionService.GetByIdAsync(input);

            // Assert
            result.Should().NotBeNull();
            result.Data!.Id.Should().Be(input);
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
        [Fact, Order(3)]
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
            result.Succeeded.Should().BeFalse();
            expected.Should().NotBeNull();
            expected.Data.Should().BeNull();
            expected.Succeeded.Should().BeFalse();
        }

        [Fact, Order(4)]
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
        [Fact, Order(4)]
        public async void GivenValidRequest_WhenUpdateAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            fusionService = scope.ServiceProvider.GetService<IFusionService>()!;

            // Arrange
            request.Epoch = 20;
            request.Loop1 = true;
            request.RFID = true;
            request.Cam1 = "12A123456";
            request.Loop2 = true;
            request.Cam2 = "12A123456";
            request.Loop3 = true;
            request.ReversedLoop1 = true;
            request.ReversedLoop2 = true;

            // Act
            var result = await fusionService.UpdateAsync(newId, request);
            var expected = await fusionService.GetByIdAsync(newId);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            expected.Should().NotBeNull();
            expected.Succeeded.Should().BeFalse();
            //result!.Data.Id.Should().Be((Guid)expected!.Data!.Id);
            result!.Data.Epoch.Should().Be(expected!.Data!.Epoch);
            result!.Data.Loop1.Should().Be(expected!.Data!.Loop1);
            result!.Data.RFID.Should().Be(expected!.Data!.RFID);
            result!.Data.Cam1.Should().Be(expected!.Data!.Cam1);
            result!.Data.Loop2.Should().Be(expected!.Data!.Loop2);
            result!.Data.Cam2.Should().Be(expected!.Data!.Cam2);
            result!.Data.Loop3.Should().Be(expected!.Data!.Loop3);
            result!.Data.ReversedLoop1.Should().Be(expected!.Data!.ReversedLoop1);
            result!.Data.ReversedLoop1.Should().Be(expected!.Data!.ReversedLoop1);
        }

        [Fact, Order(5)]
        public async void GivenValidRequestAndNonExistingRoleId_WhenUpdateAsyncIsCalled_ThenReturnNotFound()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            fusionService = scope.ServiceProvider.GetService<IFusionService>()!;

            // Arrange
            Guid newId = Guid.NewGuid();

            // Act
            var result = await fusionService.UpdateAsync(newId, request);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Count(x => x.Code == ValidationError.NotFound.Code).Should().Be(1);
        }
        #endregion
    }
}
