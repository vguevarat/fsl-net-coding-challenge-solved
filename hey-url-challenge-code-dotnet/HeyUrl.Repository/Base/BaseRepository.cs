using HeyUrl.Repository.Abstraction.Base;
using HeyUrl.Repository.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HeyUrl.Repository.Base
{
    public abstract class BaseRepository<TEntity> : ICreateRepository<TEntity>, IReadRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _dbContext;

        public BaseRepository(DbContext dbContext)
            => _dbContext = dbContext;

        #region ICreateRepository members

        public async Task AddAsync(TEntity entity)
        {
            if (entity == null) return;
            await _dbContext.Set<TEntity>().AddAsync(entity);
        }

        #endregion

        public IQueryable<TEntity> FindAll()
           => _dbContext.Set<TEntity>();

        public IQueryable<TEntity> FindAll(params Expression<Func<TEntity, object>>[] includeProperties)
            => FindAll().IncludeProperties(includeProperties);

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> filter)
           => FindAll().Filter(filter);

        public async Task<TEntity> GetByAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includeProperties)
            => await FindAll(includeProperties).Filter(filter).FirstOrDefaultAsync();

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter)
            => await FindAll().AnyAsync(filter);
    }
}
