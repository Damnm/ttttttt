using EPAY.ETC.Core.API.Core.Interfaces.Services.OrderBuilder;
using EPAY.ETC.Core.API.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Infrastructure.Services.ColumnOrderBuilder.VehicleSearchItem
{
    public class ColumnOrderBuilder : IVehicleDynamicColumnOrderBuilder<VehicleSearchItemModel>
    {
        public bool IsSupported(string columnName)
        {
            return columnName == "CreatedDate"
                || columnName == "PlateNumber";
        }

        public IEnumerable<VehicleSearchItemModel> Order(string columnName, IEnumerable<VehicleSearchItemModel> source, bool asc = true)
        {
            var columnNames = columnName.Split('.');
            string sortTypeStr = asc ? "ASC" : "DESC";

            return source.AsQueryable().OrderBy($"{columnName} {sortTypeStr}");
        }
    }
}
