using EPAY.ETC.Core.API.Core.Models.Common;
using EPAY.ETC.Core.API.Core.Models.SearchRequest;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Vehicle;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.OrderBuilder;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Common;
using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Core.Models.Vehicle;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Repositories.Vehicle
{
    public class VehicleRepositoryTests : AutoMapperTestBase
    {
        #region Init mock data
        private Mock<CoreDbContext> _dbContextMock = new Mock<CoreDbContext>();
        private Mock<ILogger<VehicleRepository>> _loggerMock = new Mock<ILogger<VehicleRepository>>();
        private Mock<DbSet<VehicleModel>>? _dbVehicleSetMock;
        private List<VehicleModel> _vehicles = new List<VehicleModel>()
        {
            new VehicleModel()
            {
                Id = Guid.NewGuid(),
                RFID = "1245asdasda",
                CreatedDate = DateTime.Now,
                PlateNumber = "Some Plate number",
                PlateColor = "Some Plate colors",
                Make = "Some make",
                Seat = 7,
                Weight = 2000,
                VehicleType = "Loại 1"
            },
            new VehicleModel()
            {
                Id = Guid.NewGuid(),
                RFID = "1245asdasda",
                CreatedDate = DateTime.Now,
                PlateNumber = "Some Plate number",
                PlateColor = "Some Plate colors",
                Make = "Some make",
                Seat = 11,
                Weight = 2000,
                VehicleType = "Loại 2"
            },
        };
        #endregion

        #region GetByIdAsync
        [Fact]
        public async void GivenValidRequest_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var data = _vehicles.FirstOrDefault();
            _dbVehicleSetMock = EFTestHelper.GetMockDbSet(_vehicles);
            _dbContextMock.Setup(x => x.Vehicles).Returns(_dbVehicleSetMock.Object);
             
            // Act
            var vehicleRepository = new VehicleRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = await vehicleRepository.GetByIdAsync(data!.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(data);
            _dbContextMock.Verify(x => x.Vehicles, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleRepository.GetByIdAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleRepository.GetByIdAsync)} method", Times.Never, null);
        }
        [Fact]
        public async void GivenValidRequestAndVehicleRepositoryIsDown_WhenGetByIdAsyncIsCalled_ThenThowETCEPAYCoreAPIException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.Vehicles).Throws(someEx);

            // Act
            var vehicleRepository = new VehicleRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await vehicleRepository.GetByIdAsync(It.IsNotNull<Guid>());

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.Vehicles, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleRepository.GetByIdAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleRepository.GetByIdAsync)} method", Times.Once, null);
        }
        #endregion        

        #region AddAsync
        [Fact]
        public async void GivenValidEntity_WhenAddAsyncIsCalled_ThenRecordAddedSuccessfullyAndReturnCorrectResult()
        {
            // Arrange
            var data = _vehicles.FirstOrDefault();
            _dbVehicleSetMock = EFTestHelper.GetMockDbSet(_vehicles);
            _dbContextMock.Setup(x => x.Vehicles).Returns(_dbVehicleSetMock.Object);

            // Act
            var vehicleRepository = new VehicleRepository(_loggerMock.Object, _dbContextMock.Object);
            var result = await vehicleRepository.AddAsync(data!);

            // Assert
            result.Should().NotBeNull().And.BeEquivalentTo(data);
            _dbContextMock.Verify(x => x.Vehicles, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleRepository.AddAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleRepository.AddAsync)} method", Times.Never, null);
        }

        [Fact]
        public async void GivenValidEntityAndVehicleRepositotyIsDown_WhenAddAsyncIsCalled_ThenThrowETCEPAYCoreAPIException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.Vehicles).Throws(someEx);

            // Act
            var vehicleRepository = new VehicleRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await vehicleRepository.AddAsync(It.IsAny<VehicleModel>());

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.Vehicles, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleRepository.AddAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleRepository.AddAsync)} method", Times.Once, null);
        }
        #endregion

        #region UpdateAsync
        [Fact]
        public async void GivenValidEntity_WhenUpdateAsyncIsCalled_ThenRecordUpdatedSuccessfully()
        {
            // Arrange
            var data = _vehicles.FirstOrDefault();
            _dbVehicleSetMock = EFTestHelper.GetMockDbSet(_vehicles);
            _dbContextMock.Setup(x => x.Vehicles).Returns(_dbVehicleSetMock.Object);

            // Act
            var vehicleRepository = new VehicleRepository(_loggerMock.Object, _dbContextMock.Object);
            await vehicleRepository.UpdateAsync(data!);

            // Assert
            _dbContextMock.Verify(x => x.Vehicles, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleRepository.UpdateAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleRepository.UpdateAsync)} method", Times.Never, null);
        }

        [Fact]
        public async void GivenValidEntityAndVehicleRepositotyIsDown_WhenUpdateAsyncIsCalled_ThenThowETCEPAYCoreAPIException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.Vehicles).Throws(someEx);

            // Act
            var vehicleRepository = new VehicleRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await vehicleRepository.UpdateAsync(It.IsAny<VehicleModel>());

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.Vehicles, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleRepository.UpdateAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleRepository.UpdateAsync)} method", Times.Once, null);
        }
        #endregion

        #region RemoveAsync
        [Fact]
        public async void GivenValidEntity_WhenRemoveAsyncIsCalled_ThenRecordRemovedSuccessfully()
        {
            // Arrange
            var data = _vehicles.FirstOrDefault();
            _dbVehicleSetMock = EFTestHelper.GetMockDbSet(_vehicles);
            _dbContextMock.Setup(x => x.Vehicles).Returns(_dbVehicleSetMock.Object);

            // Act
            var vehicleRepository = new VehicleRepository(_loggerMock.Object, _dbContextMock.Object);
            await vehicleRepository.RemoveAsync(data!);

            // Assert
            _dbContextMock.Verify(x => x.Vehicles, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleRepository.RemoveAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleRepository.RemoveAsync)} method", Times.Never, null);
        }

        [Fact]
        public async void GivenValidEntityAndPriorityVehicleRepositoryIsDown_WhenRemoveAsyncIsCalled_ThenThrowETCEPAYCoreAPIException()
        {
            // Arrange
            var someEx = new ETCEPAYCoreAPIException(99, "Some exception");
            _dbContextMock.Setup(x => x.Vehicles).Throws(someEx);

            // Act
            var vehicleRepository = new VehicleRepository(_loggerMock.Object, _dbContextMock.Object);
            Func<Task> func = async () => await vehicleRepository.RemoveAsync(It.IsAny<VehicleModel>());

            // Assert
            var ex = await Assert.ThrowsAsync<ETCEPAYCoreAPIException>(func);
            _dbContextMock.Verify(x => x.Vehicles, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(vehicleRepository.RemoveAsync)} method...", Times.Once, null);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(vehicleRepository.RemoveAsync)} method", Times.Once, null);
        }
        #endregion
    }
}