﻿using Blog.Data.Models;

namespace Blog.Data.Services
{
    public interface IComentarioService : IDisposable
    {
        Task<Comentario?> ObterPorId(Guid id);
        Task<List<Comentario>> ObterTodos();
        Task Adicionar(Comentario comentario);
        Task Atualizar(Comentario comentario);
        Task Remover(Guid id, string userId, bool isAdmin);
    }
}
