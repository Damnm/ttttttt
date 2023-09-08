using EPAY.ETC.Core.API.Core.Entities;
using EPAY.ETC.Core.API.Core.Models.FeeVehicleCategories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPAY.ETC.Core.API.Core.Models.VehicleCategories
{
    [Table("VehicleCategory")]
    public class VehicleCategoryModel : BaseEntity<Guid>
    {
        [StringLength(100)]
        public string? Name { get; set; }
        [StringLength(255)]
        public string? Desc { get; set; }

        public virtual ICollection<FeeVehicleCategoryModel>? FeeVehicleCategories { get; set; }
    }
}
