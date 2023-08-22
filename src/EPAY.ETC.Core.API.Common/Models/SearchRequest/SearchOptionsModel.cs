using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Core.Models.SearchRequest
{
    public class SearchOptionsModel
    {
        public int PageSize { get; set; }
        public int Offset { get; set; }
        public string? SortBy { get; set; }
        public bool OrderByAsc { get; set; } = true;
    }
}
