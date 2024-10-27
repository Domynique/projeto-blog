using Blog.Core.Models;
using Blog.Core.Models.Base;
using Blog.Core.Notifications;
using Blog.Core.Base.Validations;
using Blog.Core.Interfaces;

namespace Blog.Core.Services
{
    public class ComentarioService : BaseService, IComentarioService
    {
        protected readonly IComentarioRepository _comentarioRepository;

        public ComentarioService(IComentarioRepository comentarioRepository, INotificador notificador)
            : base(notificador) 
        {
            _comentarioRepository = comentarioRepository;
        }
        public async Task<Comentario?> ObterPorId(Guid id)
        {
            return await _comentarioRepository.ObterPorId(id);
        }

        public async Task<List<Comentario>> ObterTodos()
        {
            return await _comentarioRepository.ObterTodos();
        }

        public async Task Adicionar(Comentario comentario)
        {
            if (!ExecutarValidacao(new ComentarioValidation(), comentario)) return;

            await _comentarioRepository.Adicionar(comentario);
        }

        public async Task Atualizar(Comentario comentario)
        {
            if (!ExecutarValidacao(new ComentarioValidation(), comentario)) return;

            await _comentarioRepository.Atualizar(comentario);
        }

        public async Task Remover(Guid id)
        {
            var comentario = await _comentarioRepository.ObterPorId(id);
           
            if (comentario == null)
            {
                Notificar("Comentario não encontrado");
                return;
            }     

            await _comentarioRepository.Remover(comentario.Id);

        }

        public async Task RemoverComentariosPorPost(Guid postId)
        {
            var comentarios = await _comentarioRepository.ObterComentariosPorPost(postId);
            foreach (var comentario in comentarios)
            {
                await _comentarioRepository.Remover(comentario.Id);
            }
        }

        public void Dispose()
        {
            _comentarioRepository.Dispose();
        }
    }
}
