using EPAY.ETC.Core.API.Core.Entities;
using EPAY.ETC.Core.API.Core.Models.FeeVehicleCategories;
using EPAY.ETC.Core.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPAY.ETC.Core.API.Core.Models.FeeTypes
{
    [Table("FeeType")]
    [ExcludeFromCodeCoverage]
    public class FeeTypeModel : BaseEntity<Guid>
    {
        public Enum.FeeTypeEnum FeeName { get; set; }
        public CustomVehicleTypeEnum? CustomVehicleType { get; set; }
        public double? Amount { get; set; }

        [MaxLength(255)]
        public string? Desc { get; set; }

        public virtual ICollection<FeeVehicleCategoryModel>? FeeVehicleCategories { get; set; }
    }
}
