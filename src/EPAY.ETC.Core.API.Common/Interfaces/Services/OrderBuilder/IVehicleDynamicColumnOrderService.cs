using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.OrderBuilder
{
    public interface IVehicleDynamicColumnOrderService<T>
    {
        IEnumerable<T> Order(IEnumerable<T> source, string columnName, bool asc = true);
    }
}
