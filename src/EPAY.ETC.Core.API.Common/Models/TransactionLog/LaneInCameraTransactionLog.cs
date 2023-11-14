using EPAY.ETC.Core.API.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPAY.ETC.Core.API.Core.Models.TransactionLog
{
    [ExcludeFromCodeCoverage]
    [Table("LaneInCameraTransactionLog")]
    public class LaneInCameraTransactionLog : BaseEntity<Guid>
    {
        public long Epoch { get; set; }
        [MaxLength(50)]
        public string? RFID { get; set; }
        [MaxLength(50)]
        public string? CameraMacAddr { get; set; }
        [MaxLength(50)]
        public string? CameraIPAddr { get; set; }
        [MaxLength(10)]
        public string? LaneInId { get; set; }
        [MaxLength(150)]
        public string? Make { get; set; }
        [MaxLength(150)]
        public string? Model { get; set; }
        [MaxLength(20)]
        public string? PlateNumber { get; set; }
        [MaxLength(50)]
        public string? PlateColour { get; set; }
        [MaxLength(20)]
        public string? VehicleType { get; set; }
        public int? Seat { get; set; }
        public int? Weight { get; set; }
        [MaxLength(255)]
        public string? PlateNumberPhotoUrl { get; set; }
        [MaxLength(255)]
        public string? VehiclePhotoUrl { get; set; }
        public double ConfidenceScore { get; set; }
    }
}
