using EPAY.ETC.Core.API.Core.Entities;
using EPAY.ETC.Core.API.Core.Models.CustomVehicleTypes;
using System.Diagnostics.CodeAnalysis;

namespace EPAY.ETC.Core.API.Core.Models.TimeBlockFees
{
    [ExcludeFromCodeCoverage]
    public class TimeBlockFeeFormulaModel : BaseEntity<Guid>
    {
        public Guid CustomVehicleTypeId { get; set; }
        public CustomVehicleTypeModel? CustomVehicleType { get; set; }
        public int FromBlockNumber { get; set; }
        public double Amount { get; set; }
        public long IntervalInSeconds { get; set; }
        public DateTime ApplyDate { get; set; }
    }
}
