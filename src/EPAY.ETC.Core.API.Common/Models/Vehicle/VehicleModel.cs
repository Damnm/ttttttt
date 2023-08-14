using EPAY.ETC.Core.API.Core.Entities;

namespace EPAY.ETC.Core.API.Core.Models.Vehicle
{
    public class VehicleModel : BaseEntity<Guid>
    {
        public string? PlateNumber { get; set; }
        public string? PlateColor { get; set; }
        public string? Make { get; set; }
        public string? Seat { get; set; }
        public int Weight { get; set; }
        /// <summary>
        /// TODO: Check data type: use enum instead of string
        /// </summary>
        public string? VehicleType { get; set; }
    }
}
