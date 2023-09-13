using System.Runtime.Serialization;

namespace EPAY.ETC.Core.API.Core.Models.Enum
{
    public enum ChangeActionEnum
    {
        [EnumMember(Value = "Không thao tác")]
        None,
        [EnumMember(Value = "Xoá")]
        Delete,
        [EnumMember(Value = "Thêm mới")]
        Insert,
        [EnumMember(Value = "Chỉnh sửa")]
        Update
    }
}
