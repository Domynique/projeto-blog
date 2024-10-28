using Blog.Core.Context;
using Blog.Core.Models;
using Blog.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.Core.Repository
{
    public class ComentarioRepository : Repository<Comentario>, IComentarioRepository
    {
        public ComentarioRepository(MeuDbContext db) : base(db)
        {
        }
        public async Task<IEnumerable<Comentario>> ObterComentariosPorPost(Guid postId)
        {
            return await Db.Comentarios
                .AsNoTracking()
                .Include(c => c.Autor)
                .Where(c => c.PostId == postId)
                .ToListAsync();
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

