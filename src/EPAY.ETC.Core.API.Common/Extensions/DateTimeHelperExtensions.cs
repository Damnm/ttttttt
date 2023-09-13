using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace EPAY.ETC.Core.API.Core.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class DateTimeHelperExtensions
    {
        /// <summary>
        /// Converts the date and time to SE Asia Standard Time (+7:00)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime ConvertToAsianTime(this DateTime dt, DateTimeKind dateTimeKind)
        {
            dt = DateTime.SpecifyKind(dt, dateTimeKind);

            TimeZoneInfo asianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime asianTime = TimeZoneInfo.ConvertTime(dt, asianTimeZone);

            // Chuyển đổi múi giờ Asian Time sang UTC
            DateTime utcTime = asianTime.ToUniversalTime();

            return utcTime;
        }
    }
}
