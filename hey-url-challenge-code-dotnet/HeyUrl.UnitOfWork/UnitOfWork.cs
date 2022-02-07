using HeyUrl.UnitOfWork.Abstraction;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HeyUrl.UnitOfWork
{
    public class UnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
    {
        readonly TDbContext _dbContext;
        public UnitOfWork(TDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        public Task<int> CommitAsync()
            => _dbContext.SaveChangesAsync();

        public async Task<object> BeginTransactionAsync()
        {
            return await _dbContext.Database.BeginTransactionAsync();
        }
    }
}
