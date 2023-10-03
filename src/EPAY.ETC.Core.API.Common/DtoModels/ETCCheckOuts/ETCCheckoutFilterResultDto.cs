using EPAY.ETC.Core.API.Core.Models.ETCCheckOuts;

namespace EPAY.ETC.Core.API.Core.DtoModels.ETCCheckOuts
{
    public class ETCCheckoutFilterResultDto
    {
        public double TotalItems { get; set; } = 0;
        public IEnumerable<ETCCheckoutDataModel> Items { get; set; }
    }
}
