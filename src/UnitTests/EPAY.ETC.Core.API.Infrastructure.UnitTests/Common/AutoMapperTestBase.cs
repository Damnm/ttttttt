using AutoMapper;
using EPAY.ETC.Core.API.Mapping;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Common
{
    public class AutoMapperTestBase
    {
        private static MapperConfiguration mapperConfig = new MapperConfiguration(
            cfg =>
            {
                cfg.AddProfile(new Mappings());
            });

        public IMapper _mapper = new Mapper(mapperConfig);
    }
}
