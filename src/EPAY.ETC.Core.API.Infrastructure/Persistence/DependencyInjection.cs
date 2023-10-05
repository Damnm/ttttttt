using EPAY.ETC.Core.API.Core.Interfaces.Services.Authentication;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Barcode;
using EPAY.ETC.Core.API.Core.Interfaces.Services.ETCCheckouts;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Fees;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Fusion;
using EPAY.ETC.Core.API.Core.Interfaces.Services.ManualBarrierControls;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Payment;
using EPAY.ETC.Core.API.Core.Interfaces.Services.PaymentStatus;
using EPAY.ETC.Core.API.Core.Interfaces.Services.UIActions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Vehicles;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.AppConfigs;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Barcode;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.CustomVehicleTypes;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ETCCheckouts;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Fees;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.FeeTypes;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.FeeVehicleCategories;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Fusion;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ManualBarrierControls;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Payment;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.PaymentStatus;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.TimeBlockFees;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Vehicle;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.VehicleCategories;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.VehicleGroups;
using EPAY.ETC.Core.API.Infrastructure.Services.Authentication;
using EPAY.ETC.Core.API.Infrastructure.Services.Barcode;
using EPAY.ETC.Core.API.Infrastructure.Services.ETCCheckouts;
using EPAY.ETC.Core.API.Infrastructure.Services.Fees;
using EPAY.ETC.Core.API.Infrastructure.Services.Fusion;
using EPAY.ETC.Core.API.Infrastructure.Services.ManualBarrierControls;
using EPAY.ETC.Core.API.Infrastructure.Services.Payment;
using EPAY.ETC.Core.API.Infrastructure.Services.PaymentStatus;
using EPAY.ETC.Core.API.Infrastructure.Services.UIActions;
using EPAY.ETC.Core.API.Infrastructure.Services.Vehicles;
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
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                options.EnableSensitiveDataLogging();
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

            ////Add Services
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IFusionService, FusionService>();
            services.AddScoped<IFeeCalculationService, FeeCalculationService>();
            services.AddScoped<IFeeService, FeeService>();
            services.AddScoped<IPaymentStatusService, PaymentStatusService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IUIActionService, UIActionService>();
            services.AddScoped<IManualBarrierControlsService, ManualBarrierControlsService>();
            services.AddScoped<IBarcodeService, BarcodeService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            return services;
        }
    }
}
