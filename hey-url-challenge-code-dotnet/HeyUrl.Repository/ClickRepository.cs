using HeyUrl.Dto.Click;
using HeyUrl.Entity;
using HeyUrl.Persistence;
using HeyUrl.Repository.Abstraction;
using HeyUrl.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeyUrl.Repository
{
    public class ClickRepository : BaseRepository<Click>, IClickRepository
    {
        public ClickRepository(HeyUrlDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<IEnumerable<ListClicksByUrlResponseDto>> ListClickUrlCurrentMonthAsync(Guid urlId)
        {
            return await FindBy(x => x.UrlId == urlId && x.Date.Month==DateTime.Now.Month && x.Date.Year == DateTime.Now.Year).Select(x => new ListClicksByUrlResponseDto
            {
                UrlId = x.UrlId,
                Browser = x.Browser,
                Platform = x.Platform,
                Date = x.Date
            })
            .ToListAsync();
        }

    }
}
