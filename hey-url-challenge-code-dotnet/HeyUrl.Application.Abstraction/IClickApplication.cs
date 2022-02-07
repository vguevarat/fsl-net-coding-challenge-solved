using HeyUrl.Dto.Click;
using HeyUrl.Dto.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HeyUrl.Application.Abstraction
{
    public interface IClickApplication
    {
        Task<ResponseDto<CreateClickResponseDto>> CreateClickAsync(CreateClickRequestDto request);

        Task<IEnumerable<ListClicksByUrlResponseDto>> ListClickUrlCurrentMonthAsync(Guid urlId);
    }
}
