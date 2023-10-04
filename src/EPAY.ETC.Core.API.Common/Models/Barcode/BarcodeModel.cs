using EPAY.ETC.Core.API.Core.Entities;
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
        public string? ActionCode { get; set; }
        public string? ActionDesc { get; set; }
    }
}
