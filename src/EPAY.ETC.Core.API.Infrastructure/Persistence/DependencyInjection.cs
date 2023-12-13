using EPAY.ETC.Core.API.Core.Interfaces.Services.Authentication;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Barcode;
using EPAY.ETC.Core.API.Core.Interfaces.Services.ErrorResponse;
using EPAY.ETC.Core.API.Core.Interfaces.Services.ETCCheckouts;
using EPAY.ETC.Core.API.Core.Interfaces.Services.ExternalServices;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Fees;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Fusion;
using EPAY.ETC.Core.API.Core.Interfaces.Services.InfringedVehicle;
using EPAY.ETC.Core.API.Core.Interfaces.Services.ManualBarrierControls;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Parking.ParkingBuilder;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Payment;
using EPAY.ETC.Core.API.Core.Interfaces.Services.PaymentStatus;
using EPAY.ETC.Core.API.Core.Interfaces.Services.PrintLog;
using EPAY.ETC.Core.API.Core.Interfaces.Services.TicketType;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Transaction;
using EPAY.ETC.Core.API.Core.Interfaces.Services.UIActions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Vehicles;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.AppConfigs;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Barcode;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.CustomVehicleTypes;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Employees;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ErrorResponse;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ETCCheckouts;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Fees;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.FeeTypes;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.FeeVehicleCategories;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Fusion;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.InfringedVehicle;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ManualBarrierControls;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Payment;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.PaymentStatus;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.PrintLog;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.TicketType;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.TicketTypes;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.TimeBlockFees;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.TransactionLog;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Vehicle;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.VehicleCategories;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.VehicleGroups;
using EPAY.ETC.Core.API.Infrastructure.Services.Authentication;
using EPAY.ETC.Core.API.Infrastructure.Services.Barcode;
using EPAY.ETC.Core.API.Infrastructure.Services.ErrorResponse;
using EPAY.ETC.Core.API.Infrastructure.Services.ETCCheckouts;
using EPAY.ETC.Core.API.Infrastructure.Services.ExternalServices;
using EPAY.ETC.Core.API.Infrastructure.Services.Fees;
using EPAY.ETC.Core.API.Infrastructure.Services.Fusion;
using EPAY.ETC.Core.API.Infrastructure.Services.InfringedVehicle;
using EPAY.ETC.Core.API.Infrastructure.Services.ManualBarrierControls;
using EPAY.ETC.Core.API.Infrastructure.Services.Parking.BuilderService;
using EPAY.ETC.Core.API.Infrastructure.Services.Payment;
using EPAY.ETC.Core.API.Infrastructure.Services.PaymentStatus;
using EPAY.ETC.Core.API.Infrastructure.Services.PrintLog;
using EPAY.ETC.Core.API.Infrastructure.Services.TicketType;
using EPAY.ETC.Core.API.Infrastructure.Services.TransactionLog;
using EPAY.ETC.Core.API.Infrastructure.Services.UIActions;
using EPAY.ETC.Core.API.Infrastructure.Services.Vehicles;
using EPAY.ETC.Core.Models.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<CoreDbContext>(options =>
            {
                options.UseNpgsql(Environment.GetEnvironmentVariable(CoreConstant.ENVIRONMENT_PGSQL) ?? configuration.GetConnectionString("DefaultConnection"));
                options.EnableSensitiveDataLogging();
            });

            services.AddHttpClient<IPOSService, POSService>(args =>
            {
                args.BaseAddress = new Uri(Environment.GetEnvironmentVariable(CoreConstant.ENVIRONMENT_WALLET_API_BASE) ?? configuration.GetSection("WalletAPISettings").GetValue<string>("Endpoint") ?? string.Empty);
                args.Timeout = TimeSpan.FromSeconds(120);
            });

            //Add Repositories...
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IFusionRepository, FusionRepository>();
            services.AddScoped<ICustomVehicleTypeRepository, CustomVehicleTypeRepository>();
            services.AddScoped<IFeeTypeRepository, FeeTypeRepository>();
            services.AddScoped<IFeeVehicleCategoryRepository, FeeVehicleCategoryRepository>();
            services.AddScoped<ITimeBlockFeeFormulaRepository, TimeBlockFeeFormulaRepository>();
            services.AddScoped<ITimeBlockFeeRepository, TimeBlockFeeRepository>();
            services.AddScoped<IVehicleCategoryRepository, VehicleCategoryRepository>();
            services.AddScoped<IVehicleGroupRepository, VehicleGroupRepository>();
            services.AddScoped<IFeeRepository, FeeRepository>();
            services.AddScoped<IPaymentStatusRepository, PaymentStatusRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IETCCheckoutRepository, ETCCheckoutRepository>();
            services.AddScoped<IETCCheckoutService, ETCCheckoutService>();
            services.AddScoped<IManualBarrierControlRepository, ManualBarrierControlRepository>();
            services.AddScoped<IAppConfigRepository, AppConfigRepository>();
            services.AddScoped<IBarcodeRepository, BarcodeRepository>();
            services.AddScoped<IErrorResponseRepository, ErrorResponseRepository>();
            services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();
            services.AddScoped<ILaneInCameraTransactionLogRepository, LaneInCameraTransactionLogRepository>();
            services.AddScoped<ILaneInRFIDTransactionLogRepository, LaneInRFIDTransactionLogRepository>();
            services.AddScoped<IPrintLogRepository, PrintLogRepository>();
            services.AddScoped<IInfringedVehicleRepository, InfringedVehicleRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            ////Add Services
            services.AddTransient<IVehicleService, VehicleService>();
            services.AddTransient<IFusionService, FusionService>();
            services.AddTransient<IFeeCalculationService, FeeCalculationService>();
            services.AddTransient<IFeeService, FeeService>();
            services.AddTransient<IPaymentStatusService, PaymentStatusService>();
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<IUIActionService, UIActionService>();
            services.AddTransient<IManualBarrierControlsService, ManualBarrierControlsService>();
            services.AddTransient<IBarcodeService, BarcodeService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IPasswordService, PasswordService>();
            services.AddTransient<IErrorResponseService, ErrorResponseService>();
            services.AddTransient<ILaneInCameraTransactionLogService, LaneInCameraTransactionLogService>();
            services.AddTransient<ILaneInRFIDTransactionLogService, LaneInRFIDTransactionLogService>();
            services.AddTransient<IPrintLogService, PrintLogService>();
            services.AddTransient<ITicketTypeService, TicketTypeService>();
            services.AddTransient<IInfringedVehicleService, InfringedVehicleService>();

            // parking builder
            services.AddTransient<IParkingBuilderService, GeneralParkingBuilderService>();
            services.AddTransient<IParkingBuilderService, TCPParkingBuilderService>();

            return services;
        }
    }
}
