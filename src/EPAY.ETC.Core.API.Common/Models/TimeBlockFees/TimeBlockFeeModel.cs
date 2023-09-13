using EPAY.ETC.Core.API.Core.Entities;
using EPAY.ETC.Core.API.Core.Models.CustomVehicleTypes;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPAY.ETC.Core.API.Core.Models.TimeBlockFees
{
    [Table("TimeBlockFee")]
    [ExcludeFromCodeCoverage]
    public class TimeBlockFeeModel : BaseEntity<Guid>
    {
        public Guid CustomVehicleTypeId { get; set; }
        [ForeignKey("CustomVehicleTypeId")]
        public virtual CustomVehicleTypeModel? CustomVehicleType { get; set; }
        public long FromSecond { get; set; } = 0;
        public long ToSecond { get; set; } = 0;
        public int? BlockDurationInSeconds { get; set; } = 0;
        public double? Amount { get; set; }
        public int BlockNumber { set; get; } = 0;
    }
}
