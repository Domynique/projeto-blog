using Blog.Core.Business.Models;
using System.Threading.Tasks;

namespace Blog.Core.Business.Interfaces
{
    public interface IAutorRepository : IRepository<Autor>
    {
        Task<Autor> ObterAutorPorUserId(string userId);

    }
}
