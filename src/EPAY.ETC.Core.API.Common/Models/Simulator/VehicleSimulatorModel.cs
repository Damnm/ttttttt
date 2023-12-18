using EPAY.ETC.Core.API.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPAY.ETC.Core.API.Core.Models.Simulator
{
    [ExcludeFromCodeCoverage]
    [Table("VehicleSimulator")]
    public class VehicleSimulatorModel : BaseEntity<Guid>
    {
        [MaxLength(50)]
        public string? Etag { get; set; }
        [MaxLength(20)]
        public string? PlateNumber { get; set; }
        public string? Currency { set; get; } 
        public int? PlateType { get; set; }
        public string? VehicleType { get; set; }
        public int? SeatNumber { get; set; }
        public string? Weight { get; set; }
        public int? VerifyType { get; set; }
        public long? Balance { get; set; }
        public EtagTypeEnum EtagType { get; set; } = EtagTypeEnum.Valid;
        public bool? IsDeleted { get; set; }
    }

    public enum EtagTypeEnum
    {
        Valid,
        Unregistered,
        InLocked,
        Inactivated
    }
}

