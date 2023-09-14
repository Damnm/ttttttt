using EPAY.ETC.Core.API.Core.Entities;
using EPAY.ETC.Core.API.Core.Models.CustomVehicleTypes;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPAY.ETC.Core.API.Core.Models.Fees
{
    [ExcludeFromCodeCoverage]
    public class FeeModel : BaseEntity<Guid>
    {
        public Guid ObjectId { get; set; }
        public string? LaneInId { get; set; }
        public DateTime? LaneInDate { get; set; }
        public long? LaneInEpoch { get; set; }
        public string? LaneOutId { get; set; }
        public DateTime? LaneOutDate { get; set; }
        public long? LaneOutEpoch { get; set; }
        public int Duration { get; set; }
        public string? RFID { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public string? PlateNumber { get; set; }
        public string? PlateColour { get; set; }
        public Guid? CustomVehicleTypeId { get; set; }
        [ForeignKey("CustomVehicleTypeId")]
        public virtual CustomVehicleTypeModel? CustomVehicleType { get; set; }
        public int? Seat { get; set; }
        public int? Weight { get; set; }
        public string? LaneInPlateNumberPhotoUrl { get; set; }
        public string? LaneInVehiclePhotoUrl { get; set; }
        public string? LaneOutPlateNumberPhotoUrl { get; set; }
        public string? LaneOutVehiclePhotoUrl { get; set; }
        public float? ConfidenceScore { get; set; }
        public double Amount { get; set; }
        public Guid? VehicleCategoryId { get; set; }
        public string? TicketTypeId { get; set; }
        public string? TicketId { get; set; }
        public Guid? ShiftId { get; set; }
        public Guid? EmployeeId { get; set; }
    }
}
