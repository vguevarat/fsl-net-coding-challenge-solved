using AutoMapper;
using HeyUrl.Dto.Click;
using HeyUrl.Entity;
using System;

namespace HeyUrl.Mapping
{
    public class ClickProfile : Profile
    {
        public ClickProfile()
        {
            CreateMap<CreateClickRequestDto, Click>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<Click, CreateClickResponseDto>();
        }
    }
}
