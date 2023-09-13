using AutoMapper;
using EPAY.ETC.Core.API.Core.Models.Common;
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
                .ForMember(e => e.Id, act => act.MapFrom(src => src.Id))
                .ReverseMap();
            CreateMap<FusionRequestModel, FusionModel>()
                .ForMember(e => e.Id, act => act.MapFrom(src => src.ObjectId))
                .ReverseMap();
        }
    }
}
