using Blog.Data.Models;
using Blog.Data.Models.Base;
using Blog.Data.Notifications;
using Blog.Data.Repository;
using Blog.Data.Validations;

namespace Blog.Data.Services
{
    public class AutorService : BaseService, IAutorService
    {
        private readonly IAutorRepository _autorRepository;

        public AutorService(IAutorRepository autorRepository, INotificador notificador) 
            : base(notificador)
        {
            _autorRepository = autorRepository;
        }

        public async Task<Autor?> ObterPorId(Guid id)
        {
            return await _autorRepository.ObterPorId(id);
        }

        public async Task<List<Autor>> ObterTodos()
        {
            return await _autorRepository.ObterTodos();
        }

        public async Task Adicionar(Autor autor)
        {
            if (!ExecutarValidacao(new AutorValidation(), autor)) return;

            await _autorRepository.Adicionar(autor);
        }

        public async Task Atualizar(Autor autor)
        {
            if (!ExecutarValidacao(new AutorValidation(), autor)) return;

            await _autorRepository.Atualizar(autor);
        }

        public async Task Remover(Guid id, string userId, bool isAdmin)
        {
            var autor = await _autorRepository.ObterPorId(id);
            if (autor == null)
            {
                Notificar("Autor não encontrado");
                return;
            }


            if (autor.Id != Guid.Parse(userId) && !isAdmin)
            {
                Notificar("Acesso negado.");
                return;
            }

            await _autorRepository.Remover(autor.Id);
        }

        public void Dispose()
        {
            _autorRepository?.Dispose();
        }
    }
}
