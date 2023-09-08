using EPAY.ETC.Core.API.Core.Entities;
using EPAY.ETC.Core.API.Core.Models.CustomVehicleTypes;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPAY.ETC.Core.API.Core.Models.TimeBlockFees
{
    [Table("TimeBlockFee")]
    public class TimeBlockFeeModel : BaseEntity<Guid>
    {
        public Guid CustomVehicleTypeId { get; set; }
        [ForeignKey("CustomVehicletypeId")]
        public virtual CustomVehicleTypeModel? CustomVehicleType { get; set; }
        public int FromSecond { get; set; } = 0;
        public int ToSecond { get; set; } = 0;
        public int? BlockDurationInSeconds { get; set; } = 0;
        public float? Amount { get; set; }
    }
}
