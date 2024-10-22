using Blog.Data.Context;
using Blog.Business.Models;
using Blog.Business.Interfaces;

namespace Blog.Data.Repository
{
    public class AutorRepository : Repository<Autor>, IAutorRepository
    {
        public AutorRepository(MeuDbContext db) : base(db)
        {
        }
    }
}
