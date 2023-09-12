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

            // Get block 1 has been defined
            TimeBlockFeeModel? block1 = timeBlockFees.FirstOrDefault(x => x.Order == 1);
            double block1Amount = block1?.Amount ?? 0;

            // Calculate amount increase
            double amountBetweenTwoBlock = (lastBlock.Amount - nextBlock.Amount) ?? 0;
            long timeBetweenTwoBlock = lastBlock.FromSecond - nextBlock.FromSecond;

            // If timeBetweenTwoBlock equals 0 then return
            if (timeBetweenTwoBlock == 0) return fee;

            // Minus block1 time from duration
            if (block1 != null)
            {
                duration -= block1.ToSecond + 1;
            }

            // Get total block
            decimal totalBlock = Math.Ceiling(duration / (decimal)timeBetweenTwoBlock);

            // Calculate amount
            return (double)totalBlock * amountBetweenTwoBlock + block1Amount;
        }
    }
}
