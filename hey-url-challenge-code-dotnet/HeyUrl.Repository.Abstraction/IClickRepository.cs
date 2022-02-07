using HeyUrl.Dto.Click;
using HeyUrl.Entity;
using HeyUrl.Repository.Abstraction.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HeyUrl.Repository.Abstraction
{
    public interface IClickRepository : ICreateRepository<Click>, IReadRepository<Click>
    {
        Task<IEnumerable<ListClicksByUrlResponseDto>> ListClickUrlCurrentMonthAsync(Guid urlId);
    }
}
