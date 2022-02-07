using HeyUrl.Dto.Url;
using HeyUrl.Entity;
using HeyUrl.Persistence;
using HeyUrl.Repository.Abstraction;
using HeyUrl.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeyUrl.Repository
{
    public class UrlRepository : BaseRepository<Url>, IUrlRepository
    {
        public UrlRepository(HeyUrlDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<IEnumerable<ListAllUrlResponseDto>> ListAllUrlsAsync()
        {
            return await FindAll().OrderByDescending(x => x.CreatedAt).Select(x => new ListAllUrlResponseDto
            {
                Id = x.Id,
                OriginalUrl = x.OriginalUrl,
                ShortUrl = x.ShortUrl,
                CreatedAt = x.CreatedAt,
                Count = x.Clicks.Count
            })
           .ToListAsync();
        }
    }
}
