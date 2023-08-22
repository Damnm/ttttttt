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
            CreateMap<VehicleModel, VehicleHistoryModel>()
                .ForMember(e => e.Id, act => act.MapFrom(src => Guid.NewGuid()))
                .ForMember(e => e.CreatedDate, act => act.MapFrom(src => DateTime.Now.ConvertToAsianTime(DateTimeKind.Local)));
        }
    }
}
