using EPAY.ETC.Core.API.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPAY.ETC.Core.API.Core.Models.Configs
{
    [Table("AppConfig")]
    public class AppConfigModel : BaseEntity<Guid>
    {
        public bool IsApply { get; set; } = false;
        [MaxLength(250)]
        public string AppName { get; set; } = string.Empty;
        [MaxLength(250)]
        public string? HeaderHeading { get; set; }
        [MaxLength(250)]
        public string? HeaderSubHeading { get; set; }
        [MaxLength(250)]
        public string? HeaderLine1 { get; set; }
        [MaxLength(250)]
        public string? HeaderLine2 { get; set; }
        [MaxLength(250)]
        public string? FooterLine1 { get; set; }
        [MaxLength(250)]
        public string? FooterLine2 { get; set; }
    }
}
