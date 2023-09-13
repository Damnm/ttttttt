using EPAY.ETC.Core.API.Core.Models.TimeBlockFees;
using EPAY.ETC.Core.API.Infrastructure.Common.Utils;
using FluentAssertions;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Common.Utils
{
    public class FeeCalculationUtilTests
    {
        #region Init Mock data
        private static List<TimeBlockFeeModel> firstTimeBlockFee = new List<TimeBlockFeeModel>();
        private static List<TimeBlockFeeModel> secondTimeBlockFee = new List<TimeBlockFeeModel>() {
            new TimeBlockFeeModel()
            {
                Id = Guid.NewGuid(),
                CustomVehicleTypeId = Guid.NewGuid(),
                FromSecond = 0,
                ToSecond = 599,
                Amount = 9000,
                BlockDurationInSeconds = 1800,
                CreatedDate = new DateTime(2023,9,11),
                BlockNumber = 0
            }
        };
        private static List<TimeBlockFeeModel> thirdTimeBlockFee = new List<TimeBlockFeeModel>() {
            new TimeBlockFeeModel()
            {
                Id = Guid.NewGuid(),
                CustomVehicleTypeId = Guid.NewGuid(),
                FromSecond = 0,
                ToSecond = 599,
                Amount=9000,
                BlockDurationInSeconds = 1800,
                CreatedDate = new DateTime(2023,9,11),
                BlockNumber = 0
            },
            new TimeBlockFeeModel()
            {
                Id = Guid.NewGuid(),
                CustomVehicleTypeId = Guid.NewGuid(),
                FromSecond = 600,
                ToSecond = 3599,
                Amount = 14000,
                BlockDurationInSeconds = 1800,
                CreatedDate = new DateTime(2023,9,11),
                BlockNumber = 1
            },
            new TimeBlockFeeModel()
            {
                Id = Guid.NewGuid(),
                CustomVehicleTypeId = Guid.NewGuid(),
                FromSecond = 3600,
                ToSecond = 5399,
                Amount = 21000,
                BlockDurationInSeconds = 1800,
                CreatedDate = new DateTime(2023,9,11),
                BlockNumber = 2
            },
            new TimeBlockFeeModel()
            {
                Id = Guid.NewGuid(),
                CustomVehicleTypeId = Guid.NewGuid(),
                FromSecond = 5400,
                ToSecond = 7199,
                Amount = 28000,
                BlockDurationInSeconds = 1800,
                CreatedDate = new DateTime(2023,9,11),
                BlockNumber = 3
            }
        };
        private static TimeBlockFeeFormulaModel firstFormulaModel = new TimeBlockFeeFormulaModel()
        {
            Id = Guid.NewGuid(),
            CustomVehicleTypeId = Guid.NewGuid(),
            CreatedDate = new DateTime(2023, 9, 11),
            FromBlockNumber = 2,
            IntervalInSeconds = 0,
            Amount = 7000,
            ApplyDate = new DateTime(2023, 1, 1)
        }; private static TimeBlockFeeFormulaModel secondFormulaModel = new TimeBlockFeeFormulaModel()
        {
            Id = Guid.NewGuid(),
            CustomVehicleTypeId = Guid.NewGuid(),
            CreatedDate = new DateTime(2023, 9, 11),
            FromBlockNumber = 2,
            IntervalInSeconds = 1800,
            Amount = 7000,
            ApplyDate = new DateTime(2023, 1, 1)
        };

        public static IEnumerable<object[]> Data()
        {
            yield return new object[] { null!, firstFormulaModel, (long)100, (double)0 };
            yield return new object[] { firstTimeBlockFee, null!, (long)100, (double)0 };
            yield return new object[] { firstTimeBlockFee, firstFormulaModel, (long)100, (double)0 };
            yield return new object[] { firstTimeBlockFee, secondFormulaModel, (long)100, (double)0 };
            yield return new object[] { secondTimeBlockFee, secondFormulaModel, (long)350, (double)9000 };
            yield return new object[] { secondTimeBlockFee, secondFormulaModel, (long)1350, (double)7000 };
            yield return new object[] { thirdTimeBlockFee, secondFormulaModel, (long)1650, (double)14000 };
            yield return new object[] { thirdTimeBlockFee, secondFormulaModel, (long)10650, (double)42000 };
        }
        #endregion

        [Theory]
        [MemberData(nameof(Data))]
        public void GivenRequestIsValid_WhenFeeCalculationIsCalled_ThenReturnCorrectResult(List<TimeBlockFeeModel>? timeBlockFees, TimeBlockFeeFormulaModel timeBlockFeeFormula, long? duration, double? amount)
        {
            // Arrange

            // Act
            var result = FeeCalculationUtil.FeeCalculation(timeBlockFees, timeBlockFeeFormula, duration ?? 0);

            // Assert
            result.Should().Be(amount);
        }
    }
}
