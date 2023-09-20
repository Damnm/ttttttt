using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Models.Fees;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Fees;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Repositories.Fees
{
    public class FeeRepositoryTests
    {
        #region Init mock data
        private Mock<CoreDbContext> _dbContextMock = new Mock<CoreDbContext>();
        private Mock<ILogger<FeeRepository>> _loggerMock = new Mock<ILogger<FeeRepository>>();
        private Mock<DbSet<FeeModel>>? _dbSetMock;
        #endregion

        #region Init Data mock
        private List<FeeModel> entities = new List<FeeModel>
        {
            new FeeModel()
            {
                Id = Guid.NewGuid(),
                CreatedDate = new DateTime(2023,9,11),
                Amount = 15000,
                ConfidenceScore = (float?)0.9,
                CustomVehicleTypeId = Guid.NewGuid(),
                Duration = 15000,
                EmployeeId = "Some employee",
                ObjectId = Guid.NewGuid(),
                VehicleCategoryId = Guid.NewGuid(),
                LaneInId = "0301",
                LaneInDate = new DateTime(2023,9,14),
                LaneInEpoch = 1694649600,
                LaneOutId = "0302",
                LaneOutDate = new DateTime(2023,9,14,1,13,32),
                LaneOutEpoch = 1694654012,
                RFID = "023156456956875",
                PlateNumber = "29A23541"
            }
        };
        private readonly Exception _exception = null!;
        #endregion

        #region GetAllAsync
        [Fact]
        public async void GivenRequestIsValid_WhenGetAllAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var data = entities.FirstOrDefault()!;
            _dbSetMock = EFTestHelper.GetMockDbSet(entities);
            _dbContextMock.Setup(x => x.Fees).Returns(_dbSetMock.Object);

            // Act
            var repository = new FeeRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = await repository.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.First().Should().BeEquivalentTo(data);
            _dbContextMock.Verify(x => x.Fees, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.GetAllAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.GetAllAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async void GivenRequestIsValidAndExpressionIsExists_WhenGetAllAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var data = entities.FirstOrDefault()!;
            _dbSetMock = EFTestHelper.GetMockDbSet(entities);
            _dbContextMock.Setup(x => x.Fees).Returns(_dbSetMock.Object);

            Expression<Func<FeeModel, bool>> expression = s => s.Id == data.Id;

            // Act
            var repository = new FeeRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = await repository.GetAllAsync(expression);

            // Assert
            result.Should().NotBeNull();
            result.First().Should().BeEquivalentTo(data);
            _dbContextMock.Verify(x => x.Fees, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.GetAllAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.GetAllAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenRequestIsValidAndDBContextIsDown_WhenGetAllAsyncIsCalled_ThenThrowETCEPAYCoreAPIException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.Fees).Throws(someEx);

            // Act
            var repository = new FeeRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await repository.GetAllAsync();

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.Fees, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.GetAllAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.GetAllAsync)} method", Times.Once, _exception);
        }
        #endregion

        #region GetByIdAsync
        [Fact]
        public async void GivenRequestIsValid_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var data = entities.FirstOrDefault()!;
            _dbSetMock = EFTestHelper.GetMockDbSet(entities);
            _dbContextMock.Setup(x => x.Fees).Returns(_dbSetMock.Object);

            // Act
            var repository = new FeeRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = await repository.GetByIdAsync(data.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(data);
            _dbContextMock.Verify(x => x.Fees, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.GetByIdAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async void GivenRequestIsValidAndIdIsNotExists_WhenGetByIdAsyncIsCalled_ThenReturnNull()
        {
            // Arrange
            var data = entities.FirstOrDefault()!;
            _dbSetMock = EFTestHelper.GetMockDbSet(new List<FeeModel>());
            _dbContextMock.Setup(x => x.Fees).Returns(_dbSetMock.Object);

            // Act
            var repository = new FeeRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = await repository.GetByIdAsync(data.Id);

            // Assert
            result.Should().BeNull();
            _dbContextMock.Verify(x => x.Fees, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.GetByIdAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenRequestIsValidAndDBContextIsDown_WhenGetByIdAsyncIsCalled_ThenThrowETCEPAYCoreAPIException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.Fees).Throws(someEx);

            // Act
            var repository = new FeeRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await repository.GetByIdAsync(It.IsNotNull<Guid>());

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.Fees, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.GetByIdAsync)} method", Times.Once, _exception);
        }
        #endregion

        #region AddAsync
        [Fact]
        public async void GivenRequestIsValid_WhenAddAsyncIsCalled_ThenRecordAddedSuccessfulAndReturnCorrectResult()
        {
            // Arrange
            var data = entities.FirstOrDefault()!;
            _dbSetMock = EFTestHelper.GetMockDbSet(entities);
            _dbContextMock.Setup(x => x.Fees).Returns(_dbSetMock.Object);

            // Act
            var repository = new FeeRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = await repository.AddAsync(data);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(data);
            _dbContextMock.Verify(x => x.Fees, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.AddAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.AddAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenRequestIsValidAndDBContextIsDown_WhenAddAsyncIsCalled_ThenThrowETCEPAYCoreAPIException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.Fees).Throws(someEx);

            // Act
            var repository = new FeeRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await repository.AddAsync(It.IsNotNull<FeeModel>());

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.Fees, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.AddAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.AddAsync)} method", Times.Once, _exception);
        }
        #endregion

        #region UpdateAsync
        [Fact]
        public async void GivenRequestIsValid_WhenUpdateAsyncIsCalled_ThenRecordUpdatedSuccessful()
        {
            // Arrange
            var data = entities.FirstOrDefault()!;
            _dbSetMock = EFTestHelper.GetMockDbSet(entities);
            _dbContextMock.Setup(x => x.Fees).Returns(_dbSetMock.Object);

            // Act
            var repository = new FeeRepository(_loggerMock.Object, _dbContextMock.Object);
            await repository.UpdateAsync(data);

            // Assert
            _dbContextMock.Verify(x => x.Fees, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.UpdateAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.UpdateAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenRequestIsValidAndDBContextIsDown_WhenUpdateAsyncIsCalled_ThenThrowETCEPAYCoreAPIException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.Fees).Throws(someEx);

            // Act
            var repository = new FeeRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await repository.UpdateAsync(It.IsNotNull<FeeModel>());

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.Fees, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.UpdateAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.UpdateAsync)} method", Times.Once, _exception);
        }
        #endregion

        #region RemoveAsync
        [Fact]
        public async void GivenRequestIsValid_WhenRemoveAsyncIsCalled_ThenRecordRemovedSuccessful()
        {
            // Arrange
            var data = entities.FirstOrDefault()!;
            _dbSetMock = EFTestHelper.GetMockDbSet(entities);
            _dbContextMock.Setup(x => x.Fees).Returns(_dbSetMock.Object);

            // Act
            var repository = new FeeRepository(_loggerMock.Object, _dbContextMock.Object);
            await repository.RemoveAsync(data);

            // Assert
            _dbContextMock.Verify(x => x.Fees, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.RemoveAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.RemoveAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenRequestIsValidAndDBContextIsDown_WhenRemoveAsyncIsCalled_ThenThrowETCEPAYCoreAPIException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.Fees).Throws(someEx);

            // Act
            var repository = new FeeRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await repository.RemoveAsync(It.IsNotNull<FeeModel>());

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.Fees, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.RemoveAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.RemoveAsync)} method", Times.Once, _exception);
        }
        #endregion
    }
}
