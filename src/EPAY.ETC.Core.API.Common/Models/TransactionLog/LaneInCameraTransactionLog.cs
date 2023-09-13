using EPAY.ETC.Core.API.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace EPAY.ETC.Core.API.Core.Models.TransactionLog
{
    [ExcludeFromCodeCoverage]
    public class LaneInCameraTransactionLog : BaseEntity<Guid>
    {
        public Double Epoch { get; set; }
        public string? RFID { get; set; }
        public string? CameraReaderMacAddr { get; set; }
        public string? CameraReaderIPAddr { get; set; }
        public Guid LaneInId { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public string? PlateNumber { get; set; }
        public string? PlateColour { get; set; }
        public string? VehicleType { get; set; }
        public int? Seat { get; set; }
        public int? Weight { get; set; }
        public string? PlateNumberPhotoUrl { get; set; }
        public string? VehiclePhotoUrl { get; set; }
        public Double ConfidenceScore { get; set; }
    }
}
