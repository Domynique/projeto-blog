using AutoMapper;
using Blog.Api.ViewModels;
using Blog.Business.Models;

namespace Blog.Api.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig() 
        {
            CreateMap<Post, PostViewModel>().ReverseMap();
            CreateMap<Comentario, ComentarioViewModel>().ReverseMap();
        }
    }
}
