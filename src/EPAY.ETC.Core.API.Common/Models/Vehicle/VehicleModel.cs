using EPAY.ETC.Core.API.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPAY.ETC.Core.API.Core.Models.Vehicle
{
    [ExcludeFromCodeCoverage]
    [Table("Vehicle")]
    public class VehicleModel : BaseEntity<Guid>
    {
        [MaxLength(50)]
        public string? RFID { get; set; }
        [MaxLength(20)]
        public string? PlateNumber { get; set; }
        [MaxLength(50)]
        public string? PlateColor { get; set; }
        [MaxLength(150)]
        public string? Make { get; set; }
        [MaxLength(150)]
        public string? Model { get; set; }
        public int? Seat { get; set; }
        public int? Weight { get; set; }
        /// <summary>
        /// TODO: Check data type: use enum instead of string
        /// </summary>
        [MaxLength(20)]
        public string? VehicleType { get; set; }
    }
}
