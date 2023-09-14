using AutoMapper;
using EPAY.ETC.Core.API.Core.Models.Fusion;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using System.Diagnostics.CodeAnalysis;

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
                .ForMember(e => e.Id, act => act.MapFrom(src => Guid.NewGuid()))
                .ReverseMap();
            CreateMap<FusionAddRequestModel, FusionModel>()
                .ForMember(e => e.Id, act => act.MapFrom(src => src.ObjectId))
                .ReverseMap();
            CreateMap<FusionUpdateRequestModel, FusionModel>()
                .ReverseMap();
        }
    }
}
