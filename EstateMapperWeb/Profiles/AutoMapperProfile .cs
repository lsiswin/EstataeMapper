using AutoMapper;
using EstateMapperLibrary.Models;

namespace EstateMapperWeb.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // HouseDto → House
            CreateMap<HouseDto, House>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // 忽略自增字段
                .ForMember(dest => dest.Layouts, opt => opt.MapFrom(src => src.Layouts)) // 映射 Layouts
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags)); // Tags 手动处理

            // LayoutDto → Layout
            CreateMap<LayoutDto, Layout>()
                .ForMember(dest => dest.HouseId, opt => opt.Ignore()); // 由 EF 自动填充

            // TagDto → Tag（仅映射基础字段）
            CreateMap<TagDto, Tag>()
                .ForMember(dest => dest.HouseId, opt => opt.Ignore());
           

           

            // 反向映射（用于返回 DTO）
            CreateMap<House, HouseDto>();
            CreateMap<Layout, LayoutDto>();
            CreateMap<Tag, TagDto>();
        }
    }
}
