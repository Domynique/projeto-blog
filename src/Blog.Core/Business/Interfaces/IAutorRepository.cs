using Blog.Core.Business.Models;

namespace Blog.Core.Business.Interfaces
{
    public interface IAutorRepository : IRepository<Autor>
    {
        Task<Autor?> ObterAutorPorUserId(string userId);

    }
}
