using EPAY.ETC.Core.API.Core.Models.Enum;
using EPAY.ETC.Core.API.Core.Models.FeeVehicleCategories;
using EPAY.ETC.Core.API.Core.Models.TimeBlockFees;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.FeeVehicleCategories;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.TimeBlockFees;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Vehicle;
using EPAY.ETC.Core.API.Infrastructure.Services.Fees;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Helpers;
using EPAY.ETC.Core.Models.Enums;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Services.Fees
{
    public class FeeCalculationServiceTests
    {
        #region Init Method mock
        private readonly Mock<ILogger<FeeCalculationService>> _loggerMock = new();
        private readonly Mock<IFeeVehicleCategoryRepository> _feeVehicleCategoryRepositoryMock = new();
        private readonly Mock<ITimeBlockFeeRepository> _timeBlockFeeRepositoryMock = new();
        private readonly Mock<IVehicleRepository> _vehicleRepositoryMock = new();
        #endregion

        #region Init Data mock
        private readonly Exception _exception = null!;
        private static Guid customerVehicleTypeId = Guid.NewGuid();
        private List<FeeVehicleCategoryModel> feeVehicleCategories = new List<FeeVehicleCategoryModel>()
        {
            new FeeVehicleCategoryModel()
            {
                Id = Guid.NewGuid(),
                VehicleCategoryId = Guid.NewGuid(),
                FeeTypeId = Guid.NewGuid(),
                VehicleGroupId = Guid.NewGuid(),
                CustomVehicleTypeId = customerVehicleTypeId,
                PlateNumber = "30A11111",
                ValidFrom = new DateTime(2023,9,1),
                CreatedDate = new DateTime(2023,9,11),
                FeeType = new Core.Models.FeeTypes.FeeTypeModel()
                {
                    Id = Guid.NewGuid(),
                    Name = FeeTypeEnum.TimeBlock,
                    Amount = 0,
                    CreatedDate = new DateTime(2023,9,11)
                }
            }
        };
        private List<TimeBlockFeeModel> timeBlockFees = new List<TimeBlockFeeModel>
        {
            new TimeBlockFeeModel()
            {
                Id = Guid.NewGuid(),
                CustomVehicleTypeId = customerVehicleTypeId,
                FromSecond = 0,
                ToSecond = 599,
                Amount=9000,
                BlockDurationInSeconds = 1800,
                CreatedDate = new DateTime(2023,9,11)
            },
            new TimeBlockFeeModel()
            {
                Id = Guid.NewGuid(),
                CustomVehicleTypeId = customerVehicleTypeId,
                FromSecond = 600,
                ToSecond = 3599,
                Amount = 14000,
                BlockDurationInSeconds = 1800,
                CreatedDate = new DateTime(2023,9,11)
            },
            new TimeBlockFeeModel()
            {
                Id = Guid.NewGuid(),
                CustomVehicleTypeId = customerVehicleTypeId,
                FromSecond = 3600,
                ToSecond = 5399,
                Amount = 21000,
                BlockDurationInSeconds = 1800,
                CreatedDate = new DateTime(2023,9,11)
            },
            new TimeBlockFeeModel()
            {
                Id = Guid.NewGuid(),
                CustomVehicleTypeId = customerVehicleTypeId,
                FromSecond = 5400,
                ToSecond = 7199,
                Amount = 28000,
                BlockDurationInSeconds = 1800,
                CreatedDate = new DateTime(2023,9,11)
            }
        };
        private List<VehicleModel> vehicles = new List<VehicleModel>()
        {
            new VehicleModel()
            {
                Id= Guid.NewGuid(),
                CreatedDate = new DateTime(2023,9,11),
                RFID = "1253456987215",
                Make = "Some make",
                PlateColor = "Some plate color",
                PlateNumber = "30A00000",
                Seat = 16,
                VehicleType = "Some vehicle"
            }
        };
        private long startEpochTime = 1694403911;
        private long endEpochTime = 1694414211;
        #endregion

        #region CalculateFeeAsync has existed RFID
        [Theory]
        [InlineData(FeeTypeEnum.Free, 0)]
        [InlineData(FeeTypeEnum.Fixed, 10000)]
        [InlineData(FeeTypeEnum.TimeBlock, 42000)]
        public async Task GivenRequestIsValidAndFeeVehicleCategoryIsExists_WhenCalculateFeeAsyncIsCalled_ThenReturnCorrectResult(FeeTypeEnum feeType, double amount)
        {
            // Arrange
            var feeVehicleCategory = feeVehicleCategories.FirstOrDefault()!;
            feeVehicleCategory.FeeType!.Name = feeType;
            feeVehicleCategory.FeeType!.Amount = amount;

            _feeVehicleCategoryRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeVehicleCategoryModel, bool>>>())).ReturnsAsync(new List<FeeVehicleCategoryModel>() { feeVehicleCategory });
            _timeBlockFeeRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<TimeBlockFeeModel, bool>>>())).ReturnsAsync(timeBlockFees);
            _vehicleRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<VehicleModel, bool>>>())).ReturnsAsync(vehicles);

            int duration = Convert.ToInt32(endEpochTime - startEpochTime);

            // Act
            var service = new FeeCalculationService(_loggerMock.Object, _feeVehicleCategoryRepositoryMock.Object, _timeBlockFeeRepositoryMock.Object, _vehicleRepositoryMock.Object);
            var result = await service.CalculateFeeAsync(It.IsAny<string>(), It.IsAny<string>(), startEpochTime, endEpochTime);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Fee.Should().NotBeNull();
            result.Data.Fee?.Amount.Should().Be(amount);
            result.Data.Fee?.Duration.Should().Be(duration);
            result.Data.Fee?.Currency.Should().Be(CurrencyEnum.VND.ToString());
            result.Data.Vehicle.Should().NotBeNull();
            result.Data.Vehicle?.RFID.Should().Be(feeVehicleCategory.RFID);
            result.Data.Vehicle?.PlateNumber.Should().Be(feeVehicleCategory.PlateNumber);

            switch (feeType)
            {
                case FeeTypeEnum.Free:
                case FeeTypeEnum.Fixed:
                    _feeVehicleCategoryRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeVehicleCategoryModel, bool>>>()), Times.Once);
                    break;

                case FeeTypeEnum.TimeBlock:
                    _feeVehicleCategoryRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeVehicleCategoryModel, bool>>>()), Times.Once);
                    _timeBlockFeeRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<TimeBlockFeeModel, bool>>>()), Times.Once);
                    break;
            }
            _vehicleRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<VehicleModel, bool>>>()), Times.Never);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.CalculateFeeAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.CalculateFeeAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenRequestIsValidAndFeeVehicleCategoryIsNotExistsAndVehicleIsExists_WhenCalculateFeeAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var vehicle = vehicles.FirstOrDefault()!;
            timeBlockFees = new List<TimeBlockFeeModel>
            {
                new TimeBlockFeeModel()
                {
                    Id = Guid.NewGuid(),
                    CustomVehicleTypeId = customerVehicleTypeId,
                    FromSecond = 0,
                    ToSecond = 599,
                    Amount=14000,
                    BlockDurationInSeconds = 1800,
                    CreatedDate = new DateTime(2023,9,11)
                },
                new TimeBlockFeeModel()
                {
                    Id = Guid.NewGuid(),
                    CustomVehicleTypeId = customerVehicleTypeId,
                    FromSecond = 600,
                    ToSecond = 3599,
                    Amount = 19000,
                    BlockDurationInSeconds = 1800,
                    CreatedDate = new DateTime(2023,9,11)
                },
                new TimeBlockFeeModel()
                {
                    Id = Guid.NewGuid(),
                    CustomVehicleTypeId = customerVehicleTypeId,
                    FromSecond = 3600,
                    ToSecond = 5399,
                    Amount = 28000,
                    BlockDurationInSeconds = 1800,
                    CreatedDate = new DateTime(2023,9,11)
                },
                new TimeBlockFeeModel()
                {
                    Id = Guid.NewGuid(),
                    CustomVehicleTypeId = customerVehicleTypeId,
                    FromSecond = 5400,
                    ToSecond = 7199,
                    Amount = 37000,
                    BlockDurationInSeconds = 1800,
                    CreatedDate = new DateTime(2023,9,11)
                }
            };

            _feeVehicleCategoryRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeVehicleCategoryModel, bool>>>())).ReturnsAsync(new List<FeeVehicleCategoryModel>());
            _timeBlockFeeRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<TimeBlockFeeModel, bool>>>())).ReturnsAsync(timeBlockFees);
            _vehicleRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<VehicleModel, bool>>>())).ReturnsAsync(vehicles);

            int duration = Convert.ToInt32(endEpochTime - startEpochTime);
            double amount = 54000;

            // Act
            var service = new FeeCalculationService(_loggerMock.Object, _feeVehicleCategoryRepositoryMock.Object, _timeBlockFeeRepositoryMock.Object, _vehicleRepositoryMock.Object);
            var result = await service.CalculateFeeAsync(vehicle?.RFID ?? string.Empty, It.IsAny<string>(), startEpochTime, endEpochTime);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Fee.Should().NotBeNull();
            result.Data.Fee?.Amount.Should().Be(amount);
            result.Data.Fee?.Duration.Should().Be(duration);
            result.Data.Fee?.Currency.Should().Be(CurrencyEnum.VND.ToString());
            result.Data.Vehicle.Should().BeNull();

            _feeVehicleCategoryRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeVehicleCategoryModel, bool>>>()), Times.Once);
            _timeBlockFeeRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<TimeBlockFeeModel, bool>>>()), Times.Once);
            _vehicleRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<VehicleModel, bool>>>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.CalculateFeeAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.CalculateFeeAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenRequestIsValidAndFeeVehicleCategoryIsNotExistsAndVehicleIsNotExists_WhenCalculateFeeAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var vehicle = vehicles.FirstOrDefault()!;

            _feeVehicleCategoryRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeVehicleCategoryModel, bool>>>())).ReturnsAsync(new List<FeeVehicleCategoryModel>());
            _timeBlockFeeRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<TimeBlockFeeModel, bool>>>())).ReturnsAsync(timeBlockFees);
            _vehicleRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<VehicleModel, bool>>>())).ReturnsAsync(new List<VehicleModel>());

            int duration = Convert.ToInt32(endEpochTime - startEpochTime);
            double amount = 42000;

            // Act
            var service = new FeeCalculationService(_loggerMock.Object, _feeVehicleCategoryRepositoryMock.Object, _timeBlockFeeRepositoryMock.Object, _vehicleRepositoryMock.Object);
            var result = await service.CalculateFeeAsync(vehicle?.RFID ?? string.Empty, It.IsAny<string>(), startEpochTime, endEpochTime);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Fee.Should().NotBeNull();
            result.Data.Fee?.Amount.Should().Be(amount);
            result.Data.Fee?.Duration.Should().Be(duration);
            result.Data.Fee?.Currency.Should().Be(CurrencyEnum.VND.ToString());
            result.Data.Vehicle.Should().BeNull();

            _feeVehicleCategoryRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeVehicleCategoryModel, bool>>>()), Times.Once);
            _timeBlockFeeRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<TimeBlockFeeModel, bool>>>()), Times.Once);
            _vehicleRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<VehicleModel, bool>>>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.CalculateFeeAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.CalculateFeeAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenRequestIsValidAndFeeVehicleCategoryIsNotExistsAndVehicleIsNotExistsAndTimeBlockFeeIsNotExists_WhenCalculateFeeAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            var vehicle = vehicles.FirstOrDefault()!;

            _feeVehicleCategoryRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeVehicleCategoryModel, bool>>>())).ReturnsAsync(new List<FeeVehicleCategoryModel>());
            _timeBlockFeeRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<TimeBlockFeeModel, bool>>>())).ReturnsAsync(new List<TimeBlockFeeModel>());
            _vehicleRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<VehicleModel, bool>>>())).ReturnsAsync(new List<VehicleModel>());

            int duration = Convert.ToInt32(endEpochTime - startEpochTime);
            double amount = 0;

            // Act
            var service = new FeeCalculationService(_loggerMock.Object, _feeVehicleCategoryRepositoryMock.Object, _timeBlockFeeRepositoryMock.Object, _vehicleRepositoryMock.Object);
            var result = await service.CalculateFeeAsync(vehicle?.RFID ?? string.Empty, It.IsAny<string>(), startEpochTime, endEpochTime);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Fee.Should().NotBeNull();
            result.Data.Fee?.Amount.Should().Be(amount);
            result.Data.Fee?.Duration.Should().Be(duration);
            result.Data.Fee?.Currency.Should().Be(CurrencyEnum.VND.ToString());
            result.Data.Vehicle.Should().BeNull();

            _feeVehicleCategoryRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeVehicleCategoryModel, bool>>>()), Times.Once);
            _timeBlockFeeRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<TimeBlockFeeModel, bool>>>()), Times.Once);
            _vehicleRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<VehicleModel, bool>>>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.CalculateFeeAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.CalculateFeeAsync)} method", Times.Never, _exception);
        }

        [Fact]
        public async Task GivenRequestIsValidAndFeeVehicleCategoryRepositoryIsDown_WhenCalculateFeeAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var vehicle = vehicles.FirstOrDefault()!;

            _feeVehicleCategoryRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeVehicleCategoryModel, bool>>>())).ThrowsAsync(new Exception("Some ex"));
            _timeBlockFeeRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<TimeBlockFeeModel, bool>>>())).ReturnsAsync(timeBlockFees);
            _vehicleRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<VehicleModel, bool>>>())).ReturnsAsync(vehicles);

            // Act
            var service = new FeeCalculationService(_loggerMock.Object, _feeVehicleCategoryRepositoryMock.Object, _timeBlockFeeRepositoryMock.Object, _vehicleRepositoryMock.Object);
            Func<Task> func = async () => await service.CalculateFeeAsync(It.IsAny<string>(), It.IsAny<string>(), startEpochTime, endEpochTime);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => func());
            ex.Message.Should().NotBeEmpty();
            ex.Message.Should().Be("Some ex");

            _feeVehicleCategoryRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeVehicleCategoryModel, bool>>>()), Times.Once);
            _timeBlockFeeRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<TimeBlockFeeModel, bool>>>()), Times.Never);
            _vehicleRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<VehicleModel, bool>>>()), Times.Never);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.CalculateFeeAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.CalculateFeeAsync)} method", Times.Once, _exception);
        }

        [Fact]
        public async Task GivenRequestIsValidAndTimeBlockFeeRepositoryIsDown_WhenCalculateFeeAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var vehicle = vehicles.FirstOrDefault()!;

            _feeVehicleCategoryRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeVehicleCategoryModel, bool>>>())).ReturnsAsync(feeVehicleCategories);
            _timeBlockFeeRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<TimeBlockFeeModel, bool>>>())).ThrowsAsync(new Exception("Some ex"));
            _vehicleRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<VehicleModel, bool>>>())).ReturnsAsync(vehicles);

            // Act
            var service = new FeeCalculationService(_loggerMock.Object, _feeVehicleCategoryRepositoryMock.Object, _timeBlockFeeRepositoryMock.Object, _vehicleRepositoryMock.Object);
            Func<Task> func = async () => await service.CalculateFeeAsync(It.IsAny<string>(), It.IsAny<string>(), startEpochTime, endEpochTime);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => func());
            ex.Message.Should().NotBeEmpty();
            ex.Message.Should().Be("Some ex");

            _feeVehicleCategoryRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeVehicleCategoryModel, bool>>>()), Times.Once);
            _timeBlockFeeRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<TimeBlockFeeModel, bool>>>()), Times.Once);
            _vehicleRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<VehicleModel, bool>>>()), Times.Never);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.CalculateFeeAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.CalculateFeeAsync)} method", Times.Once, _exception);
        }

        [Fact]
        public async Task GivenRequestIsValidAndVehicleRepositoryIsDown_WhenCalculateFeeAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var vehicle = vehicles.FirstOrDefault()!;

            _feeVehicleCategoryRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeVehicleCategoryModel, bool>>>())).ReturnsAsync(new List<FeeVehicleCategoryModel>());
            _timeBlockFeeRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<TimeBlockFeeModel, bool>>>())).ReturnsAsync(timeBlockFees);
            _vehicleRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<VehicleModel, bool>>>())).ThrowsAsync(new Exception("Some ex"));

            // Act
            var service = new FeeCalculationService(_loggerMock.Object, _feeVehicleCategoryRepositoryMock.Object, _timeBlockFeeRepositoryMock.Object, _vehicleRepositoryMock.Object);
            Func<Task> func = async () => await service.CalculateFeeAsync("Some RFID", It.IsAny<string>(), startEpochTime, endEpochTime);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => func());
            ex.Message.Should().NotBeEmpty();
            ex.Message.Should().Be("Some ex");

            _feeVehicleCategoryRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<FeeVehicleCategoryModel, bool>>>()), Times.Once);
            _timeBlockFeeRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<TimeBlockFeeModel, bool>>>()), Times.Never);
            _vehicleRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<VehicleModel, bool>>>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.CalculateFeeAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.CalculateFeeAsync)} method", Times.Once, _exception);
        }
        #endregion

        #region CalculateFeeAsync has existed CustomVehicleType
        [Fact]
        public async Task GivenRequestIsValid_WhenCalculateFeeAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _timeBlockFeeRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<TimeBlockFeeModel, bool>>>())).ReturnsAsync(timeBlockFees);

            int duration = Convert.ToInt32(endEpochTime - startEpochTime);
            double amount = 42000;

            // Act
            var service = new FeeCalculationService(_loggerMock.Object, _feeVehicleCategoryRepositoryMock.Object, _timeBlockFeeRepositoryMock.Object, _vehicleRepositoryMock.Object);
            var result = await service.CalculateFeeAsync(It.IsAny<string>(), It.IsAny<CustomVehicleTypeEnum>(), startEpochTime, endEpochTime);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Fee.Should().NotBeNull();
            result.Data.Fee?.Amount.Should().Be(amount);
            result.Data.Fee?.Duration.Should().Be(duration);
            result.Data.Fee?.Currency.Should().Be(CurrencyEnum.VND.ToString());
            result.Data.Vehicle.Should().BeNull();

            _timeBlockFeeRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<TimeBlockFeeModel, bool>>>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.CalculateFeeAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.CalculateFeeAsync)} method", Times.Never, _exception);
        }
        [Fact]
        public async Task GivenRequestIsValidAndTimeBlockFeeRepositoryIsDown_WhenCalculateFeeAsyncMethod2IsCalled_ThenThrowException()
        {
            // Arrange
            _timeBlockFeeRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<TimeBlockFeeModel, bool>>>())).ThrowsAsync(new Exception("Some ex"));

            int duration = Convert.ToInt32(endEpochTime - startEpochTime);

            // Act
            var service = new FeeCalculationService(_loggerMock.Object, _feeVehicleCategoryRepositoryMock.Object, _timeBlockFeeRepositoryMock.Object, _vehicleRepositoryMock.Object);
            Func<Task> func = async () => await service.CalculateFeeAsync(It.IsAny<string>(), It.IsAny<CustomVehicleTypeEnum>(), startEpochTime, endEpochTime);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => func());
            ex.Message.Should().NotBeEmpty();
            ex.Message.Should().Be("Some ex");

            _timeBlockFeeRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<TimeBlockFeeModel, bool>>>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.CalculateFeeAsync)} method", Times.Once, _exception);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.CalculateFeeAsync)} method", Times.Once, _exception);
        }
        #endregion
    }
}
