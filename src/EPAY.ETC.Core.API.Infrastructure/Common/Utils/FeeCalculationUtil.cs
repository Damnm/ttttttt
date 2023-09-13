using EPAY.ETC.Core.API.Core.Models.TimeBlockFees;

namespace EPAY.ETC.Core.API.Infrastructure.Common.Utils
{
    public static class FeeCalculationUtil
    {
        public static double FeeCalculation(List<TimeBlockFeeModel>? timeBlockFees, TimeBlockFeeFormulaModel? timeBlockFeeFormula, long duration)
        {
            double fee = 0;

            // Return 0 if time block fee does not exists
            if (timeBlockFees == null || !timeBlockFees.Any())
                return fee;

            // find amount already defined
            var timeBlockFeeExists = timeBlockFees.FirstOrDefault(x => x.FromSecond <= duration && x.ToSecond >= duration);
            if (timeBlockFeeExists != null)
            {
                return timeBlockFeeExists.Amount ?? 0;
            }

            // Return 0 if not exists formula
            if (timeBlockFeeFormula == null || timeBlockFeeFormula.IntervalInSeconds == 0)
            {
                return fee;
            }

            // Get block prev has been defined
            TimeBlockFeeModel? prevBlock = timeBlockFees.FirstOrDefault(x => x.BlockNumber == (timeBlockFeeFormula.FromBlockNumber - 1));
            double block1Amount = prevBlock?.Amount ?? 0;

            // Get total block without Block less then fromBlock has define in formula
            if (prevBlock != null)
            {
                duration -= prevBlock.ToSecond + 1;
            }

            decimal totalBlock = Math.Ceiling(duration / (decimal)timeBlockFeeFormula.IntervalInSeconds);

            // Calculate amount
            return (double)totalBlock * timeBlockFeeFormula.Amount + block1Amount;
        }
    }
}
