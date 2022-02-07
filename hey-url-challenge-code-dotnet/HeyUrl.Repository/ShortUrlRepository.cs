using HeyUrl.Entity;
using HeyUrl.Persistence;
using HeyUrl.Repository.Abstraction;
using HeyUrl.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HeyUrl.Repository
{
    public class ShortUrlRepository : BaseRepository<ShortUrl>, IShortUrlRepository
    {
        public ShortUrlRepository(HeyUrlDbContext dbContext) : base(dbContext)
        {

        }

        public Task<ShortUrl> GetLastShortUrlAsync()
        {
            return FindAll().OrderByDescending(x => x.Id).FirstOrDefaultAsync(x => !string.IsNullOrWhiteSpace(x.Code));
        }
    }
}
