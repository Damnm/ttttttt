using EPAY.ETC.Core.API.Core.Models.Fees;
using EPAY.ETC.Core.API.Core.Models.Fusion;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.UnitTests.Common;
using FluentAssertions;
using CoreModel = EPAY.ETC.Core.Models.Fees;

#nullable disable
namespace EPAY.ETC.Core.API.UnitTests.Profiles
{
    public class MappingProfileTests : AutoMapperTestBase
    {
        #region Vehicle
        [Fact]
        public void GivenValidVehicleRequestModel_WhenAutoMapperIsCalled_ThenCorrectResult()
        {
            // Arrange
            var vehicleType = new VehicleRequestModel()
            {
                PlateNumber = "Some plate number",
                RFID = "Some RFID"
            };

            // Act            
            var result = _mapper.Map<VehicleModel>(vehicleType);

            // Assert
            result.Should().NotBeNull();
            result.PlateNumber.Should().Be(vehicleType.PlateNumber);
            result.RFID.Should().Be(vehicleType.RFID);
            result.GetType().Should().Be(typeof(VehicleModel));
        }

        [Fact]
        public void GivenVehicleTypeRequestModelIsNull_WhenAutoMapperIsCalled_ThenEmptyResult()
        {
            // Arrange
            VehicleRequestModel vehicleType = null;

            // Act
            var result = _mapper.Map<VehicleModel>(vehicleType);

            // Assert
            result.Should().BeNull();
        }
        #endregion

        #region Fusion
        [Fact]
        public void GivenValidFusionAddRequestModel_WhenAutoMapperIsCalled_ThenCorrectResult()
        {
            // Arrange
            var vehicleType = new FusionAddRequestModel()
            {
                Loop1 = true,
                Epoch = 123456,
                
            };

            // Act            
            var result = _mapper.Map<FusionModel>(vehicleType);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(Guid.NewGuid());
            result.Loop1.Should().Be(vehicleType.Loop1);
            result.Loop2.Should().Be(vehicleType.Loop2);
            result.Epoch.Should().Be(vehicleType.Epoch);
            result.GetType().Should().Be(typeof(FusionModel));
        }

        [Fact]
        public void GivenFusionAddRequestModelIsNull_WhenAutoMapperIsCalled_ThenEmptyResult()
        {
            // Arrange
            FusionAddRequestModel vehicleType = null;

            // Act
            var result = _mapper.Map<FusionModel>(vehicleType);

            // Assert
            result.Should().BeNull();
        }


        [Fact]
        public void GivenValidFusionUpdateRequestModel_WhenAutoMapperIsCalled_ThenCorrectResult()
        {
            // Arrange
            var vehicleType = new FusionUpdateRequestModel()
            {
                Loop1 = true,
                Epoch = 123456
            };

            // Act            
            var result = _mapper.Map<FusionModel>(vehicleType);

            // Assert
            result.Should().NotBeNull();
            result.Loop1.Should().Be(vehicleType.Loop1);
            result.Loop2.Should().Be(vehicleType.Loop2);
            result.Epoch.Should().Be(vehicleType.Epoch);
            result.GetType().Should().Be(typeof(FusionModel));
        }

        [Fact]
        public void GivenFusionUpdateRequestModelIsNull_WhenAutoMapperIsCalled_ThenEmptyResult()
        {
            // Arrange
            FusionUpdateRequestModel vehicleType = null;

            // Act
            var result = _mapper.Map<FusionModel>(vehicleType);

            // Assert
            result.Should().BeNull();
        }
        #endregion

        #region FeeModel in core models
        [Fact]
        public void GivenValidCoreFeeModel_WhenAutoMapperIsCalled_ThenCorrectResult()
        {
            // Arrange
            var vehicleType = new CoreModel.FeeModel()
            {
                ObjectId = Guid.NewGuid(),
                Payment = new CoreModel.PaymentModel()
                {
                    RFID = "Some RFID",
                    PlateNumber = "Some plate number"
                }
            };

            // Act            
            var result = _mapper.Map<FeeModel>(vehicleType);

            // Assert
            result.Should().NotBeNull();
            result.ObjectId.Should().Be(vehicleType.ObjectId);
            result.PlateNumber.Should().Be(vehicleType.Payment?.PlateNumber);
            result.RFID.Should().Be(vehicleType.Payment?.RFID);
            result.GetType().Should().Be(typeof(FeeModel));
        }

        [Fact]
        public void GivenCoreFeeModelIsNull_WhenAutoMapperIsCalled_ThenEmptyResult()
        {
            // Arrange
            CoreModel.FeeModel vehicleType = null;

            // Act
            var result = _mapper.Map<FeeModel>(vehicleType);

            // Assert
            result.Should().BeNull();
        }
        #endregion
    }
}
