using EPAY.ETC.Core.API.Core.Entities;
using EPAY.ETC.Core.API.Core.Models.FeeVehicleCategories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPAY.ETC.Core.API.Core.Models.VehicleGroups
{
    [Table("VehicleGroup")]
    [ExcludeFromCodeCoverage]
    public class VehicleGroupModel : BaseEntity<Guid>
    {
        [MaxLength(100)]
        public string? GroupName { get; set; }
        [MaxLength(255)]
        public string? Desc { get; set; }

        public virtual ICollection<FeeVehicleCategoryModel>? FeeVehicleCategories { get; set; }
    }
}
