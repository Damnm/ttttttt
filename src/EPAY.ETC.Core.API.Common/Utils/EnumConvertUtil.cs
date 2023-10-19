using System.ComponentModel;

namespace EPAY.ETC.Core.API.Core.Utils
{
    public static class EnumConvertUtil
    {
        public static string ToDescriptionString(this Enum value)
        {
            DescriptionAttribute? attribute = value.GetType().GetField(value.ToString())?.GetCustomAttributes(typeof(DescriptionAttribute), false).SingleOrDefault() as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
