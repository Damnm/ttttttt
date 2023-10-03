using EPAY.ETC.Core.API.Core.Entities;
using EPAY.ETC.Core.API.Core.Models.Payment;
using EPAY.ETC.Core.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPAY.ETC.Core.API.Core.Models.ETCCheckOuts;

[Table("ETCCheckout")]
public class ETCCheckoutDataModel : BaseEntity<Guid>
{
    public Guid PaymentId { get; set; }

    [ForeignKey("PaymentId")]
    public virtual PaymentModel? Payment { get; set; }

    [MaxLength(50)]
    public ETCServiceProviderEnum ServiceProvider { get; set; }

    [MaxLength(50)]
    public string TransactionId { get; set; }

    [MaxLength(50)]
    public TransactionStatusEnum TransactionStatus { get; set; }

    public double Amount { get; set; }

    [MaxLength(50)]
    public string? RFID { get; set; }

    [MaxLength(20)]
    public string? PlateNumber { get; set; }
}
