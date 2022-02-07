using HeyUrl.Dto.Response;
using HeyUrl.Dto.Url;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HeyUrl.Application.Abstraction
{
    public interface IUrlApplication
    {
        Task<ResponseDto<CreateUrlResponseDto>> CreateUrlAsync(CreateUrlRequestDto request);

        Task<IEnumerable<ListAllUrlResponseDto>> ListAllUrlsAsync();

        Task<GetUrlResponseDto> GetUrlByShortUrlAsync(string shortUrl);
    }
}
