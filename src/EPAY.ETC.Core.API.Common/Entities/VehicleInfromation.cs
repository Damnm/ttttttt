using EPAY.ETC.Core.API.Core.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EPAY.ETC.Core.API.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public class VehicleInfromation : BaseEntity<string>
    {
        public VehicleInfromation()
        {
            CreatedDate = DateTime.Now.ConvertToAsianTime(DateTimeKind.Local);
        }


        [MaxLength(50)]
        public string? RFID { get; set; }
        [MaxLength(20)]
        public string? PlateNumber { get; set; }
        [MaxLength(20)]
        public string? PlateColor { get; set; }
        [MaxLength(20)]
        public string? Make { get; set; }
        public int? Seat { get; set; }
        public int? Weight { get; set; }
        [MaxLength(20)]
        public string? VehicleType { get; set; }

    }
}
