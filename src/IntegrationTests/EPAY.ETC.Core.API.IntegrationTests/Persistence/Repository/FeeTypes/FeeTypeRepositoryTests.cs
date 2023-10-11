using EPAY.ETC.Core.API.Core.Models.FeeTypes;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.FeeTypes;
using EPAY.ETC.Core.API.IntegrationTests.Common;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.IntegrationTests.Persistence.Repository.FeeTypes
{
    public class FeeTypeRepositoryTests : IntegrationTestBase
    {
        #region Init Method
        private IFeeTypeRepository? _repository;
        #endregion

        #region Init Data test
        #endregion

        #region GetAllAsync
        [Fact]
        public async Task GivenRequestIsValid_WhenGetAllAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _repository = scope.ServiceProvider.GetRequiredService<IFeeTypeRepository>();

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
            _repository = scope.ServiceProvider.GetRequiredService<IFeeTypeRepository>();

            // Arrange
            Expression<Func<FeeTypeModel, bool>> expression = s => s.FeeName == Core.Models.Enum.FeeTypeEnum.Free;

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
            _repository = scope.ServiceProvider.GetRequiredService<IFeeTypeRepository>();

            // Arrange
            Guid guid = Guid.Parse("1143d8c3-22e2-4bd5-a690-89ca0c47b3c9");

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
            _repository = scope.ServiceProvider.GetRequiredService<IFeeTypeRepository>();

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
