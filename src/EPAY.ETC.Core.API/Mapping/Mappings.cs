using AutoMapper;
using EPAY.ETC.Core.API.Core.Models.Common;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Core.Extensions;

namespace EPAY.ETC.Core.API.Mapping
{
    public class Mappings: Profile
    {
        public Mappings()
        {
          

            CreateMap<VehicleRequestModel, VehicleModel>()               
                .ForMember(e => e.RFID, act => act.MapFrom(src => src.RFID))
                .ForMember(e => e.PlateNumber, act => act.MapFrom(src => src.PlateNumber))
                .ForMember(e => e.PlateColor, act => act.MapFrom(src => src.PlateColor))
                .ForMember(e => e.Seat, act => act.MapFrom(src => src.Seat))
                .ForMember(e => e.Make, act => act.MapFrom(src => src.Make))
                .ForMember(e => e.Weight, act => act.MapFrom(src => src.Weight))
                .ForMember(e => e.VehicleType, act => act.MapFrom(src => src.VehicleType));
        }
    }
}
