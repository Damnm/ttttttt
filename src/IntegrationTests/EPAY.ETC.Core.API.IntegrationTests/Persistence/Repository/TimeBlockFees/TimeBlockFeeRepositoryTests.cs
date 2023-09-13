using EPAY.ETC.Core.API.Core.Models.TimeBlockFees;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.TimeBlockFees;
using EPAY.ETC.Core.API.IntegrationTests.Common;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.IntegrationTests.Persistence.Repository.TimeBlockFees
{
    public class TimeBlockFeeRepositoryTests : IntegrationTestBase
    {
        #region Init Method
        private ITimeBlockFeeRepository? _repository;
        #endregion

        #region Init Data test
        #endregion

        #region GetAllAsync
        [Fact]
        public async Task GivenRequestIsValid_WhenGetAllAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _repository = scope.ServiceProvider.GetRequiredService<ITimeBlockFeeRepository>();

            // Arrange

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            result.Should().NotBeNull().And.HaveCount(16);
        }
        [Fact]
        public async Task GivenRequestIsValidAndExpressionIsExists_WhenGetAllAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _repository = scope.ServiceProvider.GetRequiredService<ITimeBlockFeeRepository>();

            // Arrange
            Expression<Func<TimeBlockFeeModel, bool>> expression = s => s.CustomVehicleTypeId == Guid.Parse("a4a39e55-85c0-4761-ba64-f941111186f9");

            // Act
            var result = await _repository.GetAllAsync(expression);

            // Assert
            result.Should().NotBeNull().And.HaveCount(4);
        }
        #endregion

        #region GetByIdAsync
        [Fact]
        public async Task GivenRequestIsValid_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _repository = scope.ServiceProvider.GetRequiredService<ITimeBlockFeeRepository>();

            // Arrange
            Guid guid = Guid.Parse("df059c09-28aa-4134-919a-e3b3041213a4");

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
            _repository = scope.ServiceProvider.GetRequiredService<ITimeBlockFeeRepository>();

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
