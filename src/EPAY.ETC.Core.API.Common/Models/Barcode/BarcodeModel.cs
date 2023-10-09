using EPAY.ETC.Core.API.Core.Entities;
using EPAY.ETC.Core.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPAY.ETC.Core.API.Core.Models.Barcode
{
    [ExcludeFromCodeCoverage]
    [Table("Barcode")]
    public class BarcodeModel : BaseEntity<Guid>
    {
        [MaxLength(20)]
        public string? EmployeeId { get; set; }
        [MaxLength(20)]
        public string? ActionCode { get; set; }
        [MaxLength(100)]
        public string? ActionDesc { get; set; }
        public BarcodeActionEnum BarcodeAction { set; get; }
    }
}
