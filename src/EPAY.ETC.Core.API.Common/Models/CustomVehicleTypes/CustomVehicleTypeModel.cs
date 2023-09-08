using EPAY.ETC.Core.API.Core.Entities;
using EPAY.ETC.Core.API.Core.Models.FeeVehicleCategories;
using EPAY.ETC.Core.API.Core.Models.TimeBlockFees;
using EPAY.ETC.Core.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPAY.ETC.Core.API.Core.Models.CustomVehicleTypes
{
    [Table("CustomVehicleType")]
    public class CustomVehicleTypeModel : BaseEntity<Guid>
    {
        public CustomVehicleTypeEnum Name { get; set; }
        [StringLength(255)]
        public string? Desc { get; set; }

        public virtual ICollection<FeeVehicleCategoryModel>? FeeVehicleCategories { get; set; }
        public virtual ICollection<TimeBlockFeeModel>? TimeBlockFees { get; set; }
    }
}
