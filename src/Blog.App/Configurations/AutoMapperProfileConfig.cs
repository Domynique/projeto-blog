using AutoMapper;
using Blog.App.Models;
using Blog.Core.Business.Models;

namespace Blog.App.Configurations
{
    public class AutoMapperProfileConfig : Profile
    {
        public AutoMapperProfileConfig()
        {

            CreateMap<Post, PostViewModel>()
                .ForMember(dest => dest.PublicadoEm, opt => opt.MapFrom(src => src.DataCadastro))
                .ReverseMap();

            CreateMap<Autor, AutorViewModel>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Usuario.UserName));

            CreateMap<ComentarioViewModel, Comentario>()
                .ForMember(dest => dest.DataCadastro, opt => opt.MapFrom(src => src.PublicadoEm));

            CreateMap<Comentario, ComentarioViewModel>()
                .ForMember(dest => dest.NomeUsuario, opt => opt.MapFrom(src => src.Usuario.UserName))
                .ForMember(dest => dest.PublicadoEm, opt => opt.MapFrom(src => src.DataCadastro));


        }
    }
}
