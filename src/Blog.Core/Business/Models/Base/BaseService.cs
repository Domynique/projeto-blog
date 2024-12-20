﻿using Blog.Core.Business.Interfaces;
using Blog.Core.Business.Notifications;
using FluentValidation;
using FluentValidation.Results;

namespace Blog.Core.Business.Models.Base
{
    public abstract class BaseService
    {
        private readonly INotificador _notificador;

        protected BaseService(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected void Notificar(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Notificar(error.ErrorMessage);
            }
        }

        protected void Notificar(string mensagem)
        {
            _notificador.AddNotificacao(new Notificacao(mensagem));
        }

        protected bool ExecutarValidacao<TV, TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE>
        {
            var validador = validacao.Validate(entidade);

            if (validador.IsValid) return true;

            Notificar(validador);

            return false;
        }
    }
}
