using EPAY.ETC.Core.API.Core.Extensions;
using EPAY.ETC.Core.API.Core.Models.Fees;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Fees;
using EPAY.ETC.Core.API.IntegrationTests.Common;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using XUnitPriorityOrderer;

namespace EPAY.ETC.Core.API.IntegrationTests.Persistence.Repository.Fees
{
    /// <summary>
    /// Must be run all to pass this test
    /// </summary>
    [TestCaseOrderer(CasePriorityOrderer.TypeName, CasePriorityOrderer.AssembyName)]
    public class FeeRepositoryTests : IntegrationTestBase
    {
        #region Init structure
        private IFeeRepository _feeRepository;
        #endregion

        #region Init test data
        public FeeModel fee = new FeeModel()
        {
            Id = Guid.Parse("5eac112b-3e80-487a-a736-8ff584e8b722"),
            CreatedDate = new DateTime(2023, 9, 11),
            Amount = 15000,
            ConfidenceScore = (float?)0.9,
            CustomVehicleTypeId = Guid.Parse("090a7db5-2d5d-4c1c-a32c-27f946f8dd61"),
            Duration = 15000,
            EmployeeId = "Some employee",
            ObjectId = Guid.Parse("d246a94a-8064-4bf7-bf4e-0723864daebf"),
            VehicleCategoryId = Guid.Parse("d985883f-a420-47f1-8fac-d8d79dba96ca"),
            LaneInId = "0301",
            LaneInDate = new DateTime(2023, 9, 14, 1, 0, 24),
            LaneInEpoch = 1694649600,
            LaneOutId = "0302",
            LaneOutDate = new DateTime(2023, 9, 14, 1, 13, 32),
            LaneOutEpoch = 1694654012,
            RFID = "023156456956875",
            PlateNumber = "29A23541"
        };
        #endregion

        [Fact, Order(1)]
        public async Task GivenRequestIsValid_WhenAddAsyncIsCalled_ThenRecordAddedSuccessfulAndReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _feeRepository = scope.ServiceProvider.GetRequiredService<IFeeRepository>();

            // Arrange

            // Act
            var result = await _feeRepository.AddAsync(fee);
            var expected = await _feeRepository.GetByIdAsync(fee.Id);

            // Assert
            result.Should().NotBeNull();
            expected.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Fact, Order(2)]
        public async Task GivenRequestIsValid_WhenGetAllAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _feeRepository = scope.ServiceProvider.GetRequiredService<IFeeRepository>();

            // Arrange

            // Act
            var result = await _feeRepository.GetAllAsync();

            // Assert
            result.Should().NotBeNull().And.HaveCount(1);
        }

        [Fact, Order(3)]
        public async Task GivenRequestIsValidAndExpressionIsExists_WhenGetAllAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _feeRepository = scope.ServiceProvider.GetRequiredService<IFeeRepository>();

            // Arrange
            Expression<Func<FeeModel, bool>> expression = s => s.CustomVehicleTypeId == fee.CustomVehicleTypeId;

            // Act
            var result = await _feeRepository.GetAllAsync(expression);

            // Assert
            result.Should().NotBeNull().And.HaveCount(1);
        }

        [Fact, Order(4)]
        public async Task GivenRequestIsValid_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _feeRepository = scope.ServiceProvider.GetRequiredService<IFeeRepository>();

            // Arrange

            // Act
            var result = await _feeRepository.GetByIdAsync(fee.Id);

            // Assert
            result.Should().NotBeNull().And.BeEquivalentTo(fee);
        }

        [Fact, Order(5)]
        public async Task GivenRequestIsValid_WhenUpdateAsyncIsCalled_ThenRecordUpdatedSuccessful()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _feeRepository = scope.ServiceProvider.GetRequiredService<IFeeRepository>();

            // Arrange
            fee.Seat = 7;
            fee.Weight = 950;

            // Act
            await _feeRepository.UpdateAsync(fee);
            var expected = await _feeRepository.GetByIdAsync(fee.Id);

            // Assert
            expected.Should().NotBeNull();
            expected.Should().BeEquivalentTo(fee);
        }

        [Fact, Order(6)]
        public async Task GivenRequestIsValid_WhenRemoveAsyncIsCalled_ThenRecordRemovedSuccessful()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _feeRepository = scope.ServiceProvider.GetRequiredService<IFeeRepository>();

            // Arrange
            fee.Seat = 7;
            fee.Weight = 950;

            // Act
            await _feeRepository.RemoveAsync(fee);
            var expected = await _feeRepository.GetByIdAsync(fee.Id);

            // Assert
            expected.Should().BeNull();
        }
    }
}
