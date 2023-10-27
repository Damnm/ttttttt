using EPAY.ETC.Core.API.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPAY.ETC.Core.API.Core.Models.Fusion
{
    [ExcludeFromCodeCoverage]
    [Table("Fusion")]
    public class FusionModel : BaseEntity<Guid>
    {
        public long Epoch { get; set; }
        public bool Loop1 { get; set; }
        [MaxLength(50)]
        public string? RFID { get; set; }
        [MaxLength(15)]
        public string? ANPRCam1 { get; set; }
        public bool Loop2 { get; set; }
        [MaxLength(15)]
        public string? CCTVCam2 { get; set; }
        public bool Loop3 { get; set; }
        public bool ReversedLoop1 { get; set; }
        public bool ReversedLoop2 { get; set; }
    }
}
