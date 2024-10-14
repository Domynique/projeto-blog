using Blog.Data.Context;
using Blog.Data.Models;

namespace Blog.Data.Repository
{
    public class AutorRepository : Repository<Autor>, IAutorRepository
    {
        public AutorRepository(MeuDbContext db) : base(db)
        {
        }
    }
}
