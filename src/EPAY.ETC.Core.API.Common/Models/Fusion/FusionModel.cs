using EPAY.ETC.Core.API.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EPAY.ETC.Core.API.Core.Models.Fusion
{
    [ExcludeFromCodeCoverage]
    public class FusionModel : BaseEntity<Guid>
    {
        public float Epoch { get; set; }
        public bool Loop1 { get; set; }
        public bool RFID { get; set; }
        [StringLength(15)]
        public string? Cam1 { get; set; }
        public bool Loop2 { get; set; }
        [StringLength(15)]
        public string? Cam2 { get; set; }
        public bool Loop3 { get; set; }
        public bool ReversedLoop1 { get; set; }
        public bool ReversedLoop2 { get; set; }
    }
}
