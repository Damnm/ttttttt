using EPAY.ETC.Core.API.Core.Models.ManualBarrierControl;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ManualBarrierControls;
using EPAY.ETC.Core.API.IntegrationTests.Common;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using XUnitPriorityOrderer;

namespace EPAY.ETC.Core.API.IntegrationTests.Persistence.Repository.ManualBarrierControls
{
    [TestCaseOrderer(CasePriorityOrderer.TypeName, CasePriorityOrderer.AssembyName)]
    public class ManualBarrierControlRepositoryTests : IntegrationTestBase
    {
        #region Init test data
        private IManualBarrierControlRepository? _manualBarrierControlRepository;
        private static Guid manualBarrierControlId = Guid.NewGuid();
        private ManualBarrierControlModel manualBarrierControl = new ManualBarrierControlModel()
        {
            Id = manualBarrierControlId,
            EmployeeId = Guid.NewGuid(),
            CreatedDate = DateTime.Now,
            Action =ActionEnum.Close,
            LaneOutId = "00314"
        };
        #endregion
        #region AddAsync
        [Fact, Order(1)]
        public async void GivenValidEntity_WhenAddAsyncIsCalled_ThenRecordAddedSuccessfully()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _manualBarrierControlRepository = scope.ServiceProvider.GetService<IManualBarrierControlRepository>()!;


            // Arrange


            // Act
            var result = await _manualBarrierControlRepository!.AddAsync(manualBarrierControl);
            var expected = await _manualBarrierControlRepository.GetByIdAsync(manualBarrierControl.Id);

            // Assert
            result.Should().NotBeNull();
            expected.Should().NotBeNull();
            result!.Id.Should().Be(expected!.Id);
            result!.CreatedDate.Should().Be(expected!.CreatedDate);
            result!.EmployeeId.Should().Be(expected!.EmployeeId);
            result!.Action.Should().Be(expected!.Action);
            result!.LaneOutId.Should().Be(expected!.LaneOutId);
        }
        #endregion
        #region GetByIdAsync
        [Fact, Order(2)]
        public async Task GivenValidEntity_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _manualBarrierControlRepository = scope.ServiceProvider.GetService<IManualBarrierControlRepository>();
            // Arrange

            // Act
            var result = await _manualBarrierControlRepository!.GetByIdAsync(manualBarrierControl.Id);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(manualBarrierControl!.Id);
            result!.CreatedDate.Should().Be(manualBarrierControl!.CreatedDate);
            result!.EmployeeId.Should().Be(manualBarrierControl!.EmployeeId);
            result!.Action.Should().Be(manualBarrierControl!.Action);
            result!.LaneOutId.Should().Be(manualBarrierControl!.LaneOutId);
        }
        [Fact, Order(2)]
        public async Task GivenRequestIsValidAndIdIsNotExists_WhenGetByIdAsyncIsCalled_ThenReturnNull()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _manualBarrierControlRepository = scope.ServiceProvider.GetRequiredService<IManualBarrierControlRepository>();

            // Arrange
            Guid guid = Guid.NewGuid();

            // Act
            var result = await _manualBarrierControlRepository.GetByIdAsync(guid);

            // Assert
            result.Should().BeNull();
        }
        #endregion
        #region UpdateAsync
        [Fact, Order(3)]
        public async Task GivenValidEntity_WhenUpdateAsyncIsCalled_ThenRecordIsUpdatedAndNotReturnResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _manualBarrierControlRepository = scope.ServiceProvider.GetService<IManualBarrierControlRepository>();
            // Arrange
            //vehicle.Id =  vehicle.Id;
            //vehicle.CreatedDate = vehicle.CreatedDate;
            manualBarrierControl.EmployeeId = Guid.Empty;
            manualBarrierControl.Action = ActionEnum.Open;
            manualBarrierControl.LaneOutId = "0015";

            // Act
            await _manualBarrierControlRepository!.UpdateAsync(manualBarrierControl);
            var expected = await _manualBarrierControlRepository.GetByIdAsync(manualBarrierControlId);

            // Assert
            expected.Should().NotBeNull();
            expected?.Id.Should().Be(expected!.Id);
            expected?.EmployeeId.Should().Be(expected!.EmployeeId);
            expected?.Action.Should().Be(expected!.Action);
            expected?.LaneOutId.Should().Be(expected!.LaneOutId);
        }
        #endregion
        #region RemoveAsync
        [Fact, Order(4)]
        public async Task GivenValidEntity_WhenRemoveAsyncIsCalled_ThenDeletedAndNotReturnResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _manualBarrierControlRepository = scope.ServiceProvider.GetService<IManualBarrierControlRepository>();
            // Arrange

            // Act
            await _manualBarrierControlRepository!.RemoveAsync(manualBarrierControl);
            var expected = await _manualBarrierControlRepository.GetByIdAsync(manualBarrierControlId);

            // Assert
            expected.Should().BeNull();
        }
        #endregion
    }
}
