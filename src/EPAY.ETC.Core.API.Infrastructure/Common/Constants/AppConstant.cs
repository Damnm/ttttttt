using EPAY.ETC.Core.API.Core.Models.Shifts;

namespace EPAY.ETC.Core.API.Infrastructure.Common.Constants
{
    public static class AppConstant
    {
        public static IDictionary<string, MockShiftModel> ShiftValuePairs = new Dictionary<string, MockShiftModel>()
        {
            {
                "Ca 1",
                new MockShiftModel()
                {
                    Id = Guid.Parse("2ADFA141-4245-4FC0-9AF5-9FE4EA9BB2A2"),
                    Name = "Ca 1",
                    StartTime = 0,
                    EndTime = 720
                }
            },
            {
                "Ca 2",
                new MockShiftModel()
                {
                    Id = Guid.Parse("BF10449D-622A-488D-A807-A57828CD210D"),
                    Name = "Ca 2",
                    StartTime = 720,
                    EndTime = 0
                }
            }
        };
    }
}
