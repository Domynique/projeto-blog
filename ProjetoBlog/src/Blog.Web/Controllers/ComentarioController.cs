using AutoMapper;
using Blog.Api.ViewModels;
using Blog.Core.Business.Interfaces;
using Blog.Core.Business.Models;
using Blog.Core.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Blog.Web.Controllers
{

    [Route("comentario")]
    public class ComentarioController : BaseController
    {
        private UserManager<ApplicationUser> _userManager;
        private IComentarioService _comentarioService;
        private IComentarioRepository _comentarioRepository;
        private IPostService _postService;
        private IMapper _mapper;

        public ComentarioController(UserManager<ApplicationUser> userManager,
                                    IComentarioService comentarioService,
                                    IComentarioRepository comentarioRepository,
                                    IPostService postService,
                                    IMapper mapper,
                                    INotificador notificador) : base(notificador)
        {
            _userManager = userManager;
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

            var comentariosPostViewModel = await ObterComentariosPostViewModel(comentarioViewModel.PostId);

            if (!ModelState.IsValid) 
            {
                return View("Index", comentariosPostViewModel);
            }

            var usuarioId = await ObterUsuarioId();

            var novoComentario = _mapper.Map<Comentario>(comentarioViewModel); 
            novoComentario.AutorId = usuarioId.Value;

            await _comentarioService.Adicionar(novoComentario); 

            if (!OperacaoValida()) 
            { 
                return View("Index", comentariosPostViewModel); 
            }
            
            TempData["Sucesso"] = "Comentário adicionado com sucesso!";

            return RedirectToAction("Index", new { postId = comentarioViewModel.PostId });

        }

        private async Task<ComentariosPostViewModel> ObterComentariosPostViewModel(Guid postId)
        {
            var post = await _postService.ObterPorId(postId);
			var postViewModel = _mapper.Map<PostViewModel>(post);

			var comentarios = await _comentarioRepository.ObterComentariosPorPost(postId);
            var comentariosViewModels = comentarios.Select(c => _mapper.Map<ComentarioViewModel>(c)).ToList();

            return new ComentariosPostViewModel
            {
                Post = postViewModel,
                Comentarios = comentariosViewModels
            };
        }

        private async Task<Guid?> ObterUsuarioId()
        {
            var user = await _userManager.GetUserAsync(User);
            return user?.Id;
        }
    }
}
