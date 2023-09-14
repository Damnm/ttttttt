using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace EPAY.ETC.Core.API.Core.Models.Common
{
    [ExcludeFromCodeCoverage]
    public class FusionRequestModel
    {
        [JsonIgnore]
        public Guid ObjectId { get; set; }
        public float Epoch { get; set; }
        public bool Loop1 { get; set; }
        [Required(ErrorMessage = "RFID required!")]
        public bool RFID { get; set; }
        [StringLength(15)]
        [Required(ErrorMessage = "PlateNumber required!")]
        public string? Cam1 { get; set; }
        public bool Loop2 { get; set; }
        [StringLength(15)]
        [Required(ErrorMessage = "PlateNumber required!")]
        public string? Cam2 { get; set; }
        public bool Loop3 { get; set; }
        public bool ReversedLoop1 { get; set; }
        public bool ReversedLoop2 { get; set; }
    }
}
