using EPAY.ETC.Core.API.Core.Entities;
using EPAY.ETC.Core.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPAY.ETC.Core.API.Core.Models.PaymentStatus
{
    [Table("PaymentStatus")]
    public class PaymentStatusModel : BaseEntity<Guid>
    {
        public Guid PaymentId { get; set; }

        public Double Amount { get; set; }

        public string Currency { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }

        public DateTime PaymentDate { get; set; }

        public PaymentStatusEnum Status { get; set; }

    }
}
