using AutoMapper;
using MyBlog.Model;
using MyBlog.Model.DTO;

namespace MyBlog.Utility
{
    public class CustomAutoMapperProfile : Profile
    {
        public CustomAutoMapperProfile()
        {
            base.CreateMap<WriteInfo, WriteInfoDTO>();
            base.CreateMap<BlogNews, BlogNewsDTO>()
                .ForMember(dest => dest.TypeInfoName,sou=>sou.MapFrom(src=>src.TypeInfo.Name))
                .ForMember(dest => dest.WriteInfoName, sou => sou.MapFrom(src => src.WriteInfo.Name));
        }
    }
}
