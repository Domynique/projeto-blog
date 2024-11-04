using AutoMapper;
using Blog.Api.ViewModels;
using Blog.Core.Business.Models;

namespace Blog.Api.Configurations
{
    public class AutoMapperSettings : Profile
    {
        public AutoMapperSettings() 
        {

            CreateMap<Comentario, ComentarioViewModel>()
                    .ForMember(dest => dest.NomeUsuario, opt => opt.MapFrom(src => src.Usuario.UserName))
                    .ForMember(dest => dest.DataPublicacao, opt => opt.MapFrom(src => src.DataCadastro));

            CreateMap<Post, PostViewModel>()
                    .ForMember(dest => dest.DataPublicacao, opt => opt.MapFrom(src => src.DataCadastro));

            CreateMap<Autor, AutorViewModel>()
                    .ForMember(dest => dest.NomeUsuario, opt => opt.MapFrom(src => src.Usuario.UserName));

            CreateMap<ComentarioViewModel, Comentario>()
                    .ForMember(dest => dest.DataCadastro, opt => opt.MapFrom(src => src.DataPublicacao));

            
            CreateMap<PostViewModel, Post>()
                    .ForMember(dest => dest.DataCadastro, opt => opt.MapFrom(src => src.DataPublicacao))
                    .ForMember(dest => dest.Comentarios, opt => opt.Ignore())
                    .ForMember(dest => dest.Autor, opt => opt.Ignore());
            
            CreateMap<AutorViewModel, Autor>();

        }
    }
}
