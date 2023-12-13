using EPAY.ETC.Core.API.Core.Models.Enum;
using EPAY.ETC.Core.API.Core.Models.TimeBlockFees;

namespace EPAY.ETC.Core.API.Infrastructure.Common.Utils
{
    public static class FeeCalculationUtil
    {
        public static (double Amount, int Block, long Duration) FeeCalculation(List<TimeBlockFeeModel>? timeBlockFees, TimeBlockFeeFormulaModel? timeBlockFeeFormula, long duration, ParkingChargeTypeEnum? parkingChargeType = null)
        {
            double fee = 0;
            int block = 0;
            long durationMinute = (long)Math.Ceiling((decimal)duration / 60);
            bool isBlock0 = parkingChargeType != null && parkingChargeType == ParkingChargeTypeEnum.Block0;

            // Return 0 if time block fee does not exists
            if (timeBlockFees == null || !timeBlockFees.Any())
                return (fee, block, durationMinute);

            // Return 0 if not exists formula
            if (timeBlockFeeFormula == null || timeBlockFeeFormula.IntervalInSeconds == 0)
            {
                return (fee, block, durationMinute);
            }

            // Get block prev has been defined
            TimeBlockFeeModel? prevBlock = timeBlockFees.FirstOrDefault(x => x.BlockNumber == (timeBlockFeeFormula.FromBlockNumber - 1));

            // find amount already defined
            var timeBlockFeeExists = timeBlockFees.FirstOrDefault(x => x.FromSecond <= duration && x.ToSecond >= duration);

            // Get total block without Block less then fromBlock has define in formula
            if (prevBlock != null)
            {
                duration -= prevBlock.ToSecond + 1;
            }

            if (duration < 0)
                duration = 0;

            decimal totalBlock = Math.Ceiling(duration / (decimal)timeBlockFeeFormula.IntervalInSeconds);
            block = timeBlockFeeExists?.BlockNumber ?? (int)totalBlock;

            double prevblockAmount = prevBlock?.Amount ?? 0;

            if (timeBlockFeeExists == null)
                block = (prevBlock?.BlockNumber ?? 0) + (int)totalBlock;

            if (isBlock0)
            {
                var block0 = timeBlockFees.FirstOrDefault(x => x.BlockNumber == 0);
                if (block0 != null)
                {
                    return (block0.Amount ?? 0, block, durationMinute);
                }
            }
            else if (parkingChargeType == ParkingChargeTypeEnum.Free)
                return (0, block, durationMinute);

            if (timeBlockFeeExists != null)
            {
                fee = timeBlockFeeExists.Amount ?? 0;

                return (fee, block, durationMinute);
            }

            // Calculate amount
            fee = (double)totalBlock * timeBlockFeeFormula.Amount + prevblockAmount;

            return (fee, block, durationMinute);
        }
    }
}
