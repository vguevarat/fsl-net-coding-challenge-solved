using System.Threading.Tasks;

namespace HeyUrl.Repository.Abstraction.Base
{
    public interface ICreateRepository<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity entity);
    }
}
