using Blog.Core.Business.Models;

namespace Blog.Core.Business.Interfaces
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<IEnumerable<Post>> ObterPosts();
        Task<Post> ObterPostPorId(Guid id);
    }
}
