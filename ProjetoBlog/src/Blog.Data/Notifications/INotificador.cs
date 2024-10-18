﻿namespace Blog.Data.Notifications
{
    public interface INotificador
    {
        bool TemNotificacao();
        List<Notificacao> obterNotificacoes();
        void Handle(Notificacao notificacao);
    }
}