using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace HeyUrl.Repository.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<TEntity> IncludeProperties<TEntity>(this IQueryable<TEntity> queryable, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class
        {
            includeProperties.ToList().ForEach(includeProperty =>
            {
                queryable = queryable.Include(includeProperty);
            });

            return queryable;
        }

        public static IQueryable<TEntity> Filter<TEntity>(this IQueryable<TEntity> queryable, Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            return queryable.Where(filter);
        }
    }
}
