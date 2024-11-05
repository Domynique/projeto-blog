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

        public async Task<Post> ObterPostPorId(Guid id)
        {
            return await _db.Posts
                            .Include(p => p.Autor)
                            .ThenInclude(a => a.User)
                            .Include(p => p.Comentarios)
                            .ThenInclude(c => c.User)
                            .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Post>> ObterPosts()
        {
            return await _db.Posts
                            .AsNoTracking()
                            .Include(p => p.Autor)
                            .ThenInclude(a => a.User)
                            .Include(p => p.Comentarios)
                            .ThenInclude(c => c.User)
                            .ToListAsync();
        }



    }
}
