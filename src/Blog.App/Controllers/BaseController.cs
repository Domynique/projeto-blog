using Blog.Core.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Blog.App.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly INotificador _notificador;
        private readonly IAppUser _appUser;

        protected BaseController(INotificador notificador, IAppUser appUser)
        {
            _notificador = notificador;
            _appUser = appUser;
        }

        protected bool OperacaoValida()
        {
            return !_notificador.TemNotificacao();
        }

        protected bool ValidarPermissao(string user)
        {
            return _appUser.IsUserAuthorize(user);
        }
    }
}
