using Blog.Data.Context;
using Blog.Business.Models;
using Blog.Business.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Repository
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
                .OrderBy(p => p.DataCadastro).ToListAsync();
        }

        public async Task<Post> ObterPostAutor(Guid id)
        {
            return await Db.Posts.AsNoTracking().Include(a => a.Autor).FirstOrDefaultAsync(p => p.Id == id);
        }

   }
}
