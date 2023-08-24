using EPAY.ETC.Core.API.Core.Models.Common;
using EPAY.ETC.Core.API.Core.Models.SearchRequest;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Context;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Vehicle;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Repositories.Vehicle
{
    public class VehicleRepositoryTests
    {
        #region Init mock data
        private Mock<CoreDbContext> _dbContextMock = new Mock<CoreDbContext>();
        private Mock<ILogger<VehicleRepository>> _loggerMock = new Mock<ILogger<VehicleRepository>>();
        private Mock<DbSet<VehicleModel>> _dbVehicleSetMock;
        #endregion

        private VehicleSearchRequestModel _request = new VehicleSearchRequestModel()
        {
            Refinements = new VehicleSearchRefinementsModel()
            {
                PlateNumber = "Some plate number",
            },
            SearchOptions = new SearchOptionsModel()
            {
                PageSize = 10,
            }
        };
        private List<VehicleModel> _vehicles = new List<VehicleModel>()
        {
            new VehicleModel()
            {
                Id = Guid.NewGuid()
            }


        };
    }
}