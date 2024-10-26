using Blog.Data.Context;
using Blog.Core.Models;
using Blog.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Repository
{
    public class ComentarioRepository : Repository<Comentario>, IComentarioRepository
    {
        public ComentarioRepository(MeuDbContext db) : base(db)
        {
        }
        public async Task<IEnumerable<Comentario>> ObterComentariosPorPost(Guid postId)
        {
            return await Buscar(c => c.PostId == postId);
        }

        public async Task<IEnumerable<Comentario>> ObterComentariosPosts()
        {
            return await Db.Comentarios
                .AsNoTracking()
                .Include(p => p.Post)
                .Include(p => p.Autor)
                .OrderBy(c => c.DataCadastro)
                .ToListAsync();

        }

        public async Task<Comentario> ObterComentarioPost(Guid id)
        {
            return await Db.Comentarios.AsNoTracking().Include(p => p.Post).FirstOrDefaultAsync(c => c.Id == id);
        }
    }


}

