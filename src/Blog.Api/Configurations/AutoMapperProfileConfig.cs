using AutoMapper;
using Blog.Api.ViewModels;
using Blog.Core.Business.Models;

namespace Blog.Api.Configurations
{
    public class AutoMapperProfileConfig : Profile
    {
        public AutoMapperProfileConfig() 
        {

            CreateMap<Comentario, ComentarioViewModel>()
                    .ForMember(dest => dest.NomeAutor, opt => opt.MapFrom(src => src.User.UserName))
                    .ForMember(dest => dest.PublicadoEm, opt => opt.MapFrom(src => src.DataCadastro));

            CreateMap<Post, PostViewModel>()
                    .ForMember(dest => dest.PublicadoEm, opt => opt.MapFrom(src => src.DataCadastro))
                    .ReverseMap();

            CreateMap<Autor, AutorViewModel>()
                    .ForMember(dest => dest.NomeAutor, opt => opt.MapFrom(src => src.User.UserName))
                    .ReverseMap();

            CreateMap<ComentarioViewModel, Comentario>()
                    .ForMember(dest => dest.DataCadastro, opt => opt.MapFrom(src => src.PublicadoEm));
         

        }
    }
}
