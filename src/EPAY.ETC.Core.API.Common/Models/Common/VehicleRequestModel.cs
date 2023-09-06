using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Core.Models.Common
{
    public class VehicleRequestModel
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? RFID { get; set; }
        public string? PlateNumber { get; set; }
        public string? PlateColor { get; set; }
        public string? Make { get; set; }
        public int? Seat { get; set; }
        public int? Weight { get; set; }
        public string? VehicleType { get; set; }
    }
}
