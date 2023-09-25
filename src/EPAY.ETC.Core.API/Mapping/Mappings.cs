using AutoMapper;
using EPAY.ETC.Core.API.Core.Models.Fees;
using EPAY.ETC.Core.API.Core.Models.Fusion;
using EPAY.ETC.Core.API.Core.Models.PaymentStatus;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Core.Utils;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.UI;
using System.Diagnostics.CodeAnalysis;
using CoreModel = EPAY.ETC.Core.Models.Fees;

namespace EPAY.ETC.Core.API.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Mappings : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public Mappings()
        {
            CreateMap<VehicleRequestModel, VehicleModel>()
                .ForMember(e => e.CreatedDate, act => act.MapFrom(src => DateTime.Now))
                .ReverseMap();
            CreateMap<FusionAddRequestModel, FusionModel>().ReverseMap();
            CreateMap<PaymentStatusAddRequestModel, Core.Models.PaymentStatus.PaymentStatusModel>().ReverseMap();
            CreateMap<PaymentAddOrUpdateRequestModel, Core.Models.Payment.PaymentModel>().ReverseMap();
            CreateMap<FusionUpdateRequestModel, FusionModel>().ReverseMap();
            CreateMap<PaymentStatusUpdateRequestModel, Core.Models.PaymentStatus.PaymentStatusModel>().ReverseMap();
            CreateMap<PaymentAddOrUpdateRequestModel, Core.Models.Payment.PaymentModel>().ReverseMap();
            CreateMap<CoreModel.FeeModel, FeeModel>()
                .ForMember(e => e.Id, act => act.MapFrom(src => src.FeeId.HasValue ? src.FeeId : Guid.NewGuid()))
                .ForMember(e => e.LaneInId, act => act.MapFrom(src => src.LaneInVehicle != null ? src.LaneInVehicle.LaneInId : null))
                .ForMember(e => e.LaneInEpoch, act => act.MapFrom(src => src.LaneInVehicle != null ? (long?)src.LaneInVehicle.Epoch : null))
                .ForMember(e => e.LaneInDate, act => act.MapFrom(src => src.LaneInVehicle != null ? (DateTime?)src.LaneInVehicle.Epoch.FromUnixTime() : null))
                .ForMember(e => e.LaneOutId, act => act.MapFrom(src => src.LaneOutVehicle != null ? src.LaneOutVehicle.LaneOutId : null))
                .ForMember(e => e.LaneOutEpoch, act => act.MapFrom(src => src.LaneOutVehicle != null ? (long?)src.LaneOutVehicle.Epoch : null))
                .ForMember(e => e.LaneOutDate, act => act.MapFrom(src => src.LaneOutVehicle != null ? (DateTime?)src.LaneOutVehicle.Epoch.FromUnixTime() : null))
                .ForMember(e => e.RFID, act => act.MapFrom(src => src.Payment != null ? src.Payment.RFID : null))
                .ForMember(e => e.Make, act => act.MapFrom(src => src.Payment != null ? src.Payment.Make : null))
                .ForMember(e => e.Model, act => act.MapFrom(src => src.Payment != null ? src.Payment.Model : null))
                .ForMember(e => e.PlateNumber, act => act.MapFrom(src => src.Payment != null ? src.Payment.PlateNumber : null))
                .ForMember(e => e.CustomVehicleTypeId, act => act.MapFrom(src => src.Payment != null ? src.Payment.CustomVehicleTypeId : null))
                .ForMember(e => e.Amount, act => act.MapFrom(src => src.Payment != null ? src.Payment.Amount : 0))
                .ForMember(e => e.VehicleCategoryId, act => act.MapFrom(src => src.Payment != null ? src.Payment.VehicleCategoryId : null))
                .ForMember(e => e.TicketTypeId, act => act.MapFrom(src => src.Payment != null ? src.Payment.TicketTypeId : null))
                .ForMember(e => e.TicketId, act => act.MapFrom(src => src.Payment != null ? src.Payment.TicketId : null))
                .ForMember(e => e.Duration, act => act.MapFrom(src => src.Payment != null ? src.Payment.Duration : 0))
                .ForMember(e => e.LaneInVehiclePhotoUrl, act => act.MapFrom(src => src.LaneInVehicle != null && src.LaneInVehicle.VehicleInfo != null ? src.LaneInVehicle.VehicleInfo.VehiclePhotoUrl : null))
                .ForMember(e => e.LaneInPlateNumberPhotoUrl, act => act.MapFrom(src => src.LaneInVehicle != null && src.LaneInVehicle.VehicleInfo != null ? src.LaneInVehicle.VehicleInfo.PlateNumberPhotoUrl : null))
                .ForMember(e => e.LaneOutVehiclePhotoUrl, act => act.MapFrom(src => src.LaneOutVehicle != null && src.LaneOutVehicle.VehicleInfo != null ? src.LaneOutVehicle.VehicleInfo.VehiclePhotoUrl : null))
                .ForMember(e => e.LaneOutPlateNumberPhotoUrl, act => act.MapFrom(src => src.LaneOutVehicle != null && src.LaneOutVehicle.VehicleInfo != null ? src.LaneOutVehicle.VehicleInfo.PlateNumberPhotoUrl : null))
                .ForMember(e => e.PlateColour, act => act.MapFrom(src => src.LaneOutVehicle != null && src.LaneOutVehicle.VehicleInfo != null ? src.LaneOutVehicle.VehicleInfo.PlateColour : (src.LaneInVehicle != null && src.LaneInVehicle.VehicleInfo != null ? src.LaneInVehicle.VehicleInfo.PlateColour : null)))
                .ForMember(e => e.ConfidenceScore, act => act.MapFrom(src => src.LaneOutVehicle != null && src.LaneOutVehicle.VehicleInfo != null ? (float?)src.LaneOutVehicle.VehicleInfo.ConfidenceScore : (src.LaneInVehicle != null && src.LaneInVehicle.VehicleInfo != null ? (float?)src.LaneInVehicle.VehicleInfo.ConfidenceScore : null)))
                .ForMember(e => e.Seat, act => act.MapFrom(src => src.LaneOutVehicle != null && src.LaneOutVehicle.VehicleInfo != null ? src.LaneOutVehicle.VehicleInfo.Seat : (src.LaneInVehicle != null && src.LaneInVehicle.VehicleInfo != null ? src.LaneInVehicle.VehicleInfo.Seat : null)))
                .ForMember(e => e.Weight, act => act.MapFrom(src => src.LaneOutVehicle != null && src.LaneOutVehicle.VehicleInfo != null ? src.LaneOutVehicle.VehicleInfo.Weight : (src.LaneInVehicle != null && src.LaneInVehicle.VehicleInfo != null ? src.LaneInVehicle.VehicleInfo.Weight : null)));
            CreateMap<FeeModel, CoreModel.FeeModel>()
                .ForMember(e => e.FeeId, act => act.MapFrom(src => src.Id))
                .ForPath(e => e.LaneInVehicle.LaneInId, act => act.MapFrom(src => src.LaneInId))
                .ForPath(e => e.LaneInVehicle.Epoch, act => act.MapFrom(src => src.LaneInEpoch))
                .ForPath(e => e.LaneOutVehicle.LaneOutId, act => act.MapFrom(src => src.LaneOutId))
                .ForPath(e => e.LaneOutVehicle.Epoch, act => act.MapFrom(src => src.LaneOutEpoch))
                .ForPath(e => e.Payment.RFID, act => act.MapFrom(src => src.RFID))
                .ForPath(e => e.Payment.Make, act => act.MapFrom(src => src.Make))
                .ForPath(e => e.Payment.Model, act => act.MapFrom(src => src.Model))
                .ForPath(e => e.Payment.PlateNumber, act => act.MapFrom(src => src.PlateNumber))
                .ForPath(e => e.Payment.CustomVehicleTypeId, act => act.MapFrom(src => src.CustomVehicleTypeId))
                .ForPath(e => e.Payment.CustomVehicleTypeCode, act => act.MapFrom(src => src.CustomVehicleType.Name.ToString()))
                .ForPath(e => e.Payment.Amount, act => act.MapFrom(src => src.Amount))
                .ForPath(e => e.Payment.VehicleCategoryId, act => act.MapFrom(src => src.VehicleCategoryId))
                .ForPath(e => e.Payment.TicketTypeId, act => act.MapFrom(src => src.TicketTypeId))
                .ForPath(e => e.Payment.TicketId, act => act.MapFrom(src => src.TicketId))
                .ForPath(e => e.Payment.Duration, act => act.MapFrom(src => src.Duration))
                .ForPath(e => e.LaneInVehicle.VehicleInfo.VehiclePhotoUrl, act => act.MapFrom(src => src.LaneInVehiclePhotoUrl))
                .ForPath(e => e.LaneInVehicle.VehicleInfo.PlateNumberPhotoUrl, act => act.MapFrom(src => src.LaneInPlateNumberPhotoUrl))
                .ForPath(e => e.LaneInVehicle.VehicleInfo.PlateNumber, act => act.MapFrom(src => src.PlateNumber))
                .ForPath(e => e.LaneOutVehicle.VehicleInfo.VehiclePhotoUrl, act => act.MapFrom(src => src.LaneOutVehiclePhotoUrl))
                .ForPath(e => e.LaneOutVehicle.VehicleInfo.PlateNumberPhotoUrl, act => act.MapFrom(src => src.LaneOutPlateNumberPhotoUrl))
                .ForPath(e => e.LaneOutVehicle.VehicleInfo.PlateNumber, act => act.MapFrom(src => src.PlateNumber));
        }
    }
}
