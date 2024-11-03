using AutoMapper;
using Blog.Api.ViewModels;
using Blog.Core.Interfaces;
using Blog.Core.Models;
using Blog.Core.Notifications;
using Blog.Core.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blog.Web.Controllers
{
    [Authorize]
    [Route("post")]
    public class PostController : BaseController
    {
        private readonly IPostService _postService;
        private readonly IPostRepository _postRepository;
        private readonly IComentarioService _comentarioService;
        private readonly IMapper _mapper;
        public PostController(IPostService postService,
                              IPostRepository postRepository,
                              IComentarioService comentarioService,
                              IMapper mapper,                            
                              INotificador notificador) : base(notificador)
        {
            _postService = postService;
            _postRepository = postRepository;
            _comentarioService = comentarioService;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index() 
        { 
            var autorId = Guid.Parse(User.FindFirstValue("AutorId"));
          
            return View(_mapper.Map<IEnumerable<PostViewModel>>(await _postRepository.ObterPostsPorAutor(autorId)));

        }

		[HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(PostViewModel postViewModel) 
        {
            if (!ModelState.IsValid)
            {
                return View(postViewModel);
            }

            var post = _mapper.Map<Post>(postViewModel);
            
            post.AutorId = Guid.Parse(User.FindFirstValue("AutorId"));

            await _postService.Adicionar(post);

            if (!OperacaoValida())
            {
                return View(postViewModel);

            }

            TempData["Sucesso"] = "Post adicionado com sucesso!";

            return RedirectToAction("Index", "Home");
        }
        

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(Guid id) 
        {
            var post = await _postService.ObterPorId(id);
            
            if (post == null) return NotFound();

            var postViewModel = _mapper.Map<PostViewModel>(post);
            
            return View(postViewModel);


        }

        [HttpPost("edit/{id}")]
        public async Task<IActionResult> Edit(Guid id, PostViewModel postViewModel)
        {
            if (!ModelState.IsValid) 
            { 
                return View(postViewModel); 
            }

            var post = _mapper.Map<Post>(postViewModel);

            await _postService.Atualizar(post);

            return RedirectToAction("Index", "Home");

        }

        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var post = await _postService.ObterPorId(id);
            
            if (post == null) return NotFound();

            var postViewModel = _mapper.Map<PostViewModel>(post);

            return View(postViewModel);
        }

        public async Task<IActionResult> DeleteConfirmed(Guid id, bool deleteComentario) 
        {
            var post = await _postService.ObterPorId(id);
            
            if (post == null) return NotFound();

            if (deleteComentario)
            {
                await _comentarioService.RemoverComentariosPorPost(id);
            }

            await _postService.Remover(id);

            return RedirectToAction("Index", "Home");
        }

    }
}
