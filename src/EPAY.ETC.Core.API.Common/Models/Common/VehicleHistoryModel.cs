using EPAY.ETC.Core.API.Core.Entities;
using EPAY.ETC.Core.API.Core.Extensions;
using EPAY.ETC.Core.API.Core.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Core.Models.Common
{
    public class VehicleHistoryModel: BaseEntity<Guid>
    {
        public VehicleHistoryModel()
        {
            CreatedDate = DateTime.Now.ConvertToAsianTime(DateTimeKind.Local);
        }
        [StringLength(50)]
        public string RFID { get; set; }
        [StringLength(20)]
        public string PlateNumber { get; set; }
        [StringLength(20)]
        public string PlateColor { get; set; }
        [StringLength(20)]
        public string Make { get; set; }
        public int Seat { get; set; }
        public int Weight { get; set; }
        [StringLength(20)]
        public string VehicleType { get; set; }
        public ChangeActionEnum Action { get; set; }
    }
}
