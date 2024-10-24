﻿using Blog.Business.Notifications;
using Blog.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using Blog.Api.ViewModels;
using Blog.Business.Models;
using AutoMapper;


namespace Blog.Api.Controllers
{
    [Route("api/Posts")]
    [ApiController]
    public class PostController : MainController
    {
        private readonly IPostRepository _postRepository;
        private readonly IComentarioRepository _comentarioRepository;
        private readonly IPostService _postService;
        private readonly IComentarioService _comentarioService;
        private readonly IMapper _mapper;

        public PostController(INotificador notificador, 
                              IMapper mapper,
                              IPostService postService,
                              IComentarioService comentarioService,
                              IPostRepository postRepository,
                              IComentarioRepository comentarioRepository ) : base(notificador)
        {
            _postRepository = postRepository;
            _comentarioRepository = comentarioRepository;
            _postService = postService;
            _comentarioService = comentarioService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PostViewModel>>> ObterTodos()
        {
            var posts = _mapper.Map<IEnumerable<PostViewModel>>(await _postRepository.ObterPostsAutores());
            return CustomResponse(HttpStatusCode.OK, posts);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PostViewModel>> ObterPorId(Guid id)
        {
            var post = await ObterPost(id);
            if (post == null) 
            {
                NotificarErro("Post não encontrado!");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            return CustomResponse(HttpStatusCode.OK, post);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Adicionar(PostViewModel postViewModel)
        {
            if (!ModelState.IsValid) 
            { 
                return CustomResponse(ModelState);
            }

            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (usuarioId == null)
            {
                NotificarErro("Autenticação necessária.");
                return CustomResponse(HttpStatusCode.Unauthorized);
            }

            var novoPost = _mapper.Map<Post>(postViewModel);
            novoPost.AutorId = Guid.Parse(usuarioId);
            novoPost.DataCadastro = DateTime.Now;           

            await _postService.Adicionar(novoPost);

            return CreatedAtAction(nameof(ObterPorId), new { id = novoPost.Id }, novoPost);

        }

        [Authorize]
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Atualizar(Guid id, PostViewModel postViewModel)
        {
            if (!ModelState.IsValid) 
            { 
                return CustomResponse(ModelState);
            }

            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (usuarioId == null) 
            {
                NotificarErro("Autenticação necessária.");
                return CustomResponse(HttpStatusCode.Unauthorized);
            }

            var existePost = await _postService.ObterPorId(id);
            if (existePost == null || (existePost.AutorId != Guid.Parse(usuarioId) && !User.IsInRole("Admin")))
            {
                NotificarErro("Você não tem permissão para realizar esta ação.");
                return CustomResponse(HttpStatusCode.Forbidden);
            }

            _mapper.Map(postViewModel, existePost);

            await _postService.Atualizar(existePost);

            return CustomResponse(HttpStatusCode.NoContent);
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Excluir(Guid id)
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (usuarioId == null)
            {
                NotificarErro("Autenticação necessária.");
                return CustomResponse(HttpStatusCode.Unauthorized);
            }

            var post = await ObterPost(id);
            if (post == null)
            {
                return CustomResponse(HttpStatusCode.NotFound);
            }

            if(post.AutorId != Guid.Parse(usuarioId) && !User.IsInRole("Admin"))
            {
                NotificarErro("Você não tem permissão para realizar esta ação.");
                return CustomResponse(HttpStatusCode.Forbidden);
            }

            var comentarios =  _mapper.Map<IEnumerable<ComentarioViewModel>>(await _comentarioRepository.ObterComentarioPost(id));
            foreach (var comentario in comentarios)
            {
                await _comentarioService.Remover(id, usuarioId, User.IsInRole("Admin"));
            }

            await _postService.Remover(id, usuarioId, User.IsInRole("Admin"));
            return CustomResponse(HttpStatusCode.NoContent);

        }
        private async Task<PostViewModel> ObterPost(Guid id)
        {
            return _mapper.Map<PostViewModel>(await _postRepository.ObterPostAutor(id));
        }
    }
}
