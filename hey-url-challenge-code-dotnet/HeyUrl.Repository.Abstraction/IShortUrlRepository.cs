using HeyUrl.Entity;
using HeyUrl.Repository.Abstraction.Base;
using System.Threading.Tasks;

namespace HeyUrl.Repository.Abstraction
{
    public interface IShortUrlRepository : ICreateRepository<ShortUrl>, IReadRepository<ShortUrl>
    {

        Task<ShortUrl> GetLastShortUrlAsync();
    }
}
