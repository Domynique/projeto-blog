using Blog.Core.Business.Notifications;

namespace Blog.Core.Business.Interfaces
{
    public interface INotificador
    {
        bool TemNotificacao();
        List<Notificacao> ObterNotificacoes();
        void AddNotificacao(Notificacao notificacao);
    }
}
