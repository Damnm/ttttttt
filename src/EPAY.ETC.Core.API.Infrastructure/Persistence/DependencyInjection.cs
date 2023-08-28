using EPAY.ETC.Core.API.Core.Interfaces.Services.OrderBuilder;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Vehicles;
using EPAY.ETC.Core.API.Core.Models.Common;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Vehicle;
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
            });

            //Add Repositories...
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddTransient<IVehicleDynamicColumnOrderBuilder<VehicleSearchItemModel>, Services.ColumnOrderBuilder.VehicleSearchItem.ColumnOrderBuilder>();
            services.AddTransient<IVehicleDynamicColumnOrderService<VehicleSearchItemModel>, VehicleColumnOrderService>();
            ////Add Services
            return services;
        }
    }
}
