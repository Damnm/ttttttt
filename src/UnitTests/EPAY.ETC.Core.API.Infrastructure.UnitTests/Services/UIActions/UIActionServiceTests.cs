using EPAY.ETC.Core.API.Core.Interfaces.Services.ExternalServices;
using EPAY.ETC.Core.API.Core.Models.Configs;
using EPAY.ETC.Core.API.Core.Models.CustomVehicleTypes;
using EPAY.ETC.Core.API.Core.Models.ManualBarrierControl;
using EPAY.ETC.Core.API.Core.Models.Vehicle.ReconcileVehicle;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.CustomVehicleTypes;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ETCCheckouts;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ManualBarrierControls;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Payment;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.PaymentStatus;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.PrintLog;
using EPAY.ETC.Core.API.Infrastructure.Services.UIActions;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Common;
using EPAY.ETC.Core.API.Infrastructure.UnitTests.Helpers;
using EPAY.ETC.Core.Models.Constants;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.Receipt.SessionReports;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.UI;
using EPAY.ETC.Core.Models.Validation;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using StackExchange.Redis;
using System.Linq.Expressions;
using System.Text.Json;
using PaymentModel = EPAY.ETC.Core.API.Core.Models.Payment.PaymentModel;
using PaymentStatusModel = EPAY.ETC.Core.API.Core.Models.PaymentStatus.PaymentStatusModel;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Services.UIActions
{
    public class UIActionServiceTests : TestBase<UIActionService>
    {
        #region Init mock instance
        private readonly Mock<IPaymentStatusRepository> _paymentStatusRepositoryMock = new();
        private readonly Mock<IAppConfigRepository> _appConfigRepositoryMock = new();
        private readonly Mock<ICustomVehicleTypeRepository> _customVehicleTypeRepositoryMock = new();
        private readonly Mock<IManualBarrierControlRepository> _manualBarrierControlRepository = new();
        private readonly Mock<IPaymentRepository> _paymentRepository = new();
        private readonly Mock<IDatabase> _redisDatabaseMock = new();
        private readonly Mock<IServer> _redisServerMock = new();
        private readonly Mock<IPrintLogRepository> _appPrintLogRepository = new Mock<IPrintLogRepository>();
        private readonly Mock<IPOSService> _pOSServiceMock = new Mock<IPOSService>();
        #endregion

        #region Init mock data
        private IOptions<UIModel> _uiOptions = Options.Create(new UIModel());

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
        private LaneSessionReportRequestModel sessionReportRequest = new LaneSessionReportRequestModel()
        {
            FromDateTimeEpoch = 1696909646,
            ToDateTimeEpoch = 1696909646
        };
        private BarrierRequestModel barrierRequest = new BarrierRequestModel()
        {
            Action = BarrierActionEnum.Open,
            EmployeeId = "Some",
            LaneId = "Some",
            Limit = 1
        };
        Guid guid = Guid.NewGuid();
        #endregion

        #region PrintLaneSessionReport
        [Fact]
        public async Task GivenValidRequest_WhenPrintLaneSessionReportIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            _appConfigRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<AppConfigModel, bool>>>())).ReturnsAsync(appConfigs);
            _customVehicleTypeRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<CustomVehicleTypeModel, bool>>>())).ReturnsAsync(customVehicleTypes);
            _paymentStatusRepositoryMock.Setup(x => x.GetAllWithNavigationAsync(It.IsAny<LaneSessionReportRequestModel>())).ReturnsAsync(paymentStatuses);

            // Act
            var service = new UIActionService(_loggerMock.Object, _paymentRepository.Object, _paymentStatusRepositoryMock.Object, _appConfigRepositoryMock.Object, _customVehicleTypeRepositoryMock.Object, _manualBarrierControlRepository.Object, _redisDatabaseMock.Object, _redisServerMock.Object, _uiOptions, _appPrintLogRepository.Object, _pOSServiceMock.Object);
            var result = await service.PrintLaneSessionReportAsync(sessionReportRequest);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();

            _appConfigRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<AppConfigModel, bool>>>()), Times.Once);
            _customVehicleTypeRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<CustomVehicleTypeModel, bool>>>()), Times.Once);
            _paymentStatusRepositoryMock.Verify(x => x.GetAllWithNavigationAsync(It.IsAny<LaneSessionReportRequestModel>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.PrintLaneSessionReportAsync)} method...", Times.Once, _nullException);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.PrintLaneSessionReportAsync)} method", Times.Never, _nullException);
        }

        [Fact]
        public async Task GivenValidRequestAndAppConfigRepositoryIsDown_WhenPrintLaneSessionReportIsCalled_ThenThrowException()
        {
            // Arrange
            var exception = new Exception("Some ex");
            _appConfigRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<AppConfigModel, bool>>>())).ThrowsAsync(exception);

            // Act
            var service = new UIActionService(_loggerMock.Object, _paymentRepository.Object, _paymentStatusRepositoryMock.Object
                , _appConfigRepositoryMock.Object, _customVehicleTypeRepositoryMock.Object, _manualBarrierControlRepository.Object, _redisDatabaseMock.Object, _redisServerMock.Object, _uiOptions, _appPrintLogRepository.Object, _pOSServiceMock.Object);
            Func<Task> func = () => service.PrintLaneSessionReportAsync(sessionReportRequest);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            ex.Should().NotBeNull();
            ex.Message.Should().Be(exception.Message);

            _appConfigRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<AppConfigModel, bool>>>()), Times.Once);
            _customVehicleTypeRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<CustomVehicleTypeModel, bool>>>()), Times.Never);
            _paymentStatusRepositoryMock.Verify(x => x.GetAllWithNavigationAsync(It.IsAny<LaneSessionReportRequestModel>()), Times.Never);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.PrintLaneSessionReportAsync)} method...", Times.Once, _nullException);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.PrintLaneSessionReportAsync)} method", Times.Once, _nullException);
        }

        [Fact]
        public async Task GivenValidRequestAndCustomVehicleTypeRepositoryIsDown_WhenPrintLaneSessionReportIsCalled_ThenThrowException()
        {
            // Arrange
            var exception = new Exception("Some ex");
            _appConfigRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<AppConfigModel, bool>>>())).ReturnsAsync(appConfigs);
            _customVehicleTypeRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<CustomVehicleTypeModel, bool>>>())).ThrowsAsync(exception);

            // Act
            var service = new UIActionService(_loggerMock.Object, _paymentRepository.Object, _paymentStatusRepositoryMock.Object, _appConfigRepositoryMock.Object, _customVehicleTypeRepositoryMock.Object, _manualBarrierControlRepository.Object, _redisDatabaseMock.Object, _redisServerMock.Object, _uiOptions, _appPrintLogRepository.Object, _pOSServiceMock.Object);
            Func<Task> func = () => service.PrintLaneSessionReportAsync(sessionReportRequest);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            ex.Should().NotBeNull();
            ex.Message.Should().Be(exception.Message);

            _appConfigRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<AppConfigModel, bool>>>()), Times.Once);
            _customVehicleTypeRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<CustomVehicleTypeModel, bool>>>()), Times.Once);
            _paymentStatusRepositoryMock.Verify(x => x.GetAllWithNavigationAsync(It.IsAny<LaneSessionReportRequestModel>()), Times.Never);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.PrintLaneSessionReportAsync)} method...", Times.Once, _nullException);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.PrintLaneSessionReportAsync)} method", Times.Once, _nullException);
        }

        [Fact]
        public async Task GivenValidRequestAndPaymentStatusRepositoryIsDown_WhenPrintLaneSessionReportIsCalled_ThenThrowException()
        {
            // Arrange
            var exception = new Exception("Some ex");
            _appConfigRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<AppConfigModel, bool>>>())).ReturnsAsync(appConfigs);
            _customVehicleTypeRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<CustomVehicleTypeModel, bool>>>())).ReturnsAsync(customVehicleTypes);
            _paymentStatusRepositoryMock.Setup(x => x.GetAllWithNavigationAsync(It.IsAny<LaneSessionReportRequestModel>())).ThrowsAsync(exception);

            // Act
            var service = new UIActionService(_loggerMock.Object, _paymentRepository.Object, _paymentStatusRepositoryMock.Object, _appConfigRepositoryMock.Object, _customVehicleTypeRepositoryMock.Object, _manualBarrierControlRepository.Object, _redisDatabaseMock.Object, _redisServerMock.Object, _uiOptions, _appPrintLogRepository.Object, _pOSServiceMock.Object);
            Func<Task> func = () => service.PrintLaneSessionReportAsync(sessionReportRequest);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            ex.Should().NotBeNull();
            ex.Message.Should().Be(exception.Message);

            _appConfigRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<AppConfigModel, bool>>>()), Times.Once);
            _customVehicleTypeRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<CustomVehicleTypeModel, bool>>>()), Times.Once);
            _paymentStatusRepositoryMock.Verify(x => x.GetAllWithNavigationAsync(It.IsAny<LaneSessionReportRequestModel>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.PrintLaneSessionReportAsync)} method...", Times.Once, _nullException);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.PrintLaneSessionReportAsync)} method", Times.Once, _nullException);
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
            var service = new UIActionService(_loggerMock.Object, _paymentRepository.Object, _paymentStatusRepositoryMock.Object, _appConfigRepositoryMock.Object, _customVehicleTypeRepositoryMock.Object, _manualBarrierControlRepository.Object, _redisDatabaseMock.Object, _redisServerMock.Object, _uiOptions, _appPrintLogRepository.Object, _pOSServiceMock.Object);
            var result = await service.ManipulateBarrierAsync(barrierRequest);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();

            _redisDatabaseMock.Verify(x => x.HashSetAsync(It.IsAny<RedisKey>(), It.IsAny<HashEntry[]>(), It.IsAny<CommandFlags>()), Times.Once);
            _manualBarrierControlRepository.Verify(x => x.AddAsync(It.IsAny<ManualBarrierControlModel>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.ManipulateBarrierAsync)} method...", Times.Once, _nullException);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.ManipulateBarrierAsync)} method", Times.Never, _nullException);
        }

        [Fact]
        public async Task GivenValidRequestAndRedisDatabaseIsDown_WhenManipulateBarrierIsCalled_ThenThrowException()
        {
            // Arrange
            var exception = new Exception("Some ex");
            _redisDatabaseMock.Setup(x => x.HashSetAsync(It.IsAny<RedisKey>(), It.IsAny<HashEntry[]>(), It.IsAny<CommandFlags>())).ThrowsAsync(exception);

            // Act
            var service = new UIActionService(_loggerMock.Object, _paymentRepository.Object, _paymentStatusRepositoryMock.Object, _appConfigRepositoryMock.Object, _customVehicleTypeRepositoryMock.Object, _manualBarrierControlRepository.Object, _redisDatabaseMock.Object, _redisServerMock.Object, _uiOptions, _appPrintLogRepository.Object, _pOSServiceMock.Object);
            Func<Task> func = () => service.ManipulateBarrierAsync(barrierRequest);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            ex.Should().NotBeNull();
            ex.Message.Should().Be(exception.Message);

            _redisDatabaseMock.Verify(x => x.HashSetAsync(It.IsAny<RedisKey>(), It.IsAny<HashEntry[]>(), It.IsAny<CommandFlags>()), Times.Once);
            _manualBarrierControlRepository.Verify(x => x.AddAsync(It.IsAny<ManualBarrierControlModel>()), Times.Never);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.ManipulateBarrierAsync)} method...", Times.Once, _nullException);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.ManipulateBarrierAsync)} method", Times.Once, _nullException);
        }

        [Fact]
        public async Task GivenValidRequestAndManualBarrierControlRepositoryIsDown_WhenManipulateBarrierIsCalled_ThenThrowException()
        {
            // Arrange
            var exception = new Exception("Some ex");
            _redisDatabaseMock.Setup(x => x.HashSetAsync(It.IsAny<RedisKey>(), It.IsAny<HashEntry[]>(), It.IsAny<CommandFlags>()));
            _manualBarrierControlRepository.Setup(x => x.AddAsync(It.IsAny<ManualBarrierControlModel>())).ThrowsAsync(exception);

            // Act
            var service = new UIActionService(_loggerMock.Object, _paymentRepository.Object, _paymentStatusRepositoryMock.Object, _appConfigRepositoryMock.Object, _customVehicleTypeRepositoryMock.Object, _manualBarrierControlRepository.Object, _redisDatabaseMock.Object, _redisServerMock.Object, _uiOptions, _appPrintLogRepository.Object, _pOSServiceMock.Object);
            Func<Task> func = () => service.ManipulateBarrierAsync(barrierRequest);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            ex.Should().NotBeNull();
            ex.Message.Should().Be(exception.Message);

            _redisDatabaseMock.Verify(x => x.HashSetAsync(It.IsAny<RedisKey>(), It.IsAny<HashEntry[]>(), It.IsAny<CommandFlags>()), Times.Once);
            _manualBarrierControlRepository.Verify(x => x.AddAsync(It.IsAny<ManualBarrierControlModel>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.ManipulateBarrierAsync)} method...", Times.Once, _nullException);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.ManipulateBarrierAsync)} method", Times.Once, _nullException);
        }
        #endregion

        #region ReconcileVehicleInfoAsync
        [Fact]
        public void GivenValidRequest_WhenReconcileVehicleInfoAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange

            ReconcileVehicleInfoModel reconcileVehicleInfo = new ReconcileVehicleInfoModel()
            {
                EmployeeId = "34343243",
                ObjectId = guid,
                Vehicle = new ReconcileVehicleModel()
                {
                    PlateNumber = "fffdsfdfd",
                    VehicleType = "01",

                    In = new Core.Models.Vehicle.ReconcileVehicle.LaneInModel()
                    {
                        LaneInId = "01"
                    },
                    Out = new Core.Models.Vehicle.ReconcileVehicle.LaneOutModel()
                    {
                        LaneOutId = "06"
                    }
                }
            };

            string feeModel = "{\"FeeId\":null,\"ObjectId\":\"8034d7bb-e430-4305-b8b7-8a7f640d152b\",\"CreatedDate\":\"0001-01-01T00:00:00\",\"EmployeeId\":null,\"ShiftId\":null,\"LaneInVehicle\":{\"LaneInId\":\"1\",\"Epoch\":1695185974,\"RFID\":\"dddddddddddddd\",\"Device\":{\"MacAddr\":\"macaddress\",\"IpAddr\":\"127.0.0.1\"},\"VehicleInfo\":{\"Make\":\"Toyota\",\"Model\":\"S\",\"PlateNumber\":\"52H23232\",\"PlateColour\":\"red\",\"VehicleColour\":null,\"VehicleType\":\"1\",\"Seat\":10,\"Weight\":40,\"VehiclePhotoUrl\":null,\"PlateNumberPhotoUrl\":null,\"ConfidenceScore\":0.0}},\"LaneOutVehicle\":{\"LaneOutId\":\"1\",\"Epoch\":1695285974,\"RFID\":\"dddddddddddddd\",\"Device\":{\"MacAddr\":\"macaddressout\",\"IpAddr\":\"127.0.0.1\"},\"VehicleInfo\":{\"Make\":\"Toyota\",\"Model\":\"S\",\"PlateNumber\":\"52H23232\",\"PlateColour\":\"red\",\"VehicleColour\":null,\"VehicleType\":\"1\",\"Seat\":12,\"Weight\":50,\"VehiclePhotoUrl\":null,\"PlateNumberPhotoUrl\":null,\"ConfidenceScore\":0.0}},\"Payment\":null,\"FeeType\":0,\"ValidateObjectId\":1,\"ValidateCreatedDate\":0}";
            _redisDatabaseMock.Setup(x => x.StringGetAsync(It.IsNotNull<RedisKey>(), It.IsAny<CommandFlags>())).ReturnsAsync(new RedisValue(feeModel));

            // Act
            var service = new UIActionService(_loggerMock.Object, _paymentRepository.Object, _paymentStatusRepositoryMock.Object, _appConfigRepositoryMock.Object, _customVehicleTypeRepositoryMock.Object, _manualBarrierControlRepository.Object, _redisDatabaseMock.Object, _redisServerMock.Object, _uiOptions, _appPrintLogRepository.Object, _pOSServiceMock.Object);
            var result = service.ReconcileVehicleInfo(reconcileVehicleInfo);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();

            _redisDatabaseMock.Verify(x => x.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.ReconcileVehicleInfo)} method...", Times.Once, _nullException);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.ReconcileVehicleInfo)} method", Times.Never, _nullException);
        }

        [Fact]
        public void GivenValidRequestAndRedisDatabaseIsDown_WhenReconcileVehicleInfoAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var exception = new Exception("Some ex");
            _redisDatabaseMock.Setup(x => x.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).ThrowsAsync(exception);

            // Act
            var service = new UIActionService(_loggerMock.Object, _paymentRepository.Object, _paymentStatusRepositoryMock.Object, _appConfigRepositoryMock.Object, _customVehicleTypeRepositoryMock.Object, _manualBarrierControlRepository.Object, _redisDatabaseMock.Object, _redisServerMock.Object, _uiOptions, _appPrintLogRepository.Object, _pOSServiceMock.Object);
            Func<ValidationResult<ReconcileResultModel>> func = () => service.ReconcileVehicleInfo(null!);

            // Assert
            var ex = Assert.Throws<Exception>(func);
            ex.Should().NotBeNull();
            ex.Message.Should().Be(exception.Message);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.ReconcileVehicleInfo)} method...", Times.Once, _nullException);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.ReconcileVehicleInfo)} method", Times.Once, _nullException);
        }

        [Fact]
        public void GivenValidRequestAndInputIsNull_WhenReconcileVehicleInfoAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            ReconcileVehicleInfoModel reconcileVehicleInfo = new ReconcileVehicleInfoModel()
            {
                EmployeeId = "34343243",
                ObjectId = guid,
                Vehicle = new ReconcileVehicleModel()
                {
                    PlateNumber = "fffdsfdfd",
                    VehicleType = "01",

                    In = new Core.Models.Vehicle.ReconcileVehicle.LaneInModel()
                    {
                        LaneInId = "01"
                    },
                    Out = new Core.Models.Vehicle.ReconcileVehicle.LaneOutModel()
                    {
                        LaneOutId = "06"
                    }
                }
            };

            string feeModel = "{\"FeeId\":null,\"ObjectId\":\"8034d7bb-e430-4305-b8b7-8a7f640d152b\",\"CreatedDate\":\"0001-01-01T00:00:00\",\"EmployeeId\":null,\"ShiftId\":null,\"LaneInVehicle\":{\"LaneInId\":\"1\",\"Epoch\":1695185974,\"RFID\":\"dddddddddddddd\",\"Device\":{\"MacAddr\":\"macaddress\",\"IpAddr\":\"127.0.0.1\"},\"VehicleInfo\":{\"Make\":\"Toyota\",\"Model\":\"S\",\"PlateNumber\":\"52H23232\",\"PlateColour\":\"red\",\"VehicleColour\":null,\"VehicleType\":\"1\",\"Seat\":10,\"Weight\":40,\"VehiclePhotoUrl\":null,\"PlateNumberPhotoUrl\":null,\"ConfidenceScore\":0.0}},\"LaneOutVehicle\":{\"LaneOutId\":\"1\",\"Epoch\":1695285974,\"RFID\":\"dddddddddddddd\",\"Device\":{\"MacAddr\":\"macaddressout\",\"IpAddr\":\"127.0.0.1\"},\"VehicleInfo\":{\"Make\":\"Toyota\",\"Model\":\"S\",\"PlateNumber\":\"52H23232\",\"PlateColour\":\"red\",\"VehicleColour\":null,\"VehicleType\":\"1\",\"Seat\":12,\"Weight\":50,\"VehiclePhotoUrl\":null,\"PlateNumberPhotoUrl\":null,\"ConfidenceScore\":0.0}},\"Payment\":null,\"FeeType\":0,\"ValidateObjectId\":1,\"ValidateCreatedDate\":0}";
            _redisDatabaseMock.Setup(x => x.StringGetAsync(It.IsNotNull<RedisKey>(), It.IsAny<CommandFlags>())).ReturnsAsync(new RedisValue(feeModel));

            // Act
            var service = new UIActionService(_loggerMock.Object, _paymentRepository.Object, _paymentStatusRepositoryMock.Object, _appConfigRepositoryMock.Object, _customVehicleTypeRepositoryMock.Object, _manualBarrierControlRepository.Object, _redisDatabaseMock.Object, _redisServerMock.Object, _uiOptions, _appPrintLogRepository.Object, _pOSServiceMock.Object);
            var result = service.ReconcileVehicleInfo(null!);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();

            _redisDatabaseMock.Verify(x => x.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.ReconcileVehicleInfo)} method...", Times.Once, _nullException);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.ReconcileVehicleInfo)} method", Times.Never, _nullException);
        }

        [Fact]
        public void GivenValidRequestWithFeeObjectIsNull_WhenReconcileVehicleInfoAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            ReconcileVehicleInfoModel reconcileVehicleInfo = new ReconcileVehicleInfoModel()
            {
                EmployeeId = "34343243",
                ObjectId = guid,
                Vehicle = new ReconcileVehicleModel()
                {
                    PlateNumber = "fffdsfdfd",
                    VehicleType = "01",

                    In = new Core.Models.Vehicle.ReconcileVehicle.LaneInModel()
                    {
                        LaneInId = "01"
                    },
                    Out = new Core.Models.Vehicle.ReconcileVehicle.LaneOutModel()
                    {
                        LaneOutId = "06"
                    }
                }
            };

            string feeModel = null!;
            _redisDatabaseMock.Setup(x => x.StringGetAsync(It.IsNotNull<RedisKey>(), It.IsAny<CommandFlags>())).ReturnsAsync(new RedisValue(feeModel));

            // Act
            var service = new UIActionService(_loggerMock.Object, _paymentRepository.Object, _paymentStatusRepositoryMock.Object, _appConfigRepositoryMock.Object, _customVehicleTypeRepositoryMock.Object, _manualBarrierControlRepository.Object, _redisDatabaseMock.Object, _redisServerMock.Object, _uiOptions, _appPrintLogRepository.Object, _pOSServiceMock.Object);
            var result = service.ReconcileVehicleInfo(reconcileVehicleInfo);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data?.Fee.Should().BeNull();
            result.Data?.PaymentStatus.Should().BeNull();
            result.Succeeded.Should().BeTrue();

            _redisDatabaseMock.Verify(x => x.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.ReconcileVehicleInfo)} method...", Times.Once, _nullException);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.ReconcileVehicleInfo)} method", Times.Never, _nullException);
        }
        #endregion

        #region LoadCurrentUIAsync
        [Fact]
        public async Task GivenValidRequest_WhenLoadCurrentUIAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            string redisValue = JsonSerializer.Serialize(new UIModel());
            _redisDatabaseMock.Setup(x => x.StringGetAsync(RedisConstant.UI_MODEL_KEY, It.IsAny<CommandFlags>())).ReturnsAsync(redisValue);
            _redisDatabaseMock.Setup(x => x.StringSetAsync(RedisConstant.UI_MODEL_KEY, redisValue, It.IsAny<TimeSpan>(), It.IsAny<When>(), It.IsAny<CommandFlags>())).ReturnsAsync(true);

            // Act
            var service = new UIActionService(_loggerMock.Object, _paymentRepository.Object, _paymentStatusRepositoryMock.Object, _appConfigRepositoryMock.Object, _customVehicleTypeRepositoryMock.Object, _manualBarrierControlRepository.Object, _redisDatabaseMock.Object, _redisServerMock.Object, _uiOptions, _appPrintLogRepository.Object, _pOSServiceMock.Object);
            var result = await service.LoadCurrentUIAsync();

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();

            _redisDatabaseMock.Verify(x => x.StringGetAsync(RedisConstant.UI_MODEL_KEY, It.IsAny<CommandFlags>()), Times.Once);
            _redisDatabaseMock.Verify(x => x.StringGetAsync(RedisConstant.UI_MODEL_TEMPLATE_KEY, It.IsAny<CommandFlags>()), Times.Never);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.LoadCurrentUIAsync)} method...", Times.Once, _nullException);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.LoadCurrentUIAsync)} method", Times.Never, _nullException);
        }
        [Fact]
        public async Task GivenValidRequestAndValueOfUIModelKeyIsNotExists_WhenLoadCurrentUIAsyncIsCalled_ThenReturnCorrectResult()
        {
            // Arrange
            string redisValue = JsonSerializer.Serialize(new UIModel());
            _redisDatabaseMock.Setup(x => x.StringGetAsync(RedisConstant.UI_MODEL_KEY, It.IsAny<CommandFlags>())).ReturnsAsync(string.Empty);
            _redisDatabaseMock.Setup(x => x.StringGetAsync(RedisConstant.UI_MODEL_TEMPLATE_KEY, It.IsAny<CommandFlags>())).ReturnsAsync(redisValue);
            _redisDatabaseMock.Setup(x => x.StringSetAsync(RedisConstant.UI_MODEL_KEY, redisValue, It.IsAny<TimeSpan>(), It.IsAny<When>(), It.IsAny<CommandFlags>())).ReturnsAsync(true);

            // Act
            var service = new UIActionService(_loggerMock.Object, _paymentRepository.Object, _paymentStatusRepositoryMock.Object, _appConfigRepositoryMock.Object, _customVehicleTypeRepositoryMock.Object, _manualBarrierControlRepository.Object, _redisDatabaseMock.Object, _redisServerMock.Object, _uiOptions, _appPrintLogRepository.Object, _pOSServiceMock.Object);
            var result = await service.LoadCurrentUIAsync();

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();

            _redisDatabaseMock.Verify(x => x.StringGetAsync(RedisConstant.UI_MODEL_KEY, It.IsAny<CommandFlags>()), Times.Once);
            _redisDatabaseMock.Verify(x => x.StringGetAsync(RedisConstant.UI_MODEL_TEMPLATE_KEY, It.IsAny<CommandFlags>()), Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.LoadCurrentUIAsync)} method...", Times.Once, _nullException);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.LoadCurrentUIAsync)} method", Times.Never, _nullException);
        }

        [Fact]
        public async Task GivenValidRequestAndRedisDatabaseIsDown_WhenLoadCurrentUIAsyncIsCalled_ThenThrowException()
        {
            // Arrange
            var exception = new Exception("Some ex");
            _redisDatabaseMock.Setup(x => x.StringGetAsync(RedisConstant.UI_MODEL_KEY, It.IsAny<CommandFlags>())).ThrowsAsync(exception);

            // Act
            var service = new UIActionService(_loggerMock.Object, _paymentRepository.Object, _paymentStatusRepositoryMock.Object, _appConfigRepositoryMock.Object, _customVehicleTypeRepositoryMock.Object, _manualBarrierControlRepository.Object, _redisDatabaseMock.Object, _redisServerMock.Object, _uiOptions, _appPrintLogRepository.Object, _pOSServiceMock.Object);
            Func<Task> func = () => service.LoadCurrentUIAsync();

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(func);
            ex.Should().NotBeNull();
            ex.Message.Should().Be(exception.Message);

            _redisDatabaseMock.Verify(x => x.StringGetAsync(RedisConstant.UI_MODEL_KEY, It.IsAny<CommandFlags>()), Times.Once);
            _redisDatabaseMock.Verify(x => x.StringGetAsync(RedisConstant.UI_MODEL_TEMPLATE_KEY, It.IsAny<CommandFlags>()), Times.Never);
            _redisDatabaseMock.Verify(x => x.StringSetAsync(RedisConstant.UI_MODEL_KEY, It.IsNotNull<RedisValue>(), It.IsAny<TimeSpan>(), It.IsAny<When>(), It.IsAny<CommandFlags>()), Times.Never);

            _loggerMock.VerifyLog(LogLevel.Information, $"Executing {nameof(service.LoadCurrentUIAsync)} method...", Times.Once, _nullException);
            _loggerMock.VerifyLog(LogLevel.Error, $"An error occurred when calling {nameof(service.LoadCurrentUIAsync)} method", Times.Once, _nullException);
        }

        #endregion
    }
}