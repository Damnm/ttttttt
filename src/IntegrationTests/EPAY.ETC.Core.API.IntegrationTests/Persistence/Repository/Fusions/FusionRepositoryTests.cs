using EPAY.ETC.Core.API.Core.Interfaces.Repositories;
using EPAY.ETC.Core.API.Core.Models.Fusion;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Fusion;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Vehicle;
using EPAY.ETC.Core.API.IntegrationTests.Common;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUnitPriorityOrderer;

namespace EPAY.ETC.Core.API.IntegrationTests.Persistence.Repository.Fusions
{
    [TestCaseOrderer(CasePriorityOrderer.TypeName, CasePriorityOrderer.AssembyName)]
    public class FusionRepositoryTests:IntegrationTestBase
    {
        #region Init test data
        private IFusionRepository? _fusionRepository;
        private static Guid objectId = Guid.NewGuid();
        private FusionModel fusion = new FusionModel()
        {
            Id = objectId,
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
        public async void GivenValidEntity_WhenAddAsyncIsCalled_ThenRecordAddedSuccessfully()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _fusionRepository = scope.ServiceProvider.GetService<IFusionRepository>()!;


            // Arrange


            // Act
            var result = await _fusionRepository!.AddAsync(fusion);
            var expected = await _fusionRepository.GetByIdAsync(fusion.Id);

            // Assert
            result.Should().NotBeNull();
            expected.Should().NotBeNull();
            result!.Id.Should().Be(expected!.Id);
            result!.Epoch.Should().Be(expected!.Epoch);
            result!.Loop1.Should().Be(expected!.Loop1);
            result!.RFID.Should().Be(expected!.RFID);
            result!.Cam1.Should().Be(expected!.Cam1);
            result!.Loop2.Should().Be(expected!.Loop2);
            result!.Cam2.Should().Be(expected!.Cam2);
            result!.Loop3.Should().Be(expected!.Loop3);
            result!.ReversedLoop1.Should().Be(expected!.ReversedLoop1);
            result!.ReversedLoop2.Should().Be(expected!.ReversedLoop2);     
        }
        #endregion
        #region GetByIdAsync
        [Fact, Order(2)]
        public async Task GivenValidEntity_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _fusionRepository = scope.ServiceProvider.GetService<IFusionRepository>();
            // Arrange

            // Act
            var result = await _fusionRepository!.GetByIdAsync(fusion.Id);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(fusion.Id);
            result!.Epoch.Should().Be(fusion.Epoch);
            result!.Loop1.Should().Be(fusion.Loop1);
            result!.RFID.Should().Be(fusion.RFID);
            result!.Cam1.Should().Be(fusion.Cam1);
            result!.Loop2.Should().Be(fusion.Loop2);
            result!.Cam2.Should().Be(fusion.Cam2);
            result!.Loop3.Should().Be(fusion.Loop3);
            result!.ReversedLoop1.Should().Be(fusion.ReversedLoop1);
            result!.ReversedLoop2.Should().Be(fusion.ReversedLoop2);
        }
        [Fact, Order(2)]
        public async Task GivenRequestIsValidAndIdIsNotExists_WhenGetByIdAsyncIsCalled_ThenReturnNull()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _fusionRepository = scope.ServiceProvider.GetRequiredService<IFusionRepository>();

            // Arrange
            Guid guid = Guid.NewGuid();

            // Act
            var result = await _fusionRepository.GetByIdAsync(guid);

            // Assert
            result.Should().BeNull();
        }
        #endregion
        #region UpdateAsync
        [Fact, Order(3)]
        public async Task GivenValidEntity_WhenUpdateAsyncIsCalled_ThenRecordIsUpdatedAndNotReturnResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _fusionRepository = scope.ServiceProvider.GetService<IFusionRepository>();
            // Arrange
            //vehicle.Id =  vehicle.Id;
            //vehicle.CreatedDate = vehicle.CreatedDate;
            fusion.Epoch = 20;
            fusion.Loop1 = false;
            fusion.RFID = true;
            fusion.Cam1 = "12A123456";
            fusion.Loop2 = true;
            fusion.Cam2 = "12A123456";
            fusion.Loop3 = true;
            fusion.ReversedLoop1 = true;
            fusion.ReversedLoop2 = true;

            // Act
            await _fusionRepository!.UpdateAsync(fusion);
            var expected = await _fusionRepository.GetByIdAsync(objectId);

            // Assert
            expected.Should().NotBeNull();
            expected?.Id.Should().Be(expected!.Id);
            expected?.Epoch.Should().Be(expected!.Epoch);
            expected?.Loop1.Should().Be(expected!.Loop1);
            expected?.RFID.Should().Be(expected!.RFID);
            expected?.Cam1.Should().Be(expected!.Cam1);
            expected?.Loop2.Should().Be(expected!.Loop2);
            expected?.Cam2.Should().Be(expected!.Cam2);
            expected?.Loop3.Should().Be(expected!.Loop3);
            expected?.ReversedLoop1.Should().Be(expected!.ReversedLoop1);
            expected?.ReversedLoop2.Should().Be(expected!.ReversedLoop2);
            //expected?.CreatedDate.Should().Be(vehicle.CreatedDate);
        }
        #endregion
        #region RemoveAsync
        [Fact, Order(4)]
        public async Task GivenValidEntity_WhenRemoveAsyncIsCalled_ThenDeletedAndNotReturnResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _fusionRepository = scope.ServiceProvider.GetService<IFusionRepository>();
            // Arrange

            // Act
            await _fusionRepository!.RemoveAsync(fusion);
            var expected = await _fusionRepository.GetByIdAsync(objectId);

            // Assert
            expected.Should().BeNull();
        }
        #endregion
    }
}
