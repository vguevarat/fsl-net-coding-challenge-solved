using HeyUrl.Dto.Url;
using HeyUrl.Entity;
using HeyUrl.Repository.Abstraction.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HeyUrl.Repository.Abstraction
{
    public interface IUrlRepository : ICreateRepository<Url>, IReadRepository<Url>
    {
        Task<IEnumerable<ListAllUrlResponseDto>> ListAllUrlsAsync();
    }
}
