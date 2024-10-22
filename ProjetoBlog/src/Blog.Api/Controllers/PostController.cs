using Blog.Business.Notifications;
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
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : MainController
    {
        private readonly IPostRepository _postRepository;
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public PostController(INotificador notificador, 
                              IMapper mapper,
                              IPostService postService,
                              IPostRepository postRepository) : base(notificador)
        {
            _postRepository = postRepository;   
            _postService = postService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<PostViewModel>>> ObterTodos()
        {
            var posts = _mapper.Map<IEnumerable<PostViewModel>>(await _postRepository.ObterPostsAutores());
            return CustomResponse(HttpStatusCode.OK, posts);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
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
        [ProducesDefaultResponseType]
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

            var novopost = new Post
            {
                Titulo = postViewModel.Titulo,
                Conteudo = postViewModel.Conteudo,
                AutorId = Guid.Parse(usuarioId),
                DataCadastro = DateTime.Now
            };

            await _postService.Adicionar(novopost);

            return CreatedAtAction(nameof(ObterPorId), new { id = novopost.Id }, novopost);

        }

        [Authorize]
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesDefaultResponseType]
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

            existePost.Titulo = postViewModel.Titulo;
            existePost.Conteudo = postViewModel.Conteudo;

            await _postService.Atualizar(existePost);

            return CustomResponse(HttpStatusCode.NoContent);
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
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

            await _postService.Remover(id, usuarioId, User.IsInRole("Admin"));
            return CustomResponse(HttpStatusCode.NoContent);

        }
        private async Task<PostViewModel> ObterPost(Guid id)
        {
            return _mapper.Map<PostViewModel>(await _postRepository.ObterPostAutor(id));
        }
    }
}
