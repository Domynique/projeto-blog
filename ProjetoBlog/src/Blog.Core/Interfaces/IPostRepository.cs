﻿using Blog.Core.Models;

namespace Blog.Core.Interfaces
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<IEnumerable<Post>> ObterPostsPorAutor(Guid autorId);

        Task<IEnumerable<Post>> ObterPostsAutores();

        Task<Post> ObterPostAutor(Guid id);
    }
}
