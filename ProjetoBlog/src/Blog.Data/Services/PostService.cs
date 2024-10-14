using Blog.Data.Models;
using Blog.Data.Models.Base;
using Blog.Data.Notifications;
using Blog.Data.Repository;
using Blog.Data.Validations;

namespace Blog.Data.Services
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

        public async Task Remover(Guid id, string userId, bool isAdmin)
        {
            var post = await _postRepository.ObterPorId(id);
            if (post == null)
            {
                Notificar("Post não encontrado");
                return;
            }


            if (post.AutorId != Guid.Parse(userId) && !isAdmin)
            {
                Notificar("Acesso negado.");
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
