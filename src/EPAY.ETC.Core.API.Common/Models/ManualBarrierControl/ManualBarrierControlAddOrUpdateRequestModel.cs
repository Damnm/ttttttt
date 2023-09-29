using EPAY.ETC.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Core.Models.ManualBarrierControl
{
    public class ManualBarrierControlAddOrUpdateRequestModel
    {
        [Required(ErrorMessage = "EmployeeId required!")]
        public string? EmployeeId { get; set; }
        public BarrierActionEnum Action { get; set; }
        public string LaneOutId { get; set; }
    }
}

