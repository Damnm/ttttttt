using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Core.Extensions
{
    public static class DateTimeHelperExtensions
    {
        /// <summary>
        /// Converts the date and time to SE Asia Standard Time (+7:00)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime ConvertToAsianTime(this DateTime dt, DateTimeKind dateTimeKind)
        {
            dt = DateTime.SpecifyKind(dt, dateTimeKind);
            return System.TimeZoneInfo.ConvertTime(dt, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
        }
    }
}
