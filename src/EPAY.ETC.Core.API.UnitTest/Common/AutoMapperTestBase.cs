using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EPAY.ETC.Core.API.Mapping;

namespace ACV.Toll.Admin.API.Infrastructure.UnitTests.Common
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
