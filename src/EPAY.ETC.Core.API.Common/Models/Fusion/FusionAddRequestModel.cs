using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EPAY.ETC.Core.API.Core.Models.Fusion
{
    [ExcludeFromCodeCoverage]
    public class FusionAddRequestModel
    {
        public Guid? Id { get; set; }
        public long Epoch { get; set; }
        public bool Loop1 { get; set; }
        [MaxLength(50)]
        public string? RFID { get; set; }
        [MaxLength(15)]
        public string? Cam1 { get; set; }
        public bool Loop2 { get; set; }
        [MaxLength(15)]
        public string? Cam2 { get; set; }
        public bool Loop3 { get; set; }
        public bool ReversedLoop1 { get; set; }
        public bool ReversedLoop2 { get; set; }
    }
}
