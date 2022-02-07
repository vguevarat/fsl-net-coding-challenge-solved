using AutoMapper;
using hey_url_challenge_code_dotnet.ViewModels;
using HeyUrl.Dto.Url;

namespace hey_url_challenge_code_dotnet.Mapping.Url
{
    public class ClickProfile:Profile
    {
        public ClickProfile()
        {
            CreateMap<GetUrlResponseDto, ShowViewModel>();
        }
    }
}
