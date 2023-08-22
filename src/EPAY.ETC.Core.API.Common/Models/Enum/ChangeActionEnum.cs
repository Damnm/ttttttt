using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
