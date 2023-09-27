using EPAY.ETC.Core.API.Core.Entities;
using EPAY.ETC.Core.API.Core.Models.CustomVehicleTypes;
using EPAY.ETC.Core.API.Core.Models.ETCCheckOuts;
using EPAY.ETC.Core.API.Core.Models.Fees;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPAY.ETC.Core.API.Core.Models.Payment
{
    [Table("Payment")]
    public class PaymentModel : BaseEntity<Guid>
    {
        [MaxLength(10)]
        public string? LaneInId { get; set; }

        public Guid? FeeId { get; set; }
        [ForeignKey(nameof(FeeId))]
        public virtual FeeModel? Fee { get; set; }

        [MaxLength(10)]
        public string? LaneOutId { get; set; }
        public int Duration { get; set; }
        [MaxLength(50)]
        public string? RFID { get; set; }
        [MaxLength(150)]
        public string? Make { get; set; }
        [MaxLength(150)]
        public string? Model { get; set; }
        [MaxLength(20)]
        public string? PlateNumber { get; set; }
        public string? VehicleTypeId { get; set; }
        public Guid? CustomVehicleTypeId { get; set; }
        [ForeignKey(nameof(CustomVehicleTypeId))]
        public virtual CustomVehicleTypeModel? CustomVehicleType { get; set; }

        public double Amount { get; set; }

        public virtual ICollection<PaymentStatus.PaymentStatusModel>? PaymentStatuses { get; set; }
        public virtual ICollection<ETCCheckoutDataModel>? ETCCheckOuts { get; set; }
    }
}
