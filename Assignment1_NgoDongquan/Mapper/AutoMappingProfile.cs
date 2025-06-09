using BusinessObject.Models;
using AutoMapper;
using BusinessObject.Dto;

namespace Assignment1_NgoDongquan.Mapper
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<Category, CategoryReturnDto>()
                .ForMember(dest => dest.ParentCategory, opt => opt.MapFrom(src => src.ParentCategory));

            CreateMap<Category, ParentCategoryDto>();
            CreateMap<Category, CategoryReturnDto>();
        }
    }
}
