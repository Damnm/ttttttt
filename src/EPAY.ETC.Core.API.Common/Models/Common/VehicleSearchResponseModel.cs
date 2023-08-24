using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Core.Models.Common
{
    public class VehicleSearchResponseModel
    {
        public VehicleSearchRequestModel SearchRequest { get; set; }
        public VehicleSearchResponseResultModel Result { get; set; }
    }

    public class VehicleSearchResponseResultModel
    {
        public int Total { get; set; }
        public List<VehicleSearchItemModel> Items { get; set; } = null!;
    }
}

