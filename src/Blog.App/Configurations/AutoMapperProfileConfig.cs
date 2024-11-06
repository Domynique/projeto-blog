using AutoMapper;
using Blog.App.Models;
using Blog.Core.Business.Models;

namespace Blog.App.Configurations
{
    public class AutoMapperProfileConfig : Profile
    {
        public AutoMapperProfileConfig()
        {

            CreateMap<Comentario, ComentarioViewModel>()
                    .ForMember(dest => dest.NomeAutor, opt => opt.MapFrom(src => src.User.UserName))
                    .ForMember(dest => dest.PublicadoEm, opt => opt.MapFrom(src => src.DataCadastro))
                    .ReverseMap();

            CreateMap<Post, PostViewModel>()
                    .ForMember(dest => dest.PublicadoEm, opt => opt.MapFrom(src => src.DataCadastro))
                    .ReverseMap();

            CreateMap<Autor, AutorViewModel>()
                    .ForMember(dest => dest.NomeAutor, opt => opt.MapFrom(src => src.User.UserName))
                    .ReverseMap();


        }
    }
}
