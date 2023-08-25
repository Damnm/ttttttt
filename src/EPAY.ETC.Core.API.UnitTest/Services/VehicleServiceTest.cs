using ACV.Toll.Admin.API.Infrastructure.UnitTests.Common;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Vehicle;
using EPAY.ETC.Core.API.Infrastructure.Services.Vehicles;
using EPAY.ETC.Core.API.UnitTest.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.UnitTest.Services
{
    public class VehicleServiceTest : AutoMapperTestBase
    {
        #region Variables
        private readonly Mock<ILogger<VehicleService>> _loggerMock = new();
        private readonly Mock<IVehicleRepository> _repositoryMock = new();
        private static Guid id = Guid.NewGuid();
        private VehicleModel _request = new VehicleModel()
        {
            Id = id,
            
        };
        #endregion

        #region AddAsync
        [Fact]
        public async Task GivenValidRequest_WhenAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _repositoryMock.Setup(x => x.AddAsync(It.IsNotNull<VehicleModel>())).ReturnsAsync(_request);

            // Act
            var service = new VehicleService(_loggerMock.Object, _repositoryMock.Object, _mapper);
            var result = await service.AddAsync(_request);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            _repositoryMock.Verify(x => x.AddAsync(It.IsNotNull<VehicleModel>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.AddAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"Failed to run {nameof(service.AddAsync)} method", Times.Never, null);
        }
        #endregion
    }
}
