using EPAY.ETC.Core.API.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Core.Models.Transaction
{
    public class VehiclePaymentTransaction: BaseEntity<Guid>
    {
        public Guid VehicleTransactionId { get; set; }
        public string? PaymentType { get; set; }
        public Boolean IsSuccessful { get; set; }
    }
}
