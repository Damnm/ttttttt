using Azure.Core;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Fees;
using EPAY.ETC.Core.API.Core.Models.Fees;
using EPAY.ETC.Core.API.IntegrationTests.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using XUnitPriorityOrderer;
using CoreModel = EPAY.ETC.Core.Models.Fees;

namespace EPAY.ETC.Core.API.IntegrationTests.Services.Fees
{
    [TestCaseOrderer(CasePriorityOrderer.TypeName, CasePriorityOrderer.AssembyName)]
    public class FeeServiceTests : IntegrationTestBase
    {
        private IFeeService _service;

        #region Init test data
        private static Guid testFeeId = Guid.Parse("b31717fb-cffd-4094-89d9-775a62d89399");
        private static Guid feeId = Guid.Parse("a71717fb-cffd-4994-88d9-775a62d89399");
        private CoreModel.FeeModel request = new CoreModel.FeeModel()
        {
            CreatedDate = new DateTime(2023, 9, 15, 3, 6, 8),
            EmployeeId = "Some employee",
            FeeId = feeId,
            ObjectId = Guid.Parse("a71717fb-cffd-4994-19d9-775a62d89399"),
            ShiftId = Guid.Parse("a71717fb-cffd-4994-89d9-775a62d89399"),
            Payment = new CoreModel.PaymentModel()
            {
                Duration = 32541,
                Model = "Some model",
                PlateNumber = "Some plate number",
                Amount = 9000
            }
        };
        #endregion

        #region AddAsync
        [Fact, Order(0)]
        public async Task AddTestData()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _service = scope.ServiceProvider.GetRequiredService<IFeeService>();

            // Arrange
            request.FeeId = testFeeId;
            request.ObjectId = Guid.NewGuid();

            // Act
            var result = await _service.AddAsync(request);
            var expected = await _service.GetByIdAsync(testFeeId);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            expected.Should().NotBeNull();
            expected.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull().And.BeEquivalentTo(expected.Data);
        }
        [Fact, Order(1)]
        public async Task GivenRequestIsValid_WhenAddAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _service = scope.ServiceProvider.GetRequiredService<IFeeService>();

            // Arrange

            // Act
            var result = await _service.AddAsync(request);
            var expected = await _service.GetByIdAsync(feeId);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            expected.Should().NotBeNull();
            expected.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull().And.BeEquivalentTo(expected.Data);
        }

        [Fact, Order(2)]
        public async Task GivenRequestIsValidAndObjectIdIsExists_WhenAddAsyncIsCalled_ThenReturnConflict()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _service = scope.ServiceProvider.GetRequiredService<IFeeService>();

            // Arrange
            request.FeeId = Guid.NewGuid();

            // Act
            var result = await _service.AddAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Errors.Count(x => x.Code == StatusCodes.Status409Conflict).Should().BeGreaterThan(0);
        }
        #endregion

        #region UpdateAsync
        [Fact, Order(3)]
        public async Task GivenRequestIsValid_WhenUpdateAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _service = scope.ServiceProvider.GetRequiredService<IFeeService>();

            // Arrange
            request!.Payment!.TicketId = "123145687";

            // Act
            var result = await _service.UpdateAsync(feeId, request);
            var expected = await _service.GetByIdAsync(feeId);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            expected.Should().NotBeNull();
            expected.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull().And.BeEquivalentTo(expected.Data);
        }

        [Fact, Order(4)]
        public async Task GivenRequestIsValidAndObjectIdIsExists_WhenUpdateAsyncIsCalled_ThenReturnConflict()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _service = scope.ServiceProvider.GetRequiredService<IFeeService>();

            // Arrange

            // Act
            var result = await _service.UpdateAsync(testFeeId, request);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Errors.Count(x => x.Code == StatusCodes.Status409Conflict).Should().BeGreaterThan(0);
        }

        [Fact, Order(5)]
        public async Task GivenRequestIsValidAndFeeIdIsNotExists_WhenUpdateAsyncIsCalled_ThenReturnNotFound()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _service = scope.ServiceProvider.GetRequiredService<IFeeService>();

            // Arrange

            // Act
            var result = await _service.UpdateAsync(Guid.NewGuid(), request);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Errors.Count(x => x.Code == StatusCodes.Status404NotFound).Should().BeGreaterThan(0);
        }
        #endregion

        #region GetAllAsync
        [Fact, Order(6)]
        public async Task GivenRequestIsValid_WhenGetAllAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _service = scope.ServiceProvider.GetRequiredService<IFeeService>();

            // Arrange

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(2);
        }

        [Fact, Order(7)]
        public async Task GivenRequestIsValidAndExpressionAlreadyExists_WhenGetAllAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _service = scope.ServiceProvider.GetRequiredService<IFeeService>();

            // Arrange

            // Act
            var result = await _service.GetAllAsync((Expression<Func<FeeModel, bool>>)(s => s.Id == feeId));

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull().And.HaveCount(1);
        }
        #endregion

        #region GetByIdAsync
        [Fact, Order(8)]
        public async Task GivenRequestIsValid_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _service = scope.ServiceProvider.GetRequiredService<IFeeService>();

            // Arrange

            // Act
            var result = await _service.GetByIdAsync(feeId);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data?.Id.Should().Be(feeId);
        }

        [Fact, Order(9)]
        public async Task GivenRequestIsValidAndFeeIdIsNotExists_WhenGetByIdAsyncIsCalled_ThenReturnNull()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _service = scope.ServiceProvider.GetRequiredService<IFeeService>();

            // Arrange

            // Act
            var result = await _service.GetByIdAsync(Guid.NewGuid());

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeNull();
        }
        #endregion

        #region RemoveAsync
        [Fact, Order(10)]
        public async Task GivenRequestIsValid_WhenRemoveAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _service = scope.ServiceProvider.GetRequiredService<IFeeService>();

            // Arrange

            // Act
            var result = await _service.RemoveAsync(feeId);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeNull();
        }

        [Fact, Order(11)]
        public async Task GivenRequestIsValidAndFeeIdIsNotExists_WhenRemoveAsyncIsCalled_ThenReturnNull()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _service = scope.ServiceProvider.GetRequiredService<IFeeService>();

            // Arrange

            // Act
            var result = await _service.RemoveAsync(Guid.NewGuid());

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Data.Should().BeNull();
        }
        #endregion

        #region RemoveTest
        [Fact, Order(10)]
        public async Task RemoveTestData()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _service = scope.ServiceProvider.GetRequiredService<IFeeService>();

            // Arrange

            // Act
            var result = await _service.RemoveAsync(testFeeId);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeNull();
        }
        #endregion
    }
}
