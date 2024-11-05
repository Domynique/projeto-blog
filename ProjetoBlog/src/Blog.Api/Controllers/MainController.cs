using Blog.Core.Business.Interfaces;
using Blog.Core.Business.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace Blog.Api.Controllers
{

    [ApiController]
    public abstract class MainController : ControllerBase
    {
        private readonly INotificador _notificador;
        public readonly IAppUser _appUser;

        protected bool UsuarioAutenticado { get; set; }

        protected MainController(INotificador notificador, IAppUser appUser)
        {
            _notificador = notificador;
            _appUser = appUser;

            if (appUser.IsAuthenticated())
            {
                UsuarioAutenticado = true;
            }

        }

        protected bool OperacaoValida()
        {
            return !_notificador.TemNotificacao();
        }

        protected ActionResult CustomResponse(HttpStatusCode statusCode = HttpStatusCode.OK, object? result = null)
        {
            if (OperacaoValida())
            {
                return new ObjectResult(result)
                {
                    StatusCode = Convert.ToInt32(statusCode),
                };
            }

            return BadRequest(new
            {
                errors = _notificador.ObterNotificacoes().Select(n => n.Mensagem)
            });
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) NotificarErroModelInvalida(modelState);
            return CustomResponse();
        }

        protected void NotificarErroModelInvalida(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            
            foreach (var erro in erros)
            {
                var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotificarErro(errorMsg);
            }
        }
        protected void NotificarErro(string mensagem)
        {
            _notificador.AddNotificacao(new Notificacao(mensagem));
        }
    }
}
