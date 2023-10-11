using EPAY.ETC.Core.API.Core.Models.VehicleGroups;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.VehicleGroups;
using EPAY.ETC.Core.API.IntegrationTests.Common;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.IntegrationTests.Persistence.Repository.VehicleGroups
{
    public class VehicleGroupRepositoryTests : IntegrationTestBase
    {
        #region Init Method
        private IVehicleGroupRepository? _repository;
        #endregion

        #region Init Data test
        #endregion

        #region GetAllAsync
        [Fact]
        public async Task GivenRequestIsValid_WhenGetAllAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _repository = scope.ServiceProvider.GetRequiredService<IVehicleGroupRepository>();

            // Arrange

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            result.Should().NotBeNull().And.HaveCount(3);
        }
        [Fact]
        public async Task GivenRequestIsValidAndExpressionIsExists_WhenGetAllAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _repository = scope.ServiceProvider.GetRequiredService<IVehicleGroupRepository>();

            // Arrange
            Expression<Func<VehicleGroupModel, bool>> expression = s => s.GroupName == "Taxi Xanh";

            // Act
            var result = await _repository.GetAllAsync(expression);

            // Assert
            result.Should().NotBeNull().And.HaveCount(1);
        }
        #endregion

        #region GetByIdAsync
        [Fact]
        public async Task GivenRequestIsValid_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _repository = scope.ServiceProvider.GetRequiredService<IVehicleGroupRepository>();

            // Arrange
            Guid guid = Guid.Parse("1fc5fc58-94e4-4169-a576-3cd9ecf8eb96");

            // Act
            var result = await _repository.GetByIdAsync(guid);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GivenRequestIsValidAndIdIsNotExists_WhenGetByIdAsyncIsCalled_ThenReturnNull()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _repository = scope.ServiceProvider.GetRequiredService<IVehicleGroupRepository>();

            // Arrange
            Guid guid = Guid.NewGuid();

            // Act
            var result = await _repository.GetByIdAsync(guid);

            // Assert
            result.Should().BeNull();
        }
        #endregion
    }
}
