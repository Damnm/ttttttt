using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EPAY.ETC.Core.API.Core.Entities
{
    public class BaseEntity<TId>
    {
        [Key]       
        public TId? Id { get; set; }       
        public DateTime? CreatedDate { get; set; }
    }
}
 