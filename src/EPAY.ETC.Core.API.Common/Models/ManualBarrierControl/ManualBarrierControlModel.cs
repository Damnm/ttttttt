using EPAY.ETC.Core.API.Core.Entities;
using EPAY.ETC.Core.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPAY.ETC.Core.API.Core.Models.ManualBarrierControl
{
    [Table("ManualBarrierControl")]
    public class ManualBarrierControlModel : BaseEntity<Guid>
    {
        public string? EmployeeId { get; set; }
        public BarrierActionEnum Action { get; set; }
        public string LaneOutId { get; set; }
    }
}
