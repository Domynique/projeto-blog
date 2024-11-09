using AutoMapper;
using Blog.App.Models;
using Blog.Core.Business.Interfaces;
using Blog.Core.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.App.Controllers
{
    [Route("post")]
    public class PostController : BaseController
    {
        private readonly IPostRepository _postRepository;
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly IAppUser _user; 

        public PostController(IPostService postService, 
                              IPostRepository postRepository,
                              IMapper mapper,
                              INotificador notificador, IAppUser appUser) : base(notificador, appUser)
        {
            _postService = postService;
            _mapper = mapper;
            _user = appUser;
            _postRepository = postRepository;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index() 
        {
            var postViewModel = _mapper.Map<IEnumerable<PostViewModel>>(await _postRepository.ObterTodos());

            ObterAutorizacoes(postViewModel, _user);

            return View(postViewModel);
        }

        [Authorize] 
        [HttpGet("adicionar")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost("adicionar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostViewModel postViewModel)
        {
            if (!ModelState.IsValid) return View(postViewModel);

            var post = _mapper.Map<Post>(postViewModel);
            
            await _postService.Adicionar(post);

            if (!OperacaoValida()) return View(postViewModel);

            TempData["Sucesso"] = "Post adicionado com sucesso!";

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet("editar/{id:Guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var post = await _postRepository.ObterPorId(id);

            var validationStatusCode = ValidarPost(post);

            if (validationStatusCode.HasValue)
            {
                return RedirectToAction("Index", "Error", new { statusCode = validationStatusCode.Value });
            }

            var postViewModel = _mapper.Map<PostViewModel>(post);

            ObterAutorizacao(postViewModel, _user);                  

            return View(postViewModel);
        }

        [Authorize] 
        [HttpPost("editar/{id:long}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, PostViewModel postViewModel)
        {

            if (!ModelState.IsValid)  return View(postViewModel);

            if (id != postViewModel.Id)
            {
                return RedirectToAction("Index", "Error", new { statusCode = 404 });
            }

            var post = await _postRepository.ObterPorId(id);

            var validationStatusCode = ValidarPost(post);

            if (validationStatusCode.HasValue)
            {
                return RedirectToAction("Index", "Error", new { statusCode = validationStatusCode.Value });
            }

            post.Titulo = postViewModel.Titulo;
            post.Conteudo = postViewModel.Conteudo;

            await _postService.Atualizar(post);

            if (!OperacaoValida()) return View(postViewModel);

            TempData["Sucesso"] = "Post editado com sucesso!";

            return RedirectToAction("Index");

        }

        [Authorize]
        [HttpGet("excluir/{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var post = await _postRepository.ObterPorId(id);

            var validationStatusCode = ValidarPost(post);

            if (validationStatusCode.HasValue)
            {
                return RedirectToAction("Index", "Error", new { statusCode = validationStatusCode.Value });
            }

            var postViewModel = _mapper.Map<PostViewModel>(post);
            
            ObterAutorizacao(postViewModel, _user);

            return View(postViewModel);
        }

        [Authorize] 
        [HttpPost("excluir/{id:Guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var post = await _postRepository.ObterPorId(id);

            var validationStatusCode = ValidarPost(post);

            if (validationStatusCode.HasValue)
            {
                return RedirectToAction("Index", "Error", new { statusCode = validationStatusCode.Value });
            }

            await _postService.Remover(id);

            if (!OperacaoValida()) return View(post);

            TempData["Sucesso"] = "Post excluido com sucesso!";

            return RedirectToAction("Index");
        }


        private int? ValidarPost(Post post)
        {
            if (post == null) return 404;

            var autorizado = ValidarPermissao(post.Autor!.UserId);

            if (!autorizado) return 403;

            return null;
        }

        private PostViewModel ObterAutorizacao(PostViewModel post, IAppUser appUser) 
        { 
            post.Autorizado = appUser.IsUserAuthorize(post.Autor?.UserId!); 
            
            return post; 
        }
        private IEnumerable<PostViewModel> ObterAutorizacoes(IEnumerable<PostViewModel> posts, IAppUser appUser) 
        { 
            foreach (var post in posts) 
            {
                ObterAutorizacao(post, appUser); 
            } 
            
            return posts; 
        }


    }

}
