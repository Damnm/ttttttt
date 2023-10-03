using EPAY.ETC.Core.API.Core.Models.CustomVehicleTypes;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.CustomVehicleTypes;
using EPAY.ETC.Core.API.IntegrationTests.Common;
using EPAY.ETC.Core.Models.Enums;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.IntegrationTests.Persistence.Repository.CustomVehicleTypes
{
    public class CustomVehicleTypeRepositoryTests : IntegrationTestBase
    {
        #region Init Method
        private ICustomVehicleTypeRepository? _repository;
        #endregion

        #region Init Data test
        #endregion

        #region GetAllAsync
        [Fact]
        public async Task GivenRequestIsValid_WhenGetAllAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _repository = scope.ServiceProvider.GetRequiredService<ICustomVehicleTypeRepository>();

            // Arrange

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            result.Should().NotBeNull().And.HaveCount(4);
        }
        [Fact]
        public async Task GivenRequestIsValidAndExpressionIsExists_WhenGetAllAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _repository = scope.ServiceProvider.GetRequiredService<ICustomVehicleTypeRepository>();

            // Arrange
            Expression<Func<CustomVehicleTypeModel, bool>> expression = s => s.Name == CustomVehicleTypeEnum.Type1;

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
            _repository = scope.ServiceProvider.GetRequiredService<ICustomVehicleTypeRepository>();

            // Arrange
            Guid guid = Guid.Parse("be652877-ca81-4fb4-bfa1-b9cec61f9e6b");

            // Act
            var result = await _repository.GetByIdAsync(guid);

            // Assert
            result.Should().NotBeNull();
            result?.Id.Should().Be(guid);
        }

        [Fact]
        public async Task GivenRequestIsValidAndIdIsNotExists_WhenGetByIdAsyncIsCalled_ThenReturnNull()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _repository = scope.ServiceProvider.GetRequiredService<ICustomVehicleTypeRepository>();

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
