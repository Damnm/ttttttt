using EPAY.ETC.Core.API.Core.Models.SearchRequest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Core.Models.Common
{
    public class VehicleSearchRequestModel
    {
        public VehicleSearchRefinementsModel Refinements { get; set; }
        [Required(ErrorMessage = "SearchOptions is required")]
        public SearchOptionsModel SearchOptions { get; set; } = null!;
    }

    public class VehicleSearchRefinementsModel
    {
        public string RFID { get; set; }
        public string PlateNumber { get; set; }
        public DateRangeModel DateRange { get; set; }
    }
}
