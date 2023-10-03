using EPAY.ETC.Core.API.Core.Models.Configs;
using EPAY.ETC.Core.API.Core.Models.CustomVehicleTypes;
using EPAY.ETC.Core.API.Core.Models.ManualBarrierControl;
using EPAY.ETC.Core.API.Core.Models.Payment;
using EPAY.ETC.Core.API.Core.Models.PaymentStatus;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.CustomVehicleTypes;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ETCCheckouts;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ManualBarrierControls;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.PaymentStatus;
using EPAY.ETC.Core.API.Infrastructure.Services.UIActions;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Common;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Helpers;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.Receipt.SessionReports;
using EPAY.ETC.Core.Models.Request;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using StackExchange.Redis;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Services.UIActions
{
    public class UIActionServiceTests : TestBase<UIActionService>
    {
        #region Init mock instance
        private readonly Mock<IPaymentStatusRepository> _paymentStatusRepositoryMock = new();
        private readonly Mock<IAppConfigRepository> _appConfigRepositoryMock = new();
        private readonly Mock<ICustomVehicleTypeRepository> _customVehicleTypeRepositoryMock = new();
        private readonly Mock<IManualBarrierControlRepository> _manualBarrierControlRepository = new();
        private readonly Mock<IDatabase> _redisDatabaseMock = new();
        #endregion

        #region Init mock data
        private static Guid customVehicleType_Type1 = Guid.Parse("e52b708a-b25b-431e-b12b-cd53a7f1e8cc");
        private static Guid customVehicleType_Type2 = Guid.Parse("338a065e-2ee4-47cd-9af6-04b121541d47");
        private List<AppConfigModel> appConfigs = new List<AppConfigModel>()
        {
            new AppConfigModel()
            {
                Id = Guid.NewGuid(),
                AppName = "Test",
                CreatedDate = DateTime.Now,
                FooterLine1 = "Test",
                FooterLine2 = "Test",
                HeaderHeading = "Test",
                HeaderLine1 = "Test",
                HeaderLine2 = "Test",
                HeaderSubHeading = "Test",
                IsApply = true,
                StationCode = "Test",
            }
        };
        private List<CustomVehicleTypeModel> customVehicleTypes = new List<CustomVehicleTypeModel>()
        {
            new CustomVehicleTypeModel()
            {
                Id = customVehicleType_Type1,
                Name = CustomVehicleTypeEnum.Type1,
                CreatedDate = DateTime.Now
            },
            new CustomVehicleTypeModel()
            {
                Id = Guid.NewGuid(),
                Name = CustomVehicleTypeEnum.Type2,
                CreatedDate = DateTime.Now
            },
        };
        private List<PaymentStatusModel> paymentStatuses = new List<PaymentStatusModel>()
        {
            new PaymentStatusModel()
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                Amount = 1000,
                Currency = "VND",
                Payment = new PaymentModel()
                {
                    Amount = 1000,
                    CreatedDate = DateTime.Now,
                    CustomVehicleTypeId = customVehicleType_Type1,
                    Id = Guid.NewGuid(),
                },
                PaymentMethod = PaymentMethodEnum.Cash
            },
            new PaymentStatusModel()
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                Amount = 1000,
                Currency = "VND",
                Payment = new PaymentModel()
                {
                    Amount = 1000,
                    CreatedDate = DateTime.Now,
                    CustomVehicleTypeId = customVehicleType_Type2,
                    Id = Guid.NewGuid(),
                },
                PaymentMethod = PaymentMethodEnum.RFID,
                PaymentDate = new DateTime(2023, 9, 29, 15, 35, 19)
            }
        };
        private SessionReportRequestModel sessionReportRequest = new SessionReportRequestModel()
        {
            FromDate = new DateTime(2023, 9, 29, 15, 32, 19),
            ToDate = new DateTime(2023, 9, 29, 15, 43, 53)
        };
        private BarrierRequestModel barrierRequest = new BarrierRequestModel()
        {
            Action = BarrierActionEnum.Open,
            EmployeeId = "Some",
            LaneId = "Some",
            Limit = 1
        };
        #endregion

        #region PrintLaneSessionReport
        [Fact]
        public async Task GivenValidRequest_WhenPrintLaneSessionReportIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _appConfigRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<AppConfigModel, bool>>>())).ReturnsAsync(appConfigs);
            _customVehicleTypeRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<CustomVehicleTypeModel, bool>>>())).ReturnsAsync(customVehicleTypes);
            _paymentStatusRepositoryMock.Setup(x => x.GetAllWithNavigationAsync(It.IsAny<SessionReportRequestModel>())).ReturnsAsync(paymentStatuses);

            // Act
            var service = new UIActionService(_loggerMock.Object, _paymentStatusRepositoryMock.Object, _appConfigRepositoryMock.Object, _customVehicleTypeRepositoryMock.Object, _manualBarrierControlRepository.Object, _redisDatabaseMock.Object);
            var result = await service.PrintLaneSessionReport(sessionReportRequest);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();

            _appConfigRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<AppConfigModel, bool>>>()), Times.Once);
            _customVehicleTypeRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<CustomVehicleTypeModel, bool>>>()), Times.Once);
            _paymentStatusRepositoryMock.Verify(x => x.GetAllWithNavigationAsync(It.IsAny<SessionReportRequestModel>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.PrintLaneSessionReport)} method...", Times.Once, _nullException);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.PrintLaneSessionReport)} method", Times.Never, _nullException);
        }

        [Fact]
        public async Task GivenValidRequestAndAppConfigRepositoryIsDown_WhenPrintLaneSessionReportIsCalled_ThenThrowException()
        {
            // Arrange
            var exception = new Exception("Some ex");
            _appConfigRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<AppConfigModel, bool>>>())).ThrowsAsync(exception);

            // Act
            var service = new UIActionService(_loggerMock.Object, _paymentStatusRepositoryMock.Object, _appConfigRepositoryMock.Object, _customVehicleTypeRepositoryMock.Object, _manualBarrierControlRepository.Object, _redisDatabaseMock.Object);
            Func<Task> func = () => service.PrintLaneSessionReport(sessionReportRequest);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            ex.Should().NotBeNull();
            ex.Message.Should().Be(exception.Message);

            _appConfigRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<AppConfigModel, bool>>>()), Times.Once);
            _customVehicleTypeRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<CustomVehicleTypeModel, bool>>>()), Times.Never);
            _paymentStatusRepositoryMock.Verify(x => x.GetAllWithNavigationAsync(It.IsAny<SessionReportRequestModel>()), Times.Never);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.PrintLaneSessionReport)} method...", Times.Once, _nullException);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.PrintLaneSessionReport)} method", Times.Once, _nullException);
        }

        [Fact]
        public async Task GivenValidRequestAndCustomVehicleTypeRepositoryIsDown_WhenPrintLaneSessionReportIsCalled_ThenThrowException()
        {
            // Arrange
            var exception = new Exception("Some ex");
            _appConfigRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<AppConfigModel, bool>>>())).ReturnsAsync(appConfigs);
            _customVehicleTypeRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<CustomVehicleTypeModel, bool>>>())).ThrowsAsync(exception);

            // Act
            var service = new UIActionService(_loggerMock.Object, _paymentStatusRepositoryMock.Object, _appConfigRepositoryMock.Object, _customVehicleTypeRepositoryMock.Object, _manualBarrierControlRepository.Object, _redisDatabaseMock.Object);
            Func<Task> func = () => service.PrintLaneSessionReport(sessionReportRequest);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            ex.Should().NotBeNull();
            ex.Message.Should().Be(exception.Message);

            _appConfigRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<AppConfigModel, bool>>>()), Times.Once);
            _customVehicleTypeRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<CustomVehicleTypeModel, bool>>>()), Times.Once);
            _paymentStatusRepositoryMock.Verify(x => x.GetAllWithNavigationAsync(It.IsAny<SessionReportRequestModel>()), Times.Never);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.PrintLaneSessionReport)} method...", Times.Once, _nullException);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.PrintLaneSessionReport)} method", Times.Once, _nullException);
        }

        [Fact]
        public async Task GivenValidRequestAndPaymentStatusRepositoryIsDown_WhenPrintLaneSessionReportIsCalled_ThenThrowException()
        {
            // Arrange
            var exception = new Exception("Some ex");
            _appConfigRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<AppConfigModel, bool>>>())).ReturnsAsync(appConfigs);
            _customVehicleTypeRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<CustomVehicleTypeModel, bool>>>())).ReturnsAsync(customVehicleTypes);
            _paymentStatusRepositoryMock.Setup(x => x.GetAllWithNavigationAsync(It.IsAny<SessionReportRequestModel>())).ThrowsAsync(exception);

            // Act
            var service = new UIActionService(_loggerMock.Object, _paymentStatusRepositoryMock.Object, _appConfigRepositoryMock.Object, _customVehicleTypeRepositoryMock.Object, _manualBarrierControlRepository.Object, _redisDatabaseMock.Object);
            Func<Task> func = () => service.PrintLaneSessionReport(sessionReportRequest);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            ex.Should().NotBeNull();
            ex.Message.Should().Be(exception.Message);

            _appConfigRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<AppConfigModel, bool>>>()), Times.Once);
            _customVehicleTypeRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<CustomVehicleTypeModel, bool>>>()), Times.Once);
            _paymentStatusRepositoryMock.Verify(x => x.GetAllWithNavigationAsync(It.IsAny<SessionReportRequestModel>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.PrintLaneSessionReport)} method...", Times.Once, _nullException);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.PrintLaneSessionReport)} method", Times.Once, _nullException);
        }
        #endregion

        #region ManipulateBarrier
        [Fact]
        public async Task GivenValidRequest_WhenManipulateBarrierIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _redisDatabaseMock.Setup(x => x.HashSetAsync(It.IsNotNull<RedisKey>(), It.IsNotNull<HashEntry[]>(), It.IsAny<CommandFlags>()));
            _manualBarrierControlRepository.Setup(x => x.AddAsync(It.IsAny<ManualBarrierControlModel>()));

            // Act
            var service = new UIActionService(_loggerMock.Object, _paymentStatusRepositoryMock.Object, _appConfigRepositoryMock.Object, _customVehicleTypeRepositoryMock.Object, _manualBarrierControlRepository.Object, _redisDatabaseMock.Object);
            var result = await service.ManipulateBarrier(barrierRequest);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();

            _redisDatabaseMock.Verify(x => x.HashSetAsync(It.IsAny<RedisKey>(), It.IsAny<HashEntry[]>(), It.IsAny<CommandFlags>()), Times.Once);
            _manualBarrierControlRepository.Verify(x => x.AddAsync(It.IsAny<ManualBarrierControlModel>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.ManipulateBarrier)} method...", Times.Once, _nullException);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.ManipulateBarrier)} method", Times.Never, _nullException);
        }

        [Fact]
        public async Task GivenValidRequestAndRedisDatabaseIsDown_WhenManipulateBarrierIsCalled_ThenThrowException()
        {
            // Arrange
            var exception = new Exception("Some ex");
            _redisDatabaseMock.Setup(x => x.HashSetAsync(It.IsAny<RedisKey>(), It.IsAny<HashEntry[]>(), It.IsAny<CommandFlags>())).ThrowsAsync(exception);

            // Act
            var service = new UIActionService(_loggerMock.Object, _paymentStatusRepositoryMock.Object, _appConfigRepositoryMock.Object, _customVehicleTypeRepositoryMock.Object, _manualBarrierControlRepository.Object, _redisDatabaseMock.Object);
            Func<Task> func = () => service.ManipulateBarrier(barrierRequest);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            ex.Should().NotBeNull();
            ex.Message.Should().Be(exception.Message);

            _redisDatabaseMock.Verify(x => x.HashSetAsync(It.IsAny<RedisKey>(), It.IsAny<HashEntry[]>(), It.IsAny<CommandFlags>()), Times.Once);
            _manualBarrierControlRepository.Verify(x => x.AddAsync(It.IsAny<ManualBarrierControlModel>()), Times.Never);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.ManipulateBarrier)} method...", Times.Once, _nullException);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.ManipulateBarrier)} method", Times.Once, _nullException);
        }

        [Fact]
        public async Task GivenValidRequestAndManualBarrierControlRepositoryIsDown_WhenManipulateBarrierIsCalled_ThenThrowException()
        {
            // Arrange
            var exception = new Exception("Some ex");
            _redisDatabaseMock.Setup(x => x.HashSetAsync(It.IsAny<RedisKey>(), It.IsAny<HashEntry[]>(), It.IsAny<CommandFlags>()));
            _manualBarrierControlRepository.Setup(x => x.AddAsync(It.IsAny<ManualBarrierControlModel>())).ThrowsAsync(exception);

            // Act
            var service = new UIActionService(_loggerMock.Object, _paymentStatusRepositoryMock.Object, _appConfigRepositoryMock.Object, _customVehicleTypeRepositoryMock.Object, _manualBarrierControlRepository.Object, _redisDatabaseMock.Object);
            Func<Task> func = () => service.ManipulateBarrier(barrierRequest);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            ex.Should().NotBeNull();
            ex.Message.Should().Be(exception.Message);

            _redisDatabaseMock.Verify(x => x.HashSetAsync(It.IsAny<RedisKey>(), It.IsAny<HashEntry[]>(), It.IsAny<CommandFlags>()), Times.Once);
            _manualBarrierControlRepository.Verify(x => x.AddAsync(It.IsAny<ManualBarrierControlModel>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.ManipulateBarrier)} method...", Times.Once, _nullException);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.ManipulateBarrier)} method", Times.Once, _nullException);
        }
        #endregion
    }
}