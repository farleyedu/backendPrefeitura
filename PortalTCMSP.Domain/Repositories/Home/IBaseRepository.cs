using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Linq.Expressions;

namespace PortalTCMSP.Domain.Repositories.Home
{
    public interface IBaseRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity?> FindByIdAsync(long id);
        Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> AllAsync();
        Task<int> CountAllAsync();
        Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        Task DeleteAsync(TEntity entity);
        Task<bool> DeleteAsync(long id);
        Task InsertAsync(TEntity TEntity);
        Task InsertManyAsync(TEntity[] entities);
        Task<bool> CommitAsync();
        Task UpdateAsync(TEntity TEntity);
        Task UpdateManyAsync(TEntity[] TEntities);
    }
}
