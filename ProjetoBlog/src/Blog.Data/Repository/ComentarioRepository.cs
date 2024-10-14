using Blog.Data.Context;
using Blog.Data.Models;

namespace Blog.Data.Repository
{
    public class ComentarioRepository : Repository<Comentario>, IComentarioRepository
    {
        public ComentarioRepository(MeuDbContext db) : base(db)
        {
        }
    }
}
