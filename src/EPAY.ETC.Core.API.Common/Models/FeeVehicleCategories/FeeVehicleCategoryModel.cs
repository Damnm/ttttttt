using EPAY.ETC.Core.API.Core.Entities;
using EPAY.ETC.Core.API.Core.Models.CustomVehicleTypes;
using EPAY.ETC.Core.API.Core.Models.FeeTypes;
using EPAY.ETC.Core.API.Core.Models.VehicleCategories;
using EPAY.ETC.Core.API.Core.Models.VehicleGroups;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPAY.ETC.Core.API.Core.Models.FeeVehicleCategories
{
    [Table("FeeVehicleCategory")]
    [ExcludeFromCodeCoverage]
    public class FeeVehicleCategoryModel : BaseEntity<Guid>
    {
        public Guid VehicleCategoryId { get; set; }
        [ForeignKey("VehicleCategoryId")]
        public virtual VehicleCategoryModel? VehicleCategory { get; set; }
        public Guid FeeTypeId { get; set; }
        [ForeignKey("FeeTypeId")]
        public virtual FeeTypeModel? FeeType { get; set; }
        public Guid VehicleGroupId { get; set; }
        [ForeignKey("VehicleGroupId")]
        public virtual VehicleGroupModel? VehicleGroup { get; set; }
        public Guid CustomVehicleTypeId { get; set; }
        [ForeignKey("CustomVehicleTypeId")]
        public virtual CustomVehicleTypeModel? CustomVehicleType { get; set; }
        [MaxLength(20)]
        public string? PlateNumber { get; set; }
        [MaxLength(50)]
        public string? RFID { get; set; }
        public bool IsTCPVehicle { get; set; } = false;
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
    }
}
