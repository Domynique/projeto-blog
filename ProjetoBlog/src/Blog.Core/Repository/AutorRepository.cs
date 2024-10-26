using Blog.Core.Context;
using Blog.Core.Models;
using Blog.Core.Interfaces;

namespace Blog.Core.Repository
{
    public class AutorRepository : Repository<Autor>, IAutorRepository
    {
        public AutorRepository(MeuDbContext db) : base(db)
        {
        }
    }
}
