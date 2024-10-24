﻿using Blog.Business.Models;
using Blog.Business.Models.Base;
using Blog.Business.Notifications;
using Blog.Business.Base.Validations;
using Blog.Business.Interfaces;

namespace Blog.Business.Services
{
    public class ComentarioService : BaseService, IComentarioService
    {
        protected readonly IComentarioRepository _comentarioRepository;

        public ComentarioService(IComentarioRepository comentarioRepository, INotificador notificador)
            : base(notificador) 
        {
            _comentarioRepository = comentarioRepository;
        }
        public async Task<Comentario?> ObterPorId(Guid id)
        {
            return await _comentarioRepository.ObterPorId(id);
        }

        public async Task<List<Comentario>> ObterTodos()
        {
            return await _comentarioRepository.ObterTodos();
        }

        public async Task Adicionar(Comentario comentario)
        {
            if (!ExecutarValidacao(new ComentarioValidation(), comentario)) return;

            await _comentarioRepository.Adicionar(comentario);
        }

        public async Task Atualizar(Comentario comentario)
        {
            if (!ExecutarValidacao(new ComentarioValidation(), comentario)) return;

            await _comentarioRepository.Atualizar(comentario);
        }

        public async Task Remover(Guid id, string userId, bool isAdmin)
        {
            var comentario = await _comentarioRepository.ObterPorId(id);
           
            if (comentario == null)
            {
                Notificar("Comentario não encontrado");
                return;
            }


            if (comentario.AutorId != Guid.Parse(userId) && !isAdmin)
            {
                Notificar("Acesso negado.");
                return;
            }
           

            await _comentarioRepository.Remover(comentario.Id);

        }

        public void Dispose()
        {
            _comentarioRepository.Dispose();
        }
    }
}
