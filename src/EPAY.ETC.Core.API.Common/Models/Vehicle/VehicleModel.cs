using EPAY.ETC.Core.API.Core.Entities;

namespace EPAY.ETC.Core.API.Core.Models.Vehicle
{
    public class VehicleModel : BaseEntity<Guid>
    {
        public string? RFID { get; set; }
        public string? PlateNumber { get; set; }
        public string? PlateColor { get; set; }
        public string? Make { get; set; }
        public int? Seat { get; set; }
        public int? Weight { get; set; }
        /// <summary>
        /// TODO: Check data type: use enum instead of string
        /// </summary>
        public string? VehicleType { get; set; }
    }
}
