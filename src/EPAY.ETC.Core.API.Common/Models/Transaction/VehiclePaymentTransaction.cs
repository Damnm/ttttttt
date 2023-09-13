using EPAY.ETC.Core.API.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace EPAY.ETC.Core.API.Core.Models.Transaction
{
    [ExcludeFromCodeCoverage]
    public class VehiclePaymentTransaction : BaseEntity<Guid>
    {
        public Guid VehicleTransactionId { get; set; }
        public string? PaymentType { get; set; }
        public Boolean IsSuccessful { get; set; }
    }
}
