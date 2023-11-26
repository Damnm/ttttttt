using AutoMapper;
using EPAY.ETC.Core.API.Core.Models.Barcode;
using EPAY.ETC.Core.API.Core.Models.ETCCheckOuts;
using EPAY.ETC.Core.API.Core.Models.Fusion;
using EPAY.ETC.Core.API.Core.Models.ManualBarrierControl;
using EPAY.ETC.Core.API.Core.Models.PrintLog;
using EPAY.ETC.Core.API.Core.Models.TransactionLog;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Models.Configs;
using EPAY.ETC.Core.Models.Devices;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.Fees;
using EPAY.ETC.Core.Models.Fees.PaymentStatusHistory;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.UI;
using EPAY.ETC.Core.Models.Utils;
using EPAY.ETC.Core.Publisher.Common.Options;
using System;
using System.Diagnostics.CodeAnalysis;
using CoreModel = EPAY.ETC.Core.Models.Fees;
using FeeModel = EPAY.ETC.Core.API.Core.Models.Fees.FeeModel;

#nullable disable
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
            CreateMap<ETC.Core.Models.VehicleInfoModel, VehicleModel>()
                .ForMember(e => e.Make, act => act.MapFrom(src => src.Make))
                .ForMember(e => e.Model, act => act.MapFrom(src => src.Model))
                .ForMember(e => e.PlateNumber, act => act.MapFrom(src => src.PlateNumber))
                .ForMember(e => e.PlateColor, act => act.MapFrom(src => src.PlateColour))
                .ForMember(e => e.VehicleType, act => act.MapFrom(src => src.VehicleType))
                .ForMember(e => e.Seat, act => act.MapFrom(src => src.Seat))
                .ForMember(e => e.Weight, act => act.MapFrom(src => src.Weight))
                .ReverseMap();
            CreateMap<ManualBarrierControlAddOrUpdateRequestModel, ManualBarrierControlModel>()
                .ForMember(e => e.CreatedDate, act => act.MapFrom(src => DateTime.Now))
                .ReverseMap();
            CreateMap<FusionAddRequestModel, FusionModel>()
                .ForMember(e => e.Id, act => act.MapFrom(src => src.Id ?? Guid.NewGuid()))
                .ReverseMap();
            CreateMap<PaymentStatusAddRequestModel, Core.Models.PaymentStatus.PaymentStatusModel>().ReverseMap();
            CreateMap<PaymentAddOrUpdateRequestModel, Core.Models.Payment.PaymentModel>()
                 .ForMember(e => e.Id, act => act.MapFrom(src => src.PaymentId.HasValue ? src.PaymentId : Guid.NewGuid()))
                 .ForMember(e => e.VehicleType, act => act.MapFrom(src => src.VehicleTypeId))
                 .ReverseMap();

            CreateMap<FusionUpdateRequestModel, FusionModel>().ReverseMap();
            CreateMap<PaymentStatusUpdateRequestModel, Core.Models.PaymentStatus.PaymentStatusModel>().ReverseMap();
            CreateMap<CoreModel.FeeModel, FeeModel>()
                .ForMember(e => e.Id, act => act.MapFrom(src => src.FeeId.HasValue ? src.FeeId : Guid.NewGuid()))
                .ForMember(e => e.LaneInId, act => act.MapFrom(src => src.LaneInVehicle != null ? src.LaneInVehicle.LaneInId : null))
                .ForMember(e => e.LaneInEpoch, act => act.MapFrom(src => src.LaneInVehicle != null ? (long?)src.LaneInVehicle.Epoch : null))
                .ForMember(e => e.LaneInDate, act => act.MapFrom(src => src.LaneInVehicle != null ? (DateTime?)src.LaneInVehicle.Epoch.ToSpecificDateTime("SE Asia Standard Time") : null))
                .ForMember(e => e.LaneOutId, act => act.MapFrom(src => src.LaneOutVehicle != null ? src.LaneOutVehicle.LaneOutId : null))
                .ForMember(e => e.LaneOutEpoch, act => act.MapFrom(src => src.LaneOutVehicle != null ? (long?)src.LaneOutVehicle.Epoch : null))
                .ForMember(e => e.LaneOutDate, act => act.MapFrom(src => src.LaneOutVehicle != null ? (DateTime?)src.LaneOutVehicle.Epoch.ToSpecificDateTime("SE Asia Standard Time") : null))
                .ForMember(e => e.RFID, act => act.MapFrom(src => src.Payment != null ? src.Payment.RFID : null))
                .ForMember(e => e.Make, act => act.MapFrom(src => src.Payment != null ? src.Payment.Make : null))
                .ForMember(e => e.Model, act => act.MapFrom(src => src.Payment != null ? src.Payment.Model : null))
                .ForMember(e => e.PlateNumber, act => act.MapFrom(src => src.Payment != null ? src.Payment.PlateNumber : null))
                .ForMember(e => e.CustomVehicleTypeId, act => act.MapFrom(src => src.Payment != null ? src.Payment.CustomVehicleTypeId : null))
                .ForMember(e => e.Amount, act => act.MapFrom(src => src.Payment != null ? src.Payment.Amount : 0))
                .ForMember(e => e.VehicleCategoryId, act => act.MapFrom(src => src.Payment != null ? src.Payment.VehicleCategoryId : null))
                .ForMember(e => e.TicketId, act => act.MapFrom(src => src.Payment != null ? src.Payment.TicketId : null))
                .ForMember(e => e.Duration, act => act.MapFrom(src => src.Payment != null ? src.Payment.Duration : 0))
                .ForMember(e => e.LaneInVehiclePhotoUrl, act => act.MapFrom(src => src.LaneInVehicle != null && src.LaneInVehicle.VehicleInfo != null ? src.LaneInVehicle.VehicleInfo.VehiclePhotoUrl : null))
                .ForMember(e => e.LaneInPlateNumberPhotoUrl, act => act.MapFrom(src => src.LaneInVehicle != null && src.LaneInVehicle.VehicleInfo != null ? src.LaneInVehicle.VehicleInfo.PlateNumberPhotoUrl : null))
                .ForMember(e => e.LaneOutVehiclePhotoUrl, act => act.MapFrom(src => src.LaneOutVehicle != null && src.LaneOutVehicle.VehicleInfo != null ? src.LaneOutVehicle.VehicleInfo.VehiclePhotoUrl : null))
                .ForMember(e => e.LaneOutPlateNumberPhotoUrl, act => act.MapFrom(src => src.LaneOutVehicle != null && src.LaneOutVehicle.VehicleInfo != null ? src.LaneOutVehicle.VehicleInfo.PlateNumberPhotoUrl : null))
                .ForMember(e => e.PlateColour, act => act.MapFrom(src => src.LaneOutVehicle != null && src.LaneOutVehicle.VehicleInfo != null ? src.LaneOutVehicle.VehicleInfo.PlateColour : (src.LaneInVehicle != null && src.LaneInVehicle.VehicleInfo != null ? src.LaneInVehicle.VehicleInfo.PlateColour : null)))
                .ForMember(e => e.ConfidenceScore, act => act.MapFrom(src => src.LaneOutVehicle != null && src.LaneOutVehicle.VehicleInfo != null ? (float?)src.LaneOutVehicle.VehicleInfo.ConfidenceScore : (src.LaneInVehicle != null && src.LaneInVehicle.VehicleInfo != null ? (float?)src.LaneInVehicle.VehicleInfo.ConfidenceScore : null)))
                .ForMember(e => e.Seat, act => act.MapFrom(src => src.LaneOutVehicle != null && src.LaneOutVehicle.VehicleInfo != null ? src.LaneOutVehicle.VehicleInfo.Seat : (src.LaneInVehicle != null && src.LaneInVehicle.VehicleInfo != null ? src.LaneInVehicle.VehicleInfo.Seat : null)))
                .ForMember(e => e.Weight, act => act.MapFrom(src => src.LaneOutVehicle != null && src.LaneOutVehicle.VehicleInfo != null ? src.LaneOutVehicle.VehicleInfo.Weight : (src.LaneInVehicle != null && src.LaneInVehicle.VehicleInfo != null ? src.LaneInVehicle.VehicleInfo.Weight : null)))
                .ForMember(e => e.TicketTypeId, act => act.MapFrom(src => src.Payment != null ? src.Payment.TicketTypeId : null))

                .ForPath(e => e.TicketType.Code, act => act.Ignore())
                .ForMember(e => e.CustomVehicleType, act => act.Ignore());
            CreateMap<FeeModel, CoreModel.FeeModel>()
                .ForMember(e => e.FeeId, act => act.MapFrom(src => src.Id))
                .ForPath(e => e.LaneInVehicle.LaneInId, act => act.MapFrom(src => src.LaneInId))
                .ForPath(e => e.LaneInVehicle.Epoch, act => act.MapFrom(src => src.LaneInEpoch))
                .ForPath(e => e.LaneOutVehicle.LaneOutId, act => act.MapFrom(src => src.LaneOutId))
                .ForPath(e => e.LaneOutVehicle.Epoch, act => act.MapFrom(src => src.LaneOutEpoch))
                .ForPath(e => e.Payment.RFID, act => act.MapFrom(src => src.RFID))
                .ForPath(e => e.Payment.Make, act => act.MapFrom(src => src.Make))
                .ForPath(e => e.Payment.TicketTypeId, act => act.MapFrom(src => src.TicketTypeId))
                .ForPath(e => e.Payment.Model, act => act.MapFrom(src => src.Model))
                .ForPath(e => e.Payment.PlateNumber, act => act.MapFrom(src => src.PlateNumber))
                .ForPath(e => e.Payment.CustomVehicleTypeId, act => act.MapFrom(src => src.CustomVehicleTypeId))
                .ForPath(e => e.Payment.CustomVehicleTypeCode, act => act.MapFrom(src => src.CustomVehicleType.Name.ToString()))
                .ForPath(e => e.Payment.Amount, act => act.MapFrom(src => src.Amount))
                .ForPath(e => e.Payment.VehicleCategoryId, act => act.MapFrom(src => src.VehicleCategoryId))
                .ForPath(e => e.Payment.TicketTypeId, act => act.MapFrom(src => src.TicketType.Code))
                .ForPath(e => e.Payment.TicketId, act => act.MapFrom(src => src.TicketId))
                .ForPath(e => e.Payment.Duration, act => act.MapFrom(src => src.Duration))
                .ForPath(e => e.LaneInVehicle.VehicleInfo.VehiclePhotoUrl, act => act.MapFrom(src => src.LaneInVehiclePhotoUrl))
                .ForPath(e => e.LaneInVehicle.VehicleInfo.PlateNumberPhotoUrl, act => act.MapFrom(src => src.LaneInPlateNumberPhotoUrl))
                .ForPath(e => e.LaneInVehicle.VehicleInfo.PlateNumber, act => act.MapFrom(src => src.PlateNumber))
                .ForPath(e => e.LaneOutVehicle.VehicleInfo.VehiclePhotoUrl, act => act.MapFrom(src => src.LaneOutVehiclePhotoUrl))
                .ForPath(e => e.LaneOutVehicle.VehicleInfo.PlateNumberPhotoUrl, act => act.MapFrom(src => src.LaneOutPlateNumberPhotoUrl))
                .ForPath(e => e.LaneOutVehicle.VehicleInfo.PlateNumber, act => act.MapFrom(src => src.PlateNumber))
                .ForPath(e => e.CustomVehicleType, act => act.MapFrom(src => src.CustomVehicleType != null ? src.CustomVehicleType.Name : CustomVehicleTypeEnum.Type1));

            CreateMap<ETCCheckoutAddUpdateRequestModel, ETCCheckoutDataModel>().ReverseMap();
            CreateMap<ETCCheckoutResponseModel, ETCCheckoutDataModel>();
            CreateMap<ETCCheckoutDataModel, ETCCheckoutResponseModel>()
                .ForPath(e => e.Payment.PaymentId, act => act.MapFrom(src => src.PaymentId))
                .ForPath(e => e.Payment.FeeId, act => act.MapFrom(src => src.Payment.FeeId))
                .ForPath(e => e.Payment.CreatedDate, act => act.MapFrom(src => src.Payment.CreatedDate))
                .ForPath(e => e.Payment.LaneInId, act => act.MapFrom(src => src.Payment.LaneInId))
                .ForPath(e => e.Payment.LaneOutId, act => act.MapFrom(src => src.Payment.LaneOutId))
                .ForPath(e => e.Payment.Duration, act => act.MapFrom(src => src.Payment.Duration))
                .ForPath(e => e.Payment.RFID, act => act.MapFrom(src => src.Payment.RFID))
                .ForPath(e => e.Payment.Make, act => act.MapFrom(src => src.Payment.Make))
                .ForPath(e => e.Payment.Model, act => act.MapFrom(src => src.Payment.Model))
                .ForPath(e => e.Payment.PlateNumber, act => act.MapFrom(src => src.Payment.PlateNumber))
                .ForPath(e => e.Payment.VehicleTypeId, act => act.MapFrom(src => src.Payment.VehicleType))
                .ForPath(e => e.Payment.Amount, act => act.MapFrom(src => src.Payment.Amount))
                .ForPath(e => e.Payment.Currency, act => act.MapFrom(src => src.Payment.PaymentStatuses.FirstOrDefault().Currency))
                .ForPath(e => e.Payment.TicketTypeId, act => act.MapFrom(src => src.Payment.Fee.TicketTypeId))
                .ForPath(e => e.Payment.TicketId, act => act.MapFrom(src => src.Payment.Fee.TicketId))
                .ForPath(e => e.Payment.VehicleCategoryId, act => act.MapFrom(src => src.Payment.Fee.VehicleCategoryId))
                .ForPath(e => e.Payment.CustomVehicleTypeId, act => act.MapFrom(src => src.Payment.CustomVehicleTypeId))
                .ForPath(e => e.Payment.CheckInTime, act => act.MapFrom(src => src.Payment.Fee.LaneInEpoch))
                .ForPath(e => e.Payment.CheckOutTime, act => act.MapFrom(src => src.Payment.Fee.LaneOutEpoch))
                .ForPath(e => e.Payment.ObjectId, act => act.MapFrom(src => src.Payment.Fee.ObjectId));

            CreateMap<PaymentStatusUIRequestModel, CoreModel.PaymenStatusResponseModel>()
                .ForPath(e => e.PaymentStatus.Amount, act => act.MapFrom(src => src.Amount))
                .ForPath(e => e.PaymentStatus.PaymentId, act => act.MapFrom(src => src.PaymentId))
                .ForPath(e => e.PaymentStatus.PaymentMethod, act => act.MapFrom(src => src.PaymentMethod))
                .ForPath(e => e.PaymentStatus.Status, act => act.MapFrom(src => src.Status));

            CreateMap<PublisherConfigurationOption, PublisherOptions>().ReverseMap();
            CreateMap<BarcodeAddOrUpdateRequestModel, BarcodeModel>().ReverseMap();
            CreateMap<Core.Models.PaymentStatus.PaymentStatusModel, PaymentStatusHistoryModel>()
                 .ForPath(e => e.Time, act => act.MapFrom(src => DateTime.SpecifyKind(src.PaymentDate, DateTimeKind.Utc).ToUnixTime().ToAsiaTimeString("HH:mm:ss")))
                 .ForPath(e => e.Reason, act => act.MapFrom(src => src.Reason));

            CreateMap<EmployeeAutoLoginRequest, AuthenticatedEmployeeResponseModel>().ReverseMap();
            CreateMap<EmployeeLoginRequest, AuthenticatedEmployeeResponseModel>().ReverseMap();
            CreateMap<Core.Models.Devices.RFID.RFIDModel, LaneInVehicleModel>()
                .ForPath(e => e.Epoch, act => act.MapFrom(src => src.Epoch))
                .ForPath(e => e.RFID, act => act.MapFrom(src => src.TagId))
                .ForPath(e => e.Device.MacAddr, act => act.MapFrom(src => src.RFIDDeviceInfo.MacAddr))
                .ForPath(e => e.Device.IpAddr, act => act.MapFrom(src => src.RFIDDeviceInfo.IPAddr))
                .ForPath(e => e.VehicleInfo.Make, act => act.MapFrom(src => src.VehicleInfo.Make))
                .ForPath(e => e.VehicleInfo.Model, act => act.MapFrom(src => src.VehicleInfo.Model))
                .ForPath(e => e.VehicleInfo.PlateNumber, act => act.MapFrom(src => src.VehicleInfo.PlateNumber))
                .ForPath(e => e.VehicleInfo.VehiclePhotoUrl, act => act.MapFrom(src => src.VehicleInfo.VehiclePhotoUrl))
                .ForPath(e => e.VehicleInfo.VehicleType, act => act.MapFrom(src => src.VehicleInfo.VehicleType))
                .ForPath(e => e.VehicleInfo.Seat, act => act.MapFrom(src => src.VehicleInfo.Seat))
                .ForPath(e => e.VehicleInfo.Weight, act => act.MapFrom(src => src.VehicleInfo.Weight))
                .ForPath(e => e.VehicleInfo.PlateNumberPhotoUrl, act => act.MapFrom(src => src.VehicleInfo.PlateNumberPhotoUrl));

            CreateMap<ANPRCameraModel, LaneInVehicleModel>()
                .ForPath(e => e.LaneInId, act => act.MapFrom(src => src.LaneInId))
                .ForPath(e => e.Epoch, act => act.MapFrom(src => src.CheckpointTimeEpoch))
                .ForPath(e => e.Device.MacAddr, act => act.MapFrom(src => src.MacAddr))
                .ForPath(e => e.Device.IpAddr, act => act.MapFrom(src => src.IpAddr))
                .ForPath(e => e.VehicleInfo.Make, act => act.MapFrom(src => src.VehicleInfo.Make))
                .ForPath(e => e.VehicleInfo.Model, act => act.MapFrom(src => src.VehicleInfo.Model))
                .ForPath(e => e.VehicleInfo.PlateNumber, act => act.MapFrom(src => !string.IsNullOrEmpty(src.VehicleInfo.PlateNumber) ? src.VehicleInfo.PlateNumber : src.VehicleInfo.RearPlateNumber))
                .ForPath(e => e.VehicleInfo.PlateColour, act => act.MapFrom(src => !string.IsNullOrEmpty(src.VehicleInfo.PlateColour) ? src.VehicleInfo.PlateColour : src.VehicleInfo.RearPlateColour))
                .ForPath(e => e.VehicleInfo.VehicleColour, act => act.MapFrom(src => src.VehicleInfo.VehicleColour))
                .ForPath(e => e.VehicleInfo.VehicleType, act => act.MapFrom(src => src.VehicleInfo.VehicleType))
                .ForPath(e => e.VehicleInfo.Seat, act => act.MapFrom(src => src.VehicleInfo.Seat))
                .ForPath(e => e.VehicleInfo.Weight, act => act.MapFrom(src => src.VehicleInfo.Weight))
                .ForPath(e => e.VehicleInfo.VehiclePhotoUrl, act => act.MapFrom(src => !string.IsNullOrEmpty(src.VehicleInfo.VehiclePhotoUrl) ? src.VehicleInfo.VehiclePhotoUrl : src.VehicleInfo.VehicleRearPhotoUrl))
                .ForPath(e => e.VehicleInfo.PlateNumberPhotoUrl, act => act.MapFrom(src => !string.IsNullOrEmpty(src.VehicleInfo.PlateNumberPhotoUrl) ? src.VehicleInfo.PlateNumberPhotoUrl : src.VehicleInfo.PlateNumberRearPhotoUrl))
                .ForPath(e => e.VehicleInfo.ConfidenceScore, act => act.MapFrom(src => src.VehicleInfo.ConfidenceScore));

            CreateMap<ANPRCameraModel, LaneInCameraDataModel>()
                .ForPath(e => e.LaneId, act => act.MapFrom(src => src.LaneInId))
                .ForPath(e => e.Epoch, act => act.MapFrom(src => src.CheckpointTimeEpoch))
                .ForPath(e => e.CameraDeviceInfo.MacAddr, act => act.MapFrom(src => src.MacAddr))
                .ForPath(e => e.CameraDeviceInfo.IpAddr, act => act.MapFrom(src => src.IpAddr))
                .ForPath(e => e.VehicleInfo.Make, act => act.MapFrom(src => src.VehicleInfo.Make))
                .ForPath(e => e.VehicleInfo.Model, act => act.MapFrom(src => src.VehicleInfo.Model))
                .ForPath(e => e.VehicleInfo.PlateNumber, act => act.MapFrom(src => !string.IsNullOrEmpty(src.VehicleInfo.PlateNumber) ? src.VehicleInfo.PlateNumber : src.VehicleInfo.RearPlateNumber))
                .ForPath(e => e.VehicleInfo.PlateColour, act => act.MapFrom(src => !string.IsNullOrEmpty(src.VehicleInfo.PlateColour) ? src.VehicleInfo.PlateColour : src.VehicleInfo.RearPlateColour))
                .ForPath(e => e.VehicleInfo.VehicleColour, act => act.MapFrom(src => src.VehicleInfo.VehicleColour))
                .ForPath(e => e.VehicleInfo.VehicleType, act => act.MapFrom(src => src.VehicleInfo.VehicleType))
                .ForPath(e => e.VehicleInfo.Seat, act => act.MapFrom(src => src.VehicleInfo.Seat))
                .ForPath(e => e.VehicleInfo.Weight, act => act.MapFrom(src => src.VehicleInfo.Weight))
                .ForPath(e => e.VehicleInfo.VehiclePhotoUrl, act => act.MapFrom(src => !string.IsNullOrEmpty(src.VehicleInfo.VehiclePhotoUrl) ? src.VehicleInfo.VehiclePhotoUrl : src.VehicleInfo.VehicleRearPhotoUrl))
                .ForPath(e => e.VehicleInfo.PlateNumberPhotoUrl, act => act.MapFrom(src => !string.IsNullOrEmpty(src.VehicleInfo.PlateNumberPhotoUrl) ? src.VehicleInfo.PlateNumberPhotoUrl : src.VehicleInfo.PlateNumberRearPhotoUrl))
                .ForPath(e => e.VehicleInfo.ConfidenceScore, act => act.MapFrom(src => src.VehicleInfo.ConfidenceScore));

            CreateMap<LaneInCameraTransactionLogRequest, LaneInCameraTransactionLog>()
               .ReverseMap();

            CreateMap<LaneInRFIDTransactionLogRequestModel, LaneInRFIDTransactionLog>()
               .ReverseMap();

            CreateMap<LaneInVehicleModel, LaneInRFIDTransactionLog>()
                .ForMember(e => e.Epoch, act => act.MapFrom(src => src.Epoch))
                .ForMember(e => e.RFID, act => act.MapFrom(src => src.RFID))
                .ForMember(e => e.RFIDReaderMacAddr, act => act.MapFrom(src => src.Device.MacAddr))
                .ForMember(e => e.RFIDReaderIPAddr, act => act.MapFrom(src => src.Device.IpAddr))
                .ForMember(e => e.LaneInId, act => act.MapFrom(src => src.LaneInId))
                .ForMember(e => e.Make, act => act.MapFrom(src => src.VehicleInfo.Make))
                .ForMember(e => e.Model, act => act.MapFrom(src => src.VehicleInfo.Model))
                .ForMember(e => e.PlateNumber, act => act.MapFrom(src => src.VehicleInfo.PlateNumber))
                .ForMember(e => e.PlateColour, act => act.MapFrom(src => src.VehicleInfo.PlateColour))
                .ForMember(e => e.VehicleType, act => act.MapFrom(src => src.VehicleInfo.VehicleType))
                .ForMember(e => e.Seat, act => act.MapFrom(src => src.VehicleInfo.Seat))
                .ForMember(e => e.Weight, act => act.MapFrom(src => src.VehicleInfo.Weight))
                .ForMember(e => e.PlateNumberPhotoUrl, act => act.MapFrom(src => src.VehicleInfo.PlateNumberPhotoUrl))
                .ForMember(e => e.VehiclePhotoUrl, act => act.MapFrom(src => src.VehicleInfo.VehiclePhotoUrl))
                .ForMember(e => e.ConfidenceScore, act => act.MapFrom(src => src.VehicleInfo.ConfidenceScore))
                .ForMember(e => e.RearPlateNumber, act => act.MapFrom(src => src.VehicleInfo.RearPlateNumber))
                .ForMember(e => e.RearPlateColour, act => act.MapFrom(src => src.VehicleInfo.RearPlateColour))
                .ForMember(e => e.VehicleRearPhotoUrl, act => act.MapFrom(src => src.VehicleInfo.VehicleRearPhotoUrl))
                .ForMember(e => e.PlateNumberRearPhotoUrl, act => act.MapFrom(src => src.VehicleInfo.PlateNumberRearPhotoUrl))
                .ForMember(e => e.CreatedDate, act => act.MapFrom(src => DateTime.Now));

            CreateMap<LaneInRFIDTransactionLog, LaneInVehicleModel>()
                .ForMember(e => e.Epoch, act => act.MapFrom(src => src.Epoch))
                .ForMember(e => e.RFID, act => act.MapFrom(src => src.RFID))
                .ForMember(e => e.LaneInId, act => act.MapFrom(src => src.LaneInId))
                .ForPath(e => e.Device.MacAddr, act => act.MapFrom(src => src.RFIDReaderMacAddr))
                .ForPath(e => e.Device.IpAddr, act => act.MapFrom(src => src.RFIDReaderIPAddr))
                .ForPath(e => e.VehicleInfo.Make, act => act.MapFrom(src => src.Make))
                .ForPath(e => e.VehicleInfo.Model, act => act.MapFrom(src => src.Model))
                .ForPath(e => e.VehicleInfo.PlateNumber, act => act.MapFrom(src => src.PlateNumber))
                .ForPath(e => e.VehicleInfo.PlateColour, act => act.MapFrom(src => src.PlateColour))
                .ForPath(e => e.VehicleInfo.VehicleType, act => act.MapFrom(src => src.VehicleType))
                .ForPath(e => e.VehicleInfo.Seat, act => act.MapFrom(src => src.Seat))
                .ForPath(e => e.VehicleInfo.Weight, act => act.MapFrom(src => src.Weight))
                .ForPath(e => e.VehicleInfo.PlateNumberPhotoUrl, act => act.MapFrom(src => src.PlateNumberPhotoUrl))
                .ForPath(e => e.VehicleInfo.VehiclePhotoUrl, act => act.MapFrom(src => src.VehiclePhotoUrl))
                .ForPath(e => e.VehicleInfo.ConfidenceScore, act => act.MapFrom(src => src.ConfidenceScore))
                .ForPath(e => e.VehicleInfo.RearPlateNumber, act => act.MapFrom(src => src.RearPlateNumber))
                .ForPath(e => e.VehicleInfo.RearPlateColour, act => act.MapFrom(src => src.RearPlateColour))
                .ForPath(e => e.VehicleInfo.VehicleRearPhotoUrl, act => act.MapFrom(src => src.VehicleRearPhotoUrl))
                .ForPath(e => e.VehicleInfo.PlateNumberRearPhotoUrl, act => act.MapFrom(src => src.PlateNumberRearPhotoUrl));

            CreateMap<PrintLogRequestModel, PrintLogModel>()
                 .ForMember(e => e.Id, act => act.MapFrom(src => src.PrintLogId))
               .ReverseMap();
        }
    }
}
