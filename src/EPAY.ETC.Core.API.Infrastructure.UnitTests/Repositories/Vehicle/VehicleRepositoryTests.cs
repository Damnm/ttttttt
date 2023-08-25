using EPAY.ETC.Core.API.Core.Models.Common;
using EPAY.ETC.Core.API.Core.Models.SearchRequest;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Vehicle;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.OrderBuilder;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Common;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Repositories.Vehicle
{
    public class VehicleRepositoryTests : AutoMapperTestBase
    {
        #region Init mock data
        private Mock<CoreDbContext> _dbContextMock = new Mock<CoreDbContext>();
        private Mock<ILogger<VehicleRepository>> _loggerMock = new Mock<ILogger<VehicleRepository>>();
        private Mock<IVehicleDynamicColumnOrderService<VehicleSearchItemModel>> vehicleColumnOrderService = new Mock<IVehicleDynamicColumnOrderService<VehicleSearchItemModel>>();
        private Mock<DbSet<VehicleModel>>? _dbVehicleSetMock;
        #endregion

        private VehicleSearchRequestModel _request = new VehicleSearchRequestModel()
        {
            Refinements = new VehicleSearchRefinementsModel()
            {
                PlateNumber = "Some plate number",
            },
            SearchOptions = new SearchOptionsModel()
            {
                PageSize = 10,
            }
        };
        private List<VehicleModel> _vehicles = new List<VehicleModel>()
        {
            new VehicleModel()
            {
                Id = Guid.NewGuid(),
                RFID = "1245asdasda",
                CreatedDate = DateTime.Now,
                PlateNumber = "Some Plate number",
                PlateColor = "Some Plate colors",
                Make = "Some make",
                Seat = 7,
                Weight = 2000,
                VehicleType = "Loại 1"
            }
        };

        #region GetByIdAsync
        [Fact]
        public async void GivenValidRequest_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var data = _vehicles.FirstOrDefault();
            _dbVehicleSetMock = EFTestHelper.GetMockDbSet(_vehicles);
            _dbContextMock.Setup(x => x.Vehicles).Returns(_dbVehicleSetMock.Object);
             
            // Act
            var vehicleRepository = new VehicleRepository(_loggerMock.Object, _dbContextMock.Object, vehicleColumnOrderService.Object);
            var result = await vehicleRepository.GetByIdAsync(data!.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(data);
            _dbContextMock.Verify(x => x.Vehicles, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleRepository.GetByIdAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleRepository.GetByIdAsync)} method", Times.Never, null);
        }

        #endregion
    }
}