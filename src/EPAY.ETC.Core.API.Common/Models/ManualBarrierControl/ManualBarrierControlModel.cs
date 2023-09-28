using EPAY.ETC.Core.API.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPAY.ETC.Core.API.Core.Models.ManualBarrierControl
{
    [Table("ManualBarrierControl")]
    public class ManualBarrierControlModel : BaseEntity<Guid>
    {
        public Guid? EmployeeId { get; set; }
        public ActionEnum Action { get; set; }
        public string LaneOutId { get; set; }
    }
    public enum ActionEnum
    {
        Open,
        Close
    }
}
