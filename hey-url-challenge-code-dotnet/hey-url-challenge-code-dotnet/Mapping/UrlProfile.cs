using AutoMapper;
using hey_url_challenge_code_dotnet.ViewModels.Url;
using HeyUrl.Dto.Url;

namespace hey_url_challenge_code_dotnet.Mapping.Url
{
    public class UrlProfile:Profile
    {
        public UrlProfile()
        {
            CreateMap<HomeViewModel, CreateUrlRequestDto>();

            CreateMap<ListAllUrlResponseDto, ListUrlViewModel>();
        }
    }
}
