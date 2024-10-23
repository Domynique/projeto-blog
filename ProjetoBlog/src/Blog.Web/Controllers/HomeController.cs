using AutoMapper;
using Blog.Api.ViewModels;
using Blog.Business.Interfaces;
using Blog.Business.Notifications;

using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        
        public HomeController(IPostService postService, 
                              IMapper mapper, 
                              INotificador notificador) : base(notificador)
        {
            _postService = postService;
            _mapper = mapper;
        }


        public async Task<IActionResult> Index()
        {
            try
            {
                var posts = await _postService.ObterTodos();
                var postViewModels = _mapper.Map<IEnumerable<PostViewModel>>(posts);
                return View(postViewModels);
            }
            catch (Exception ex)
            {
                NotificarErro("Erro ao carregar os posts. Tente novamente mais tarde.");
                return View("Error", new ErrorViewModel { Mensagem = ex.Message });
            }
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("erro/{id:length(3,3)}")]
        public IActionResult Errors(int id)
        {
            var modelErro = new ErrorViewModel();

            if (id == 500)
            {
                modelErro.Mensagem = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";
                modelErro.Titulo = "Ocorreu um erro!";
                modelErro.ErroCode = id;
            }
            else if (id == 404)
            {
                modelErro.Mensagem = "A página que está procurando não existe! <br />Em caso de dúvidas entre em contato com nosso suporte";
                modelErro.Titulo = "Ops! Página não encontrada.";
                modelErro.ErroCode = id;
            }
            else if (id == 403)
            {
                modelErro.Mensagem = "Você não tem permissão para fazer isto.";
                modelErro.Titulo = "Acesso Negado";
                modelErro.ErroCode = id;
            }
            else
            {
                return StatusCode(500);
            }

            return View("Error", modelErro);
        }
    }
}
