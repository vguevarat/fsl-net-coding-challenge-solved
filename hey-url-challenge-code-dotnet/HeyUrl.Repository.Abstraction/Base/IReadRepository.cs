using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HeyUrl.Repository.Abstraction.Base
{
    public interface IReadRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> FindAll();

        IQueryable<TEntity> FindAll(params Expression<Func<TEntity, object>>[] includeProperties);

        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> filter);

        Task<TEntity> GetByAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includeProperties);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter);
    }
}
