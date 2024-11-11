using Blog.Core.Business.Interfaces;
using Blog.Core.Business.Models;
using Blog.Core.Business.Models.Base;
using Blog.Core.Business.Models.Validations;

namespace Blog.Core.Business.Services
{
    public class PostService : BaseService, IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IAutorRepository _autorRepository;
        private readonly IAppUser _appUser;

        public PostService(IPostRepository postRepository,
                           IAutorRepository autorRepository,
                           IAppUser appUser, 
                           INotificador notificador) : base(notificador)
        {
            _postRepository = postRepository;
            _autorRepository = autorRepository;
            _appUser = appUser;
        }


        public async Task Adicionar(Post post)
        {
            var userId = _appUser.GetUserId();
            var autor = await _autorRepository.ObterAutorPorUserId(userId);

            if (autor == null)
            {
                autor = new Autor { UsuarioId = userId };
                await _autorRepository.Adicionar(autor);
            }

            post.Autor = autor;


            if (!ExecutarValidacao(new PostValidation(), post)) return;
            
            await _postRepository.Adicionar(post);

        }

        public async Task Atualizar(Post post)
        {

            if (!ExecutarValidacao(new PostValidation(), post)) return;

            await _postRepository.Atualizar(post);
        }

        public async Task Remover(Guid id)
        {
            var post = await _postRepository.ObterPorId(id);
            if (post == null)
            {
                Notificar("Post não encontrado");
                return;
            }
            await _postRepository.Remover(post.Id);
        }

        public void Dispose()
        {
            _postRepository.Dispose();
        }
    }
}
