using EPAY.ETC.Core.API.Core.Extensions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Vehicles;
using EPAY.ETC.Core.API.Core.Models.Common;
using EPAY.ETC.Core.API.Core.Validation;
using EPAY.ETC.Core.API.IntegrationTests.Common;
using FluentAssertions;
using FluentAssertions.Common;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUnitPriorityOrderer;

namespace EPAY.ETC.Core.API.IntegrationTests.Services
{
    [TestCaseOrderer(CasePriorityOrderer.TypeName, CasePriorityOrderer.AssembyName)]
    public class VehicleServiceTest : IntegrationTestBase
    {
        #region
        private IVehicleService? _vehicleService;
        private static Guid id { get;set; }
        private VehicleRequestModel request = new VehicleRequestModel()
        {
            CreatedDate = DateTime.Now.ConvertToAsianTime(DateTimeKind.Local),
            RFID = "123as4d65vasdasfv",
            PlateNumber = "12A12345",
            PlateColor = "Blue",
            Make = "Toyota",
            Seat = 5,
            VehicleType ="Loai 1",
            Weight = 5000
        };
        #endregion
        #region AddAsync
        [Fact, Order(1)]
        public async Task GivenValidRequest_WhenAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _vehicleService = scope.ServiceProvider.GetService<IVehicleService>()!;

            // Arrange

            // Act
            var result = await _vehicleService.AddAsync(request);
            id = result.Data.Id;
            var expected = await _vehicleService.GetByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            expected.Should().NotBeNull();
            expected.Succeeded.Should().BeTrue();
            result!.Data.Id.Should().Be(expected!.Data.Id);
            result!.Data.RFID.Should().Be(expected!.Data.RFID);
            result!.Data.PlateNumber.Should().Be(expected!.Data.PlateNumber);
            result!.Data.PlateColor.Should().Be(expected!.Data.PlateColor);
            result!.Data.Make.Should().Be(expected!.Data.Make);
            result!.Data.Seat.Should().Be(expected!.Data.Seat);
            result!.Data.VehicleType.Should().Be(expected!.Data.VehicleType);
            result!.Data.Weight.Should().Be(expected!.Data.Weight);
        }
        [Fact, Order(2)]
        public async Task GivenValidRequestAndVehiclesAlreadyExists_WhenAddAsyncIsCalled_ThenReturnConflict()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _vehicleService = scope.ServiceProvider.GetService<IVehicleService>();

            // Arrange

            // Act
            var result = await _vehicleService!.AddAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Errors.Count(x => x.Code == 409).Should().Be(1);
        }
        #endregion
        #region GetByIdAsync
        [Fact, Order(3)]
        public async Task GivenValidRequest_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _vehicleService = scope.ServiceProvider.GetService<IVehicleService>();

            // Arrange

            // Act
            var result = await _vehicleService.GetByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(id);
            result.Data.RFID.Should().Be(request.RFID);
            result.Data.PlateNumber.Should().Be(request.PlateNumber);
            result.Data.PlateColor.Should().Be(request.PlateColor);
            result.Data.Make.Should().Be(request.Make);
            result.Data.Seat.Should().Be(request.Seat);
            result.Data.VehicleType.Should().Be(request.VehicleType);
            result.Data.Weight.Should().Be(request.Weight);
        }
        #endregion
    }
}
