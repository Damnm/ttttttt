using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Core.Models.Simulator
{
    [ExcludeFromCodeCoverage]
    public class VehicleSimulatorRequestModel
    {
        [Required(ErrorMessage = "RFID required!")]
        public string? Etag { get; set; }
        [Required(ErrorMessage = "PlateNumber required!")]
        public string? PlateNumber { get; set; }
        public string? Currency { set; get; }
        public int? PlateType { get; set; }
        public string? VehicleType { get; set; }
        public int? SeatNumber { get; set; }
        public string? Weight { get; set; }
        public int? VerifyType { get; set; }
        public long? Balance { get; set; }
        public string EtagType { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
