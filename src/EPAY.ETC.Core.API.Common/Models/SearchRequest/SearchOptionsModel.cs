using System.Diagnostics.CodeAnalysis;

namespace EPAY.ETC.Core.API.Core.Models.SearchRequest
{
    [ExcludeFromCodeCoverage]
    public class SearchOptionsModel
    {
        public int PageSize { get; set; }
        public int Offset { get; set; }
        public string? SortBy { get; set; }
        public bool OrderByAsc { get; set; } = true;
    }
}
