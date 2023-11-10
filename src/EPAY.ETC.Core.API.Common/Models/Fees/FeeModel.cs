using EPAY.ETC.Core.API.Core.Entities;
using EPAY.ETC.Core.API.Core.Models.CustomVehicleTypes;
using EPAY.ETC.Core.API.Core.Models.Payment;
using EPAY.ETC.Core.API.Core.Models.TicketType;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPAY.ETC.Core.API.Core.Models.Fees
{
    [ExcludeFromCodeCoverage]
    [Table("Fee")]
    public class FeeModel : BaseEntity<Guid>
    {
        public Guid ObjectId { get; set; }
        [MaxLength(10)]
        public string? LaneInId { get; set; }
        public DateTime? LaneInDate { get; set; }
        public long? LaneInEpoch { get; set; }
        [MaxLength(10)]
        public string? LaneOutId { get; set; }
        public DateTime? LaneOutDate { get; set; }
        public long? LaneOutEpoch { get; set; }
        public int Duration { get; set; }
        [MaxLength(50)]
        public string? RFID { get; set; }
        [MaxLength(150)]
        public string? Make { get; set; }
        [MaxLength(150)]
        public string? Model { get; set; }
        [MaxLength(20)]
        public string? PlateNumber { get; set; }
        [MaxLength(50)]
        public string? PlateColour { get; set; }
        public Guid? CustomVehicleTypeId { get; set; }
        [ForeignKey("CustomVehicleTypeId")]
        public virtual CustomVehicleTypeModel? CustomVehicleType { get; set; }
        public int? Seat { get; set; }
        public int? Weight { get; set; }
        [MaxLength(255)]
        public string? LaneInPlateNumberPhotoUrl { get; set; }
        [MaxLength(255)]
        public string? LaneInVehiclePhotoUrl { get; set; }
        [MaxLength(255)]
        public string? LaneOutPlateNumberPhotoUrl { get; set; }
        [MaxLength(255)]
        public string? LaneOutVehiclePhotoUrl { get; set; }
        public float? ConfidenceScore { get; set; }
        public double Amount { get; set; }
        public Guid? VehicleCategoryId { get; set; }
        [MaxLength(50)]
        public string? TicketId { get; set; }
        [MaxLength(50)]
        public string? ShiftId { get; set; }
        [MaxLength(20)]
        public string? EmployeeId { get; set; }

        public Guid? TicketTypeId { get; set; }
        [ForeignKey("TicketTypeId")]
        public virtual TicketTypeModel? TicketType { get; set; }

        public virtual ICollection<PaymentModel> Payments { get; set; }
    }
}
