using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace EPAY.ETC.Core.API.Core.Models.Common
{
    public class VehicleSearchItemModel
    {
        public Guid Id { get; set; }
        public string? RFID { get; set; }
        public string? PlateNumber { get; set; }
        public string? PlateColor { get; set; }
        public string? Make { get; set; }
        public int? Seat { get; set; }
        public int? Weight { get; set; }
        public string? VehicleType { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime CreatedDate { get; set; }
    }
}
