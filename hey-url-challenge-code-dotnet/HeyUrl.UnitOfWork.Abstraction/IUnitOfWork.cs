using System.Threading.Tasks;

namespace HeyUrl.UnitOfWork.Abstraction
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync();
        Task<object> BeginTransactionAsync();
    }
}
