using EPAY.ETC.Core.API.Core.Interfaces.Services.OrderBuilder;
using EPAY.ETC.Core.API.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Infrastructure.Services.Vehicles
{
    public class VehicleColumnOrderService : IVehicleDynamicColumnOrderService<VehicleSearchItemModel>
    {
        private readonly IEnumerable<IVehicleDynamicColumnOrderBuilder<VehicleSearchItemModel>> _builders;

        public VehicleColumnOrderService(IEnumerable<IVehicleDynamicColumnOrderBuilder<VehicleSearchItemModel>> builders)
        {
            _builders = builders;
        }

        public IEnumerable<VehicleSearchItemModel> Order(IEnumerable<VehicleSearchItemModel> source, string columnName, bool asc = true)
        {
            var builder = _builders.FirstOrDefault(x => x.IsSupported(columnName));
            if (builder == null)
            {
                throw new NotImplementedException($"Order by {columnName} not inherit");
            }

            return builder.Order(columnName, source, asc);
        }
    }
}
