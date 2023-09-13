using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Models.Enum;
using EPAY.ETC.Core.API.Core.Models.FeeTypes;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.FeeTypes;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Repositories.FeeTypes
{
    public class FeeTypeRepositoryTests
    {
        #region Init mock data
        private Mock<CoreDbContext> _dbContextMock = new Mock<CoreDbContext>();
        private Mock<ILogger<FeeTypeRepository>> _loggerMock = new Mock<ILogger<FeeTypeRepository>>();
        private Mock<DbSet<FeeTypeModel>>? _dbSetMock;
        #endregion

        #region Init Data mock
        private List<FeeTypeModel> entities = new List<FeeTypeModel>
        {
            new FeeTypeModel()
            {
                Id = Guid.NewGuid(),
                Name = FeeTypeEnum.TimeBlock,
                CreatedDate = new DateTime(2023,9,11)
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
            _dbContextMock.Setup(x => x.FeeTypes).Returns(_dbSetMock.Object);

            // Act
            var repository = new FeeTypeRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = await repository.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.First().Should().BeEquivalentTo(data);
            _dbContextMock.Verify(x => x.FeeTypes, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.GetAllAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.GetAllAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async void GivenRequestIsValidAndExpressionIsExists_WhenGetAllAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var data = entities.FirstOrDefault()!;
            _dbSetMock = EFTestHelper.GetMockDbSet(entities);
            _dbContextMock.Setup(x => x.FeeTypes).Returns(_dbSetMock.Object);

            Expression<Func<FeeTypeModel, bool>> expression = s => s.Id == data.Id;

            // Act
            var repository = new FeeTypeRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = await repository.GetAllAsync(expression);

            // Assert
            result.Should().NotBeNull();
            result.First().Should().BeEquivalentTo(data);
            _dbContextMock.Verify(x => x.FeeTypes, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.GetAllAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.GetAllAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenRequestIsValidAndDBContextIsDown_WhenGetAllAsyncIsCalled_ThenThrowExceptionAsync()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.FeeTypes).Throws(someEx);

            // Act
            var repository = new FeeTypeRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await repository.GetAllAsync();

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.FeeTypes, Times.Once);
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
            _dbContextMock.Setup(x => x.FeeTypes).Returns(_dbSetMock.Object);

            // Act
            var repository = new FeeTypeRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = await repository.GetByIdAsync(data.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(data);
            _dbContextMock.Verify(x => x.FeeTypes, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.GetByIdAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async void GivenRequestIsValidAndIdIsNotExists_WhenGetByIdAsyncIsCalled_ThenReturnNull()
        {
            // Arrange
            var data = entities.FirstOrDefault()!;
            _dbSetMock = EFTestHelper.GetMockDbSet(new List<FeeTypeModel>());
            _dbContextMock.Setup(x => x.FeeTypes).Returns(_dbSetMock.Object);

            // Act
            var repository = new FeeTypeRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = await repository.GetByIdAsync(data.Id);

            // Assert
            result.Should().BeNull();
            _dbContextMock.Verify(x => x.FeeTypes, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.GetByIdAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenRequestIsValidAndDBContextIsDown_WhenGetByIdAsyncIsCalled_ThenThrowExceptionAsync()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.FeeTypes).Throws(someEx);

            // Act
            var repository = new FeeTypeRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await repository.GetByIdAsync(It.IsNotNull<Guid>());

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.FeeTypes, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.GetByIdAsync)} method", Times.Once, _exception);
        }
        #endregion
    }
}
