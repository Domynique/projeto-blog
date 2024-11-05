using Blog.Core.Business.Interfaces;
using Blog.Core.Business.Models;
using Blog.Core.Business.Models.Base;

namespace Blog.Core.Business.Services
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

        public void Dispose()
        {
            _autorRepository?.Dispose();
        }
    }
}
