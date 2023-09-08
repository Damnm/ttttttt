using System.Runtime.Serialization;

namespace EPAY.ETC.Core.API.Core.Models.Enum
{
    public enum FeeTypeEnum
    {

        [EnumMember(Value = "Miễn phí")]
        Free,
        [EnumMember(Value = "Phí cố định")]
        Fixed,
        [EnumMember(Value = "Tính phí theo thời gian")]
        TimeBlock,
        [EnumMember(Value = "Tính phí theo ngày")]
        DayBlock
    }
}
