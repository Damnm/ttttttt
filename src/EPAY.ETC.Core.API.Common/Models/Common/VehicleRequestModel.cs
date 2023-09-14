using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace EPAY.ETC.Core.API.Core.Models.Common
{
    [ExcludeFromCodeCoverage]
    public class VehicleRequestModel
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        [JsonIgnore]
        public DateTime? CreatedDate { get; set; }
        [Required(ErrorMessage = "RFID required!")]
        public string RFID { get; set; }
        [Required(ErrorMessage = "PlateNumber required!")]
        public string? PlateNumber { get; set; }
        public string? PlateColor { get; set; }
        public string? Make { get; set; }
        public int? Seat { get; set; }
        public int? Weight { get; set; }
        public string? VehicleType { get; set; }
    }
}
