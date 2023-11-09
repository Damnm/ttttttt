using EPAY.ETC.Core.API.Core.Entities;
using EPAY.ETC.Core.API.Core.Models.Fees;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPAY.ETC.Core.API.Core.Models.TicketType
{
    [ExcludeFromCodeCoverage]
    [Table("TicketType")]
    public class TicketTypeModel : BaseEntity<Guid>
    {
        [MaxLength(50)]
        public string Code { get; set; }
        [MaxLength(150)]
        public string Name { get; set; }
        [MaxLength(250)]
        public string? Description { get; set; }

        public virtual ICollection<FeeModel> Fees { get; set; }
    }
}
