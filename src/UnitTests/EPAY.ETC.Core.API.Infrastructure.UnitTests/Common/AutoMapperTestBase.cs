using AutoMapper;
using EPAY.ETC.Core.API.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    //public class UnitTestMappings : Profile
    //{
    //    public UnitTestMappings() 
    //    {

    //    }
    //}
}
