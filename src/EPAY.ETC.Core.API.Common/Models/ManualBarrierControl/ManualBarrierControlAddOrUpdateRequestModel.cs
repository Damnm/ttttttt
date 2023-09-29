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
        public Guid? EmployeeId { get; set; }
        public ActionEnum Action { get; set; }
        public string LaneOutId { get; set; }
    }
}

