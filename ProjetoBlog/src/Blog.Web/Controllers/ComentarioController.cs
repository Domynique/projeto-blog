using AutoMapper;
using Blog.Api.ViewModels;
using Blog.Core.Interfaces;
using Blog.Core.Models;
using Blog.Core.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blog.Web.Controllers
{
    
    [Route("comentario")]
    public class ComentarioController : BaseController
    {
        private IComentarioService _comentarioService;
        private IComentarioRepository _comentarioRepository;
        private IPostService _postService;
        private IMapper _mapper;

        public ComentarioController(IComentarioService comentarioService,
                                    IComentarioRepository comentarioRepository,
                                    IPostService postService,
                                    IMapper mapper, 
                                    INotificador notificador) : base(notificador)
        {
            _comentarioService = comentarioService;
            _comentarioRepository = comentarioRepository;
            _postService = postService;
            _mapper = mapper;
        }
        
        [HttpGet("{postId}")]
        public async Task<IActionResult> Index(Guid postId) 
        {
            var comentariosViewModel = _mapper.Map<IEnumerable<ComentarioViewModel>>(await _comentarioRepository.ObterComentariosPorPost(postId));

            var postViewModel = _mapper.Map<PostViewModel>(await _postService.ObterPorId(postId));

            var viewModel = new ComentariosPostViewModel
            {
                Post = postViewModel,
                Comentarios = comentariosViewModel
            };

            ViewBag.PostId = postId;
            
            return View(viewModel);
        }
		
        [Authorize]
		[HttpPost("{postId}")]
        public async Task<IActionResult> Create(ComentarioViewModel comentarioViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", comentarioViewModel);
            }

            var comentario = _mapper.Map<Comentario>(comentarioViewModel); 
            
            comentario.AutorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            await _comentarioService.Adicionar(comentario);

            if (!OperacaoValida())
            {
                return View(comentarioViewModel);              
            
            }
            
            TempData["Sucesso"] = "Comentário adicionado com sucesso!";
            
            return RedirectToAction("Index", new {postId = comentarioViewModel.PostId});
        }
    }
}
