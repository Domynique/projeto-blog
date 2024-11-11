using Microsoft.EntityFrameworkCore;
using Blog.Core.Business.Interfaces;
using Blog.Core.Business.Models;
using Blog.Core.Data.Context;

namespace Blog.Core.Data.Repository
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(MeuDbContext db) : base(db)
        {
        }

        public async Task<Post?> ObterPostPorId(Guid id)
        {
            return await Db.Posts
                            .Include(p => p.Autor)
                            .ThenInclude(a => a.Usuario)
                            .Include(p => p.Comentarios)
                            .ThenInclude(c => c.Usuario)
                            .FirstOrDefaultAsync(p => p.Id == id && p.Ativo == true);
        }

        public async Task<IEnumerable<Post>> ObterPosts()
        {
            return await Db.Posts
                            .AsNoTracking()
                            .Include(p => p.Autor)
                            .ThenInclude(a => a.Usuario)
                            .Include(p => p.Comentarios)!
                            .ThenInclude(c => c.Usuario)
                            .OrderByDescending(c => c.DataCadastro)
                            .Where(p => p.Ativo == true)
                            .ToListAsync();
        }



    }
}
