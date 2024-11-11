using Microsoft.EntityFrameworkCore;
using Blog.Core.Business.Interfaces;
using Blog.Core.Business.Models;
using Blog.Core.Data.Context;

namespace Blog.Core.Data.Repository
{
    public class ComentarioRepository : Repository<Comentario>, IComentarioRepository
    {
        public ComentarioRepository(MeuDbContext db) : base(db)
        {
        }

        public async Task<Comentario?> ObterComentarioPorPost(Guid id, Guid postId)
        {
            return await Db.Comentarios
                            .Include(c => c.Usuario)
                            .Include(c => c.Post)
                            .ThenInclude(p => p.Autor)
                            .Where(c => c.PostId == postId)
                            .FirstOrDefaultAsync(c => c.Id == id && c.Ativo == true);
        }

        public async Task<IEnumerable<Comentario>> ObterComentariosPorPost(Guid postId)
        {
            return await Db.Comentarios
                .AsNoTracking()
                .Include(c => c.Usuario)
                .Where(c => c.PostId == postId)
                .OrderBy(c => c.DataCadastro)
                .Where(c => c.Ativo == true)
                .ToListAsync();

        }

    }


}

