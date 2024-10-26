using Blog.Core.Context;
using Blog.Core.Models;
using Blog.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.Core.Repository
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(MeuDbContext db) : base(db)
        {
        }
        public async Task<IEnumerable<Post>> ObterPostsPorAutor(Guid autorId)
        {
            return await Buscar(p => p.AutorId == autorId);
        }
 
        public async Task<IEnumerable<Post>> ObterPostsAutores()
        {
            return await Db.Posts
                .AsNoTracking()
                .Include(p => p.Autor)
                .Include(p => p.Comentarios)
                .ThenInclude(c => c.Autor)
                .OrderBy(p => p.DataCadastro).ToListAsync();
        }

        public async Task<Post> ObterPostAutor(Guid id)
        {
            return await Db.Posts.AsNoTracking().Include(a => a.Autor).FirstOrDefaultAsync(p => p.Id == id);
        }

   }
}
