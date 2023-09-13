using System.Diagnostics.CodeAnalysis;

namespace EPAY.ETC.Core.API.Core.Models.SearchRequest
{
    [ExcludeFromCodeCoverage]
    public class DateRangeModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
