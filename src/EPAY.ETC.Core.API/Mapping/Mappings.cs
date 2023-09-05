using AutoMapper;
using EPAY.ETC.Core.API.Core.Models.Common;
using EPAY.ETC.Core.API.Core.Models.Fusion;
using EPAY.ETC.Core.API.Core.Models.Vehicle;

namespace EPAY.ETC.Core.API.Mapping
{
    public class Mappings: Profile
    {
        public Mappings()
        {
            CreateMap<VehicleRequestModel, VehicleModel>()
                .ForMember(e => e.Id, act => act.MapFrom(src => src.Id))
                .ReverseMap();
            CreateMap<FusionRequestModel, FusionModel>()
                .ForMember(e => e.Id, act => act.MapFrom(src => src.ObjectId))
                .ReverseMap();
        }
    }
}
