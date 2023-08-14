using EPAY.ETC.Core.API.Core.Entities;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Core.Interfaces.Repositories
{
    public interface IAddRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        Task<TEntity> AddAsync(TEntity entity);
    }
    public interface IUpdateRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        Task UpdateAsync(TEntity entity);
    }
    public interface IRemoveRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        Task RemoveAsync(TEntity entity);
    }
    public interface IGetByIdRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        Task<TEntity?> GetByIdAsync(TKey id);
    }
    public interface IGetAllRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? expression = null);
        Task<TEntity?> GetByIdAsync(TKey id);
    }
    public interface IRepository<TEntity, TKey> :
            IAddRepository<TEntity, TKey>,
            IUpdateRepository<TEntity, TKey>,
            IRemoveRepository<TEntity, TKey>,
            IGetAllRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
    }
}
