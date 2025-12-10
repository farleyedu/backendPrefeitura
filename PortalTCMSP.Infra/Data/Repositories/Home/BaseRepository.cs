using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.BaseEntity;
using PortalTCMSP.Domain.Repositories.Home;
using PortalTCMSP.Infra.Data.Context;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace PortalTCMSP.Infra.Data.Repositories.Home
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : Entity
    {
        protected DbSet<TEntity> Set;
        protected readonly PortalTCMSPContext Context;

        protected BaseRepository(PortalTCMSPContext admanagerContext)
        {
            Set = admanagerContext.Set<TEntity>();
            Context = admanagerContext;
        }
        public virtual async Task<IEnumerable<TEntity>> AllAsync()
        {
            return await Set.ToListAsync();
        }

        public virtual async Task<int> CountAllAsync()
        {
            return await Set.CountAsync();
        }

        public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Set.AnyAsync(predicate);
        }

        public virtual Task DeleteAsync(TEntity entity)
        {
            Set.Remove(entity);
            Set.Attach(entity).State = EntityState.Deleted;
            return Task.CompletedTask;
        }

        public virtual async Task<bool> DeleteAsync(long id)
        {
            var entity = await Set.FindAsync(id);
            if (entity is null) return false;

            Set.Remove(entity);
            return true;
        }

        public virtual Task<TEntity?> FindByIdAsync(long id)
            => Set.FindAsync(new object[] { id }).AsTask();

        public virtual async Task<bool> CommitAsync()
        {
            return await Context.SaveChangesAsync() > 0;
        }

        public virtual Task InsertAsync(TEntity TEntity)
        {
            return Set.AddAsync(TEntity).AsTask();
        }

        public Task InsertManyAsync(TEntity[] entities)
        {
            return Set.AddRangeAsync(entities);
        }

        public virtual Task UpdateAsync(TEntity TEntity)
        {
            Set.Update(TEntity);
            return Task.CompletedTask;
        }

        public virtual Task UpdateManyAsync(TEntity[] TEntities)
        {
            Set.UpdateRange(TEntities);
            return Task.CompletedTask;
        }

        public async virtual Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var data = await Set.Where(predicate).ToListAsync();
            return data;
        }

        public virtual Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate)
            => Set.FirstOrDefaultAsync(predicate);

        public IEnumerable<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
