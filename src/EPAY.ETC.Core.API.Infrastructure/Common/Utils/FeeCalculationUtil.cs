using EPAY.ETC.Core.API.Core.Models.TimeBlockFees;

namespace EPAY.ETC.Core.API.Infrastructure.Common.Utils
{
    public static class FeeCalculationUtil
    {
        public static (double Amount, int Block, long Duration) FeeCalculation(List<TimeBlockFeeModel>? timeBlockFees, TimeBlockFeeFormulaModel? timeBlockFeeFormula, long duration)
        {
            double fee = 0;
            int block = 0;
            long durationMinute = (long)Math.Ceiling((decimal)duration / 60);

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
            double block1Amount = prevBlock?.Amount ?? 0;

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
            block = (prevBlock?.BlockNumber ?? 0) + (int)(totalBlock > 0 ? totalBlock : 0);

            if (timeBlockFeeExists != null)
            {
                fee = timeBlockFeeExists.Amount ?? 0;

                return (fee, block, durationMinute);
            }

            // Calculate amount
            fee = (double)totalBlock * timeBlockFeeFormula.Amount + block1Amount;

            return (fee, block, durationMinute);
        }
    }
}
