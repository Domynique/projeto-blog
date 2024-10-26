using AutoMapper;
using Blog.Api.ViewModels;
using Blog.Core.Models;

namespace Blog.Api.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig() 
        {
            CreateMap<Post, PostViewModel>()
                        .ForMember(dest => dest.NomeAutor, opt => opt.MapFrom(src => src.Autor.Nome))
                        .ReverseMap();

            CreateMap<Comentario, ComentarioViewModel>()
                .ForMember(dest => dest.NomeAutor, opt => opt.MapFrom(src => src.Autor.Nome))
                .ReverseMap();

            CreateMap<Autor, AutorViewModel>().ReverseMap();
            CreateMap<ApplicationUser, ApplicationUserViewModel>().ReverseMap();

            CreateMap<RegisterViewModel, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<LoginViewModel, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
        }
    }
}
