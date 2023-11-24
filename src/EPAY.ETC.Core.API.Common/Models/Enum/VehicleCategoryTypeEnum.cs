using System.Runtime.Serialization;

namespace EPAY.ETC.Core.API.Core.Models.Enum
{
    public enum VehicleCategoryTypeEnum
    {
        [EnumMember(Value = "Xe ưu tiên")]
        Priority,
        [EnumMember(Value = "Xe nhượng quyền")]
        Contract
    }
}
