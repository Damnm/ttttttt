using EPAY.ETC.Core.API.Core.Models.VehicleCategories;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.VehicleCategories;
using EPAY.ETC.Core.API.IntegrationTests.Common;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.IntegrationTests.Persistence.Repository.VehicleCategories
{
    public class VehicleCategoryRepositoryTests : IntegrationTestBase
    {
        #region Init Method
        private IVehicleCategoryRepository? _repository;
        #endregion

        #region Init Data test
        #endregion

        #region GetAllAsync
        [Fact]
        public async Task GivenRequestIsValid_WhenGetAllAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _repository = scope.ServiceProvider.GetRequiredService<IVehicleCategoryRepository>();

            // Arrange

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            result.Should().NotBeNull().And.HaveCount(5);
        }
        [Fact]
        public async Task GivenRequestIsValidAndExpressionIsExists_WhenGetAllAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _repository = scope.ServiceProvider.GetRequiredService<IVehicleCategoryRepository>();

            // Arrange
            Expression<Func<VehicleCategoryModel, bool>> expression = s => s.Name == "Xe nhượng quyền";

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
            _repository = scope.ServiceProvider.GetRequiredService<IVehicleCategoryRepository>();

            // Arrange
            Guid guid = Guid.Parse("2b0557d0-cc6b-4fc2-a0b3-08788c9fd8c7");

            // Act
            var result = await _repository.GetByIdAsync(guid);

            // Assert
            result.Should().NotBeNull();
            result?.Id.Should().Be(guid);
        }
        #endregion
    }
}
