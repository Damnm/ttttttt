using EPAY.ETC.Core.API.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Core.Entities
{
    public class VehicleInfromation : BaseEntity<string>
    {
        public VehicleInfromation() 
        {
            CreatedDate = DateTime.Now.ConvertToAsianTime(DateTimeKind.Local);
        }

        [Key]
        [StringLength(50)]
        public string RFID { get; set; }
        [StringLength(30)]
        public string PlateNumber { get; set; }
        [StringLength(30)]
        public string PlateColor { get; set; }
        [StringLength(30)]
        public string Make { get; set; }
        public int Seat { get; set; }
        public int Weight { get; set; }
        [StringLength(30)]
        public string VehicleType { get; set; }

    }
}
