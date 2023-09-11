using EPAY.ETC.Core.API.Core.Models.TimeBlockFees;

namespace EPAY.ETC.Core.API.Infrastructure.Common.Utils
{
    public static class FeeCalculationUtil
    {
        public static double FeeCalculation(List<TimeBlockFeeModel>? timeBlockFees, long duration)
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

            // Get last 2 record
            var reverse = timeBlockFees.OrderByDescending(x => x.FromSecond).Take(2);
            var lastBlock = reverse.First();
            var nextBlock = reverse.Last();

            // Calculate amount increase and time duration in 1 block
            var amountBetweenTwoBlock = (lastBlock.Amount - nextBlock.Amount) ?? 0;
            var timeBetweenTwoBlock = lastBlock.FromSecond - nextBlock.FromSecond;

            // Calculate amount
            return (double)Math.Ceiling(duration / (decimal)timeBetweenTwoBlock) * amountBetweenTwoBlock;
        }
    }
}
