using System.ComponentModel.DataAnnotations;

namespace EPAY.ETC.Core.API.Core.Entities
{
    public class BaseEntity<TId>
    {
        [Key]
        public TId? Id { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
