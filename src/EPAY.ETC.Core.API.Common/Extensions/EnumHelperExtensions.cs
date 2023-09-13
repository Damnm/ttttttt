using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace EPAY.ETC.Core.API.Core.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class EnumHelperExtensions
    {
        public static string ToEnumMemberAttrValue(this Enum @enum)
        {
            var attr = @enum.GetType().GetMember(@enum.ToString()).FirstOrDefault()?.
                    GetCustomAttributes(false).OfType<EnumMemberAttribute>().
                    FirstOrDefault();

            if (attr == null)
                return @enum.ToString();

            return attr?.Value ?? string.Empty;
        }
    }
}
