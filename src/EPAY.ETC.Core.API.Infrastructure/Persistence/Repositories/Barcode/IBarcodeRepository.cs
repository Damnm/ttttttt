using EPAY.ETC.Core.API.Core.Interfaces.Repositories;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Barcode
{
    public interface IBarcodeRepository:IRepository<Core.Models.Barcode.BarcodeModel, Guid>
    {
    }
}
