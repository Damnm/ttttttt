using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EPAY.ETC.Core.API.Core.Models.Fusion
{
    [ExcludeFromCodeCoverage]
    public class FusionUpdateRequestModel
    {
        public long Epoch { get; set; }
        public bool Loop1 { get; set; }
        [MaxLength(50)]
        [Required(ErrorMessage = "RFID required!")]
        public string? RFID { get; set; }
        [MaxLength(15)]
        [Required(ErrorMessage = "PlateNumber required!")]
        public string? Cam1 { get; set; }
        public bool Loop2 { get; set; }
        [MaxLength(15)]
        [Required(ErrorMessage = "PlateNumber required!")]
        public string? Cam2 { get; set; }
        public bool Loop3 { get; set; }
        public bool ReversedLoop1 { get; set; }
        public bool ReversedLoop2 { get; set; }
    }
}
