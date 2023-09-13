using EPAY.ETC.Core.API.Core.Interfaces.Services.Fees;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Fusion;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Vehicles;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.CustomVehicleTypes;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.FeeTypes;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.FeeVehicleCategories;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Fusion;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.TimeBlockFees;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Vehicle;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.VehicleCategories;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.VehicleGroups;
using EPAY.ETC.Core.API.Infrastructure.Services.Fees;
using EPAY.ETC.Core.API.Infrastructure.Services.Fusion;
using EPAY.ETC.Core.API.Infrastructure.Services.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence
{
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

            ////Add Services
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IFusionService, FusionService>();
            services.AddScoped<IFeeCalculationService, FeeCalculationService>();

            return services;
        }
    }
}
