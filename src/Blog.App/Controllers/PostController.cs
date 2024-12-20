﻿using AutoMapper;
using Blog.App.Configurations;
using Blog.App.Models;
using Blog.Core.Business.Interfaces;
using Blog.Core.Business.Models;
using Blog.Core.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Blog.App.Controllers
{
    [Route("posts")]
    public class PostController : BaseController
    {
        private readonly IPostRepository _postRepository;
        private readonly IPostService _postService;
        private readonly IComentarioRepository _comentarioRepository;
        private readonly IMapper _mapper;
        private readonly IAppUser _user;
        private readonly RazorViewToStringRenderer _renderer;

        public PostController(IPostService postService, 
                              IPostRepository postRepository,
                              IComentarioRepository comentarioRepository,
                              IMapper mapper,
                              RazorViewToStringRenderer renderer,
                              INotificador notificador, IAppUser appUser) : base(notificador, appUser)
        {
            _postService = postService;
            _mapper = mapper;
            _user = appUser;
            _postRepository = postRepository;
            _comentarioRepository = comentarioRepository;
            _renderer = renderer;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index() 
        {
            var postViewModel = _mapper.Map<IEnumerable<PostViewModel>>(await _postRepository.ObterPosts());

            ObterAutorizacoes(postViewModel, _user);

            return View(postViewModel);
        }

        [HttpGet("detalhes/{id:Guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var post = await _postRepository.ObterPostPorId(id);

            if (post == null)
            {
                return RedirectToAction("Index", "Error", new { statusCode = 404 });
            }

            var postViewModel = _mapper.Map<PostViewModel>(post);

            ObterAutorizacao(postViewModel, _user);

            ViewBag.UserLoggedIn = _user.IsAuthenticated();

            return View(postViewModel);
        }

        [Authorize] 
        [HttpGet("novo")]
        public IActionResult Create()
        {
            var postViewModel = new PostViewModel();
           
            return View(postViewModel);
        }

        [Authorize]
        [HttpPost("novo")]
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
            var post = await _postRepository.ObterPostPorId(id);

            var validationStatusCode = ValidarPost(post!);

            if (validationStatusCode.HasValue)
            {
                return RedirectToAction("Index", "Error", new { statusCode = validationStatusCode.Value });
            }

            var postViewModel = _mapper.Map<PostViewModel>(post);

            ObterAutorizacao(postViewModel, _user);
           
            return View(postViewModel);
        }

        [Authorize] 
        [HttpPost("editar/{id:Guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, PostViewModel postViewModel)
        {

            if (!ModelState.IsValid) 
            {
                var comentarios = await _comentarioRepository.ObterComentariosPorPost(id);
                
                var html = await _renderer.RenderViewToStringAsync(ControllerContext, "_PostForm", postViewModel); 
                var comentariosHtml = await _renderer.RenderViewToStringAsync(ControllerContext, "_ComentarioList", comentarios); 
                return Json(new 
                { 
                    success = false, 
                    html, 
                    comentariosHtml,
                    mensagens = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }
            
            if (id != postViewModel.Id)
            {
                return RedirectToAction("Index", "Error", new { statusCode = 404 });
            }
            
            var post = await _postRepository.ObterPostPorId(id);

            var validationStatusCode = ValidarPost(post);

            if (validationStatusCode.HasValue)
            {
                return RedirectToAction("Index", "Error", new { statusCode = validationStatusCode.Value });
            }

            post.Titulo = postViewModel.Titulo;
            post.Conteudo = postViewModel.Conteudo; 

            await _postService.Atualizar(post);

            if (!OperacaoValida()) 
            {
                var comentarios = await _comentarioRepository.ObterComentariosPorPost(id);

                var html = await _renderer.RenderViewToStringAsync(ControllerContext, "_PostForm", postViewModel);
                var comentariosHtml = await _renderer.RenderViewToStringAsync(ControllerContext, "_ComentarioList", comentarios);
                return Json(new
                {
                    success = false,
                    html,
                    comentariosHtml,
                    mensagens = TempData["Errors"]
                });
            }

            TempData["Sucesso"] = "Post editado com sucesso!";

            return Json(new { success = true, mensagem = TempData["Sucesso"] });

        }

        [Authorize]
        [HttpGet("excluir/{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var post = await _postRepository.ObterPostPorId(id);

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
            var post = await _postRepository.ObterPostPorId(id);

            var validationStatusCode = ValidarPost(post);

            if (validationStatusCode.HasValue)
            {
                return RedirectToAction("Index", "Error", new { statusCode = validationStatusCode.Value });
            }

            await _postService.Remover(id);

            if (!OperacaoValida()) return View(post);

            return RedirectToAction("Index");
        }


        private int? ValidarPost(Post post)
        {
            if (post == null) return 404;

            var autorizado = ValidarPermissao(post.Autor!.UsuarioId);

            if (!autorizado) return 403;

            return null;
        }

        private PostViewModel ObterAutorizacao(PostViewModel post, IAppUser appUser) 
        { 
            post.Autorizado = appUser.IsUserAuthorize(post.Autor?.UsuarioId); 
            
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
