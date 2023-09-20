using EPAY.ETC.Core.API.Core.Interfaces.Services.Vehicles;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.IntegrationTests.Common;
using EPAY.ETC.Core.Models.Validation;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using XUnitPriorityOrderer;

namespace EPAY.ETC.Core.API.IntegrationTests.Persistence.Services.Vehicles
{
    [TestCaseOrderer(CasePriorityOrderer.TypeName, CasePriorityOrderer.AssembyName)]
    public class VehicleServiceTests : IntegrationTestBase
    {
        #region Init test data
        private IVehicleService? vehicleService;
        private static Guid newId = Guid.NewGuid();
        private VehicleRequestModel request = new VehicleRequestModel()
        {
            RFID = "12s4adsv123sad",
            PlateNumber = "12A122345",
            PlateColor = "blue",
            Make = "Toyota",
            Seat = 5,
            Model = "Model",
            VehicleType = "Loai 1",
            Weight = 5000,
        };
        #endregion
        #region AddAsync
        [Fact, Order(1)]
        public async void GivenValidRequest_WhenAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            vehicleService = scope.ServiceProvider.GetService<IVehicleService>()!;

            // Arrange

            // Act
            var result = await vehicleService.AddAsync(request);
            newId = result!.Data!.Id!;
            var expected = await vehicleService.GetByIdAsync(newId);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            expected.Should().NotBeNull();
            expected.Succeeded.Should().BeTrue();
            result!.Data.Id.Should().Be((Guid)expected!.Data!.Id);
            result!.Data.PlateNumber.Should().Be(expected!.Data!.PlateNumber);
            result!.Data.PlateColor.Should().Be(expected!.Data!.PlateColor);
            result!.Data.RFID.Should().Be(expected!.Data!.RFID);
            result!.Data.Make.Should().Be(expected!.Data!.Make);
            result!.Data.Model.Should().Be(expected!.Data!.Model);
            result!.Data.Seat.Should().Be(expected!.Data!.Seat);
            result!.Data.VehicleType.Should().Be(expected!.Data!.VehicleType);
            result!.Data.Weight.Should().Be(expected!.Data!.Weight);

        }
        #endregion
        #region GetByIdAsync
        [Fact, Order(2)]
        public async void GivenValidRequest_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            vehicleService = scope.ServiceProvider.GetService<IVehicleService>()!;

            // Arrange

            // Act
            var result = await vehicleService.GetByIdAsync(newId);

            // Assert
            result.Should().NotBeNull();
            result.Data!.Id.Should().Be(newId);
            result.Succeeded.Should().BeTrue();
        }

        [Fact, Order(2)]
        public async void GivenValidRequestAndNonExistingRoleId_WhenGetByIdAsyncIsCalled_ThenReturnEmpty()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            vehicleService = scope.ServiceProvider.GetService<IVehicleService>()!;

            // Arrange
            Guid input = Guid.NewGuid();

            // Act
            var result = await vehicleService.GetByIdAsync(input);

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
            vehicleService = scope.ServiceProvider.GetService<IVehicleService>()!;

            // Arrange
            request.RFID = "New RFID";
            request.PlateNumber = "New PlateNumber";
            request.PlateColor = "New PlateColor";
            request.Seat = 2;
            request.Make = "New Make";
            request.Model = "New Model";
            request.VehicleType = "New VehicleType";
            request.Weight = 2000;

            // Act
            var result = await vehicleService.UpdateAsync(newId, request);
            var expected = await vehicleService.GetByIdAsync(newId);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            expected.Should().NotBeNull();
            expected.Succeeded.Should().BeTrue();
            result!.Data?.Id.Should().Be((Guid)expected!.Data!.Id);
            result!.Data?.RFID.Should().Be(expected!.Data!.RFID);
            result!.Data?.PlateNumber.Should().Be(expected!.Data!.PlateNumber);
            result!.Data?.PlateColor.Should().Be(expected!.Data!.PlateColor);
            result!.Data?.Make.Should().Be(expected!.Data!.Make);
            result!.Data?.Model.Should().Be(expected!.Data!.Model);
            result!.Data?.Seat.Should().Be(expected!.Data!.Seat);
            result!.Data?.VehicleType.Should().Be(expected!.Data!.VehicleType);
            result!.Data?.Weight.Should().Be(expected!.Data!.Weight);
        }

        [Fact, Order(5)]
        public async void GivenValidRequestAndNonExistingRoleId_WhenUpdateAsyncIsCalled_ThenReturnNotFound()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            vehicleService = scope.ServiceProvider.GetService<IVehicleService>()!;

            // Arrange
            Guid newId = Guid.NewGuid();

            // Act
            var result = await vehicleService.UpdateAsync(newId, request);

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
            vehicleService = scope.ServiceProvider.GetService<IVehicleService>()!;

            // Arrange

            // Act
            var result = await vehicleService.RemoveAsync(newId);
            var expected = await vehicleService.GetByIdAsync(newId);

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
            vehicleService = scope.ServiceProvider.GetService<IVehicleService>()!;

            // Arrange
            Guid newId = Guid.NewGuid();

            // Act
            var result = await vehicleService.RemoveAsync(newId);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Count(x => x.Code == ValidationError.NotFound.Code).Should().Be(1);
        }
        #endregion
    }
}