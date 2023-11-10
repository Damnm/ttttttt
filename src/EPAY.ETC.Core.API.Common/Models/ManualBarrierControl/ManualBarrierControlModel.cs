using EPAY.ETC.Core.API.Core.Entities;
using EPAY.ETC.Core.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPAY.ETC.Core.API.Core.Models.ManualBarrierControl
{
    [Table("ManualBarrierControl")]
    public class ManualBarrierControlModel : BaseEntity<Guid>
    {
        [MaxLength(20)]
        public string? EmployeeId { get; set; }
        public BarrierActionEnum Action { get; set; }
        [MaxLength(10)]
        public string LaneOutId { get; set; }
        public string? ManualBarrierType { get; set; }
    }
}
