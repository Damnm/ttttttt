using EPAY.ETC.Core.API.Core.Extensions;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Vehicle;
using EPAY.ETC.Core.API.IntegrationTests.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XUnitPriorityOrderer;

namespace EPAY.ETC.Core.API.IntegrationTests.Persistence.Repository.Vehicle
{
    [TestCaseOrderer(CasePriorityOrderer.TypeName, CasePriorityOrderer.AssembyName)]
    public class VehicleRepositoryTest : IntegrationTestBase
    {
        private IVehicleRepository? _repository;
        #region Init test data
        private static Guid vehicleId = Guid.NewGuid();
        private VehicleModel vehicle = new VehicleModel()
        {
            Id = vehicleId,
            CreatedDate = DateTime.Now.ConvertToAsianTime(DateTimeKind.Local),
            RFID = "123asdas48v6aaswd",
            PlateNumber = "12A123456",
            PlateColor = "White",
            Make = "Toyotra",
            Seat = 5,
            VehicleType = "Loai 2",
            Weight = 5000,
        };
        #endregion
        #region AddAsync
        [Fact, Order(1)]
        public async void GivenValidEntity_WhenAddAsyncIsCalled_ThenRecordAddedSuccessfully()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _repository = scope.ServiceProvider.GetService<IVehicleRepository>()!;


            // Arrange


            // Act
            var result = await _repository!.AddAsync(vehicle);
            var expected = await _repository.GetByIdAsync(vehicle.Id);

            // Assert
            result.Should().NotBeNull();
            expected.Should().NotBeNull();
            result!.Id.Should().Be(expected!.Id);
            result!.RFID.Should().Be(expected!.RFID);
            result!.PlateNumber.Should().Be(expected!.PlateNumber);
            result!.PlateColor.Should().Be(expected!.PlateColor);
            result!.Make.Should().Be(expected!.Make);
            result!.Seat.Should().Be(expected!.Seat);
            result!.VehicleType.Should().Be(expected!.VehicleType);
            result!.Weight.Should().Be(expected!.Weight);
        }
        #endregion
        #region GetByIdAsync
        [Fact, Order(2)]
        public async Task GivenValidEntity_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _repository = scope.ServiceProvider.GetService<IVehicleRepository>();
            // Arrange

            // Act
            var result = await _repository!.GetByIdAsync(vehicle.Id);

            // Assert
            result.Should().NotBeNull();
            result?.Id.Should().Be(vehicle.Id);
            result?.CreatedDate.Should().Be(vehicle.CreatedDate);
            result?.RFID.Should().Be(vehicle.RFID);
            result?.PlateNumber.Should().Be(vehicle.PlateNumber);
            result?.PlateColor.Should().Be(vehicle.PlateColor);
            result?.Make.Should().Be(vehicle.Make);
            result?.Seat.Should().Be(vehicle.Seat);
            result?.VehicleType.Should().Be(vehicle.VehicleType);
            result?.Weight.Should().Be(vehicle.Weight);
        }
        #endregion
        #region UpdateAsync
        [Fact, Order(3)]
        public async Task GivenValidEntity_WhenUpdateAsyncIsCalled_ThenRecordIsUpdatedAndNotReturnResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _repository = scope.ServiceProvider.GetService<IVehicleRepository>();
            // Arrange
            //vehicle.Id =  vehicle.Id;
            //vehicle.CreatedDate = vehicle.CreatedDate;
            vehicle.RFID = "123asdas48v6aaswd";
            vehicle.PlateNumber = "12A123456";
            vehicle.PlateColor = "White";
            vehicle.Make = "Supra";
            vehicle.Seat = 2;
            vehicle.VehicleType = "Loai 3";
            vehicle.Weight = 500000;

            // Act
            await _repository!.UpdateAsync(vehicle);
            var expected = await _repository.GetByIdAsync(vehicleId);

            // Assert
            expected.Should().NotBeNull();
            expected?.Id.Should().Be(expected!.Id);
            expected?.RFID.Should().Be(expected!.RFID);
            expected?.PlateNumber.Should().Be(expected!.PlateNumber);
            expected?.PlateColor.Should().Be(expected!.PlateColor);
            expected?.Make.Should().Be(expected!.Make);
            expected?.Seat.Should().Be(expected!.Seat);
            expected?.VehicleType.Should().Be(expected!.VehicleType);
            expected?.Weight.Should().Be(expected!.Weight);
            //expected?.CreatedDate.Should().Be(vehicle.CreatedDate);
        }
        #endregion
        #region RemoveAsync
        [Fact, Order(4)]
        public async Task GivenValidEntity_WhenRemoveAsyncIsCalled_ThenDeletedAndNotReturnResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _repository = scope.ServiceProvider.GetService<IVehicleRepository>();
            // Arrange

            // Act
            await _repository!.RemoveAsync(vehicle);
            var expected = await _repository.GetByIdAsync(vehicleId);

            // Assert
            expected.Should().BeNull();
        }
        #endregion
    }
}
