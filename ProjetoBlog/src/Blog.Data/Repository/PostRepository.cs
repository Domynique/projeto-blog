using Blog.Data.Context;
using Blog.Data.Models;

namespace Blog.Data.Repository
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(MeuDbContext db) : base(db)
        {
        }
    }
}
