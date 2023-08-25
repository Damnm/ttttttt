using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.OrderBuilder
{
    public interface IVehicleDynamicColumnOrderBuilder<T>
    {
        bool IsSupported(string columnName);
        IEnumerable<T> Order(string columnName, IEnumerable<T> source, bool asc = true);
    }
}
