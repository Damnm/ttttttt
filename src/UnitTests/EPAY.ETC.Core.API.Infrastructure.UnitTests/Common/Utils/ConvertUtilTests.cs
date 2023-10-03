using EPAY.ETC.Core.API.Core.Utils;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Common.Utils
{
    public class ConvertUtilTests
    {
        #region DocTienBangChuV2
        [Fact]
        public void GivenValidRequestInputNegative_WhenDocTienBangChuV2IsCalled_ReturnCorrectResult()
        {
            // Arrange
            var resultNegative = "Âm một đồng";
            var money = -1;
            // Act
            var sut = ConvertUtil.DocTienBangChuV2(money);
            //Assert
            Assert.Equal(resultNegative, sut);
        }

        [Fact]
        public void GivenValidRequestInputZero_WhenDocTienBangChuV2IsCalled_ReturnCorrectResult()
        {
            // Arrange
            var resultNegative = "Không đồng";
            var money = 0;
            // Act
            var sut = ConvertUtil.DocTienBangChuV2(money);
            //Assert
            Assert.Equal(resultNegative, sut);
        }

        [Fact]
        public void GivenValidRequestInputMax_WhenDocTienBangChuV2IsCalled_ReturnCorrectResult()
        {
            // Arrange
            var resultNegative = "Chín triệu tỷ đồng";
            var money = 8999999999999999L + 1;
            // Act
            var sut = ConvertUtil.DocTienBangChuV2(money);
            //Assert
            Assert.Equal(resultNegative, sut);
        }

        [Theory]
        [InlineData(14567, true, "Mười bốn nghìn năm trăm sáu mươi bảy đồng")]
        [InlineData(6048754307, true, "Sáu tỷ không trăm bốn mươi tám triệu bảy trăm năm mươi bốn nghìn ba trăm lẻ bảy đồng")]
        [InlineData(400000000000, false, "Bốn trăm tỷ")]
        [InlineData(5400912600001, true, "Năm nghìn bốn trăm tỷ chín trăm mười hai triệu sáu trăm nghìn không trăm lẻ một đồng")]
        [InlineData(12, true, "Mười hai đồng")]
        [InlineData(124, true, "Một trăm hai mươi bốn đồng")]
        [InlineData(109, true, "Một trăm lẻ chín đồng")]
        [InlineData(59876, false, "Năm mươi chín nghìn tám trăm bảy mươi sáu")]
        [InlineData(125678, true, "Một trăm hai mươi lăm nghìn sáu trăm bảy mươi tám đồng")]
        [InlineData(1256782, false, "Một triệu hai trăm năm mươi sáu nghìn bảy trăm tám mươi hai")]
        [InlineData(12356782, true, "Mười hai triệu ba trăm năm mươi sáu nghìn bảy trăm tám mươi hai đồng")]
        [InlineData(142356782, true, "Một trăm bốn mươi hai triệu ba trăm năm mươi sáu nghìn bảy trăm tám mươi hai đồng")]
        public void GivenValidRequestRandom_WhenDocTienBangChuV2IsCalled_ReturnCorrectResult(long money, bool subffix, string result)
        {
            // Arrange

            // Act
            var sut = ConvertUtil.DocTienBangChuV2(money, subffix);
            //Assert
            Assert.Equal(result, sut);
        }
        #endregion
    }
}