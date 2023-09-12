using EPAY.ETC.Core.Models.Enums;

namespace EPAY.ETC.Core.API.Core.Utils
{
    public static class VehicleTypeConverter
    {
        public static CustomVehicleTypeEnum ConvertVehicleType(int seat, int? payload)
        {
            if (
                (payload.HasValue
                && payload.Value > 950
                && payload.Value <= 1500)
                ||
                ((!payload.HasValue || payload.Value <= 1) && seat <= 9))
            {
                return CustomVehicleTypeEnum.Type1;
            }

            if (
                (payload.HasValue
                && payload.Value > 0
                && payload.Value <= 950
                && seat >= 3
                && seat <= 5)
                ||
                (payload.HasValue
                && payload.Value > 1500
                && payload.Value <= 3500)
                ||
                (seat >= 10 && seat <= 16)
                )
            {
                return CustomVehicleTypeEnum.Type2;
            }

            if (
                (payload.HasValue
                && payload.Value > 3500
                && payload.Value <= 7000)
                ||
                (seat >= 17 && seat <= 29))
            {
                return CustomVehicleTypeEnum.Type3;
            }

            if ((payload.HasValue && payload.Value > 7000) || seat >= 30)
            {
                return CustomVehicleTypeEnum.Type4;
            }

            return CustomVehicleTypeEnum.Type1;
        }

    }
}
