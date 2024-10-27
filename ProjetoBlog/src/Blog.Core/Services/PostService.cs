using Blog.Core.Models;
using Blog.Core.Models.Base;
using Blog.Core.Notifications;
using Blog.Core.Interfaces;
using Blog.Core.Base.Validations;

namespace Blog.Core.Services
{
    public class PostService : BaseService, IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository, INotificador notificador) 
            : base(notificador) 
        {
            _postRepository = postRepository;
        }

        public async Task<Post?> ObterPorId(Guid id)
        {
            return await _postRepository.ObterPorId(id);
        }

        public async Task<List<Post>> ObterTodos()
        {
            return await _postRepository.ObterTodos();
        }

        public async Task Adicionar(Post post)
        {
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
