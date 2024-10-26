﻿using Blog.Core.Models;

namespace Blog.Core.Interfaces
{
    public interface IAutorService : IDisposable
    {
        Task<Autor?> ObterPorId(Guid id);
        Task<List<Autor>> ObterTodos();
        Task Adicionar(Autor autor);
        Task Atualizar(Autor autor);
        Task Remover(Guid id, string userId, bool isAdmin);
    }
}
