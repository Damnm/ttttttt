using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Models.Employees;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Employees;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Repositories.Employees
{
    public class EmployeeRepositoryTests
    {
        #region Init mock data
        private Mock<CoreDbContext> _dbContextMock = new Mock<CoreDbContext>();
        private Mock<ILogger<EmployeeRepository>> _loggerMock = new Mock<ILogger<EmployeeRepository>>();
        private Mock<DbSet<EmployeeModel>>? _dbSetMock;
        #endregion

        #region Init Data mock
        private List<EmployeeModel> entities = new List<EmployeeModel>
        {
            new EmployeeModel()
            {
                Id = "Some Id",
                CreatedDate = new DateTime(2023, 9, 11),
                Address = "Some addr",
                Birthday = new DateTime(2000,1,1),
                Email = "Some email",
                FirstName = "Some first name",
                Gender = 1,
                LastName = "Some last name",
                Mobile = "Some mobile",
                NickName = "Some nick name",
                Password = "Some password",
                Salt = "Some salt",
                StationId = "00",
                UserName = "username",
                TeamId = "00"
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
            _dbContextMock.Setup(x => x.Employees).Returns(_dbSetMock.Object);

            // Act
            var repository = new EmployeeRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = await repository.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.First().Should().BeEquivalentTo(data);
            _dbContextMock.Verify(x => x.Employees, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.GetAllAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.GetAllAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async void GivenRequestIsValidAndExpressionIsExists_WhenGetAllAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var data = entities.FirstOrDefault()!;
            _dbSetMock = EFTestHelper.GetMockDbSet(entities);
            _dbContextMock.Setup(x => x.Employees).Returns(_dbSetMock.Object);

            Expression<Func<EmployeeModel, bool>> expression = s => s.Id == data.Id;

            // Act
            var repository = new EmployeeRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = await repository.GetAllAsync(expression);

            // Assert
            result.Should().NotBeNull();
            result.First().Should().BeEquivalentTo(data);
            _dbContextMock.Verify(x => x.Employees, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.GetAllAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.GetAllAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenRequestIsValidAndDBContextIsDown_WhenGetAllAsyncIsCalled_ThenThrowETCEPAYCoreAPIException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.Employees).Throws(someEx);

            // Act
            var repository = new EmployeeRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await repository.GetAllAsync();

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.Employees, Times.Once);
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
            _dbContextMock.Setup(x => x.Employees).Returns(_dbSetMock.Object);

            // Act
            var repository = new EmployeeRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = await repository.GetByIdAsync(data.Id ?? string.Empty);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(data);
            _dbContextMock.Verify(x => x.Employees, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.GetByIdAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async void GivenRequestIsValidAndIdIsNotExists_WhenGetByIdAsyncIsCalled_ThenReturnNull()
        {
            // Arrange
            var data = entities.FirstOrDefault()!;
            _dbSetMock = EFTestHelper.GetMockDbSet(new List<EmployeeModel>());
            _dbContextMock.Setup(x => x.Employees).Returns(_dbSetMock.Object);

            // Act
            var repository = new EmployeeRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = await repository.GetByIdAsync(data.Id ?? string.Empty);

            // Assert
            result.Should().BeNull();
            _dbContextMock.Verify(x => x.Employees, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.GetByIdAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenRequestIsValidAndDBContextIsDown_WhenGetByIdAsyncIsCalled_ThenThrowETCEPAYCoreAPIException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.Employees).Throws(someEx);

            // Act
            var repository = new EmployeeRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await repository.GetByIdAsync(It.IsNotNull<string>());

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.Employees, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(repository.GetByIdAsync)} method...", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(repository.GetByIdAsync)} method", Times.Once, _exception);
        }
        #endregion
    }
}
