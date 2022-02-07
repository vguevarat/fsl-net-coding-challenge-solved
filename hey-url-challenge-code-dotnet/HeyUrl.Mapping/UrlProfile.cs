using AutoMapper;
using HeyUrl.Dto.Url;
using HeyUrl.Entity;
using System;

namespace HeyUrl.Mapping
{
    public class UrlProfile : Profile
    {
        public UrlProfile()
        {
            CreateMap<CreateUrlRequestDto, Url>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<Url, CreateUrlResponseDto>();

            CreateMap<Url, GetUrlResponseDto>();
        }
    }
}
