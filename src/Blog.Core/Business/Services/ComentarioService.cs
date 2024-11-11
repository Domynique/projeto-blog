using Blog.Core.Business.Models.Validations;
using Blog.Core.Business.Interfaces;
using Blog.Core.Business.Models;
using Blog.Core.Business.Models.Base;

namespace Blog.Core.Business.Services
{
    public class ComentarioService : BaseService, IComentarioService
    {
        protected readonly IComentarioRepository _comentarioRepository;
        protected readonly IIdentityUserService _identityUserService;
        protected readonly IAppUser _appUser;

        public ComentarioService(IComentarioRepository comentarioRepository,
                                 IIdentityUserService identityUserService,
                                 IAppUser appUser,
                                 INotificador notificador) : base(notificador)
        {
            _comentarioRepository = comentarioRepository;
            _identityUserService = identityUserService;
            _appUser = appUser;

        }

        public async Task Adicionar(Comentario comentario)
        {
            var userId = _appUser.GetUserId();
            var user = await _identityUserService.GetUserById(userId);

            comentario.UsuarioId = userId;
            comentario.Usuario = user;

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

        public void Dispose()
        {
            _comentarioRepository.Dispose();
        }
    }
}
