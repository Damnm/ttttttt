using EPAY.ETC.Core.API.Core.Entities;
using EPAY.ETC.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Core.Models.ManualBarrierControl
{
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
