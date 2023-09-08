using EPAY.ETC.Core.API.Core.Entities;
using EPAY.ETC.Core.API.Core.Models.Enum;
using EPAY.ETC.Core.API.Core.Models.FeeVehicleCategories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPAY.ETC.Core.API.Core.Models.FeeTypes
{
    [Table("FeeType")]
    public class FeeTypeModel : BaseEntity<Guid>
    {
        public FeeTypeEnum Name { get; set; }
        public float? Amount { get; set; }
        [StringLength(255)]
        public string? Desc { get; set; }

        public virtual ICollection<FeeVehicleCategoryModel>? FeeVehicleCategories { get; set; }
    }
}
