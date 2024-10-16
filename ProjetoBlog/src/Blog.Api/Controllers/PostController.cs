using Blog.Data.Models;
using Blog.Data.Notifications;
using Blog.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Blog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : MainController
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService, INotificador notificador) : base(notificador)
        {
            _postService = postService;
        }

        [HttpGet]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<Post>>> ObterTodos()
        {
            var posts = await _postService.ObterTodos();
            return CustomResponse(HttpStatusCode.OK, posts);
        }

        [HttpGet("{id}")]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Post>> ObterPorId(Guid id)
        {
            var post = await _postService.ObterPorId(id);
            if (post == null) 
            {
                NotificarErro("Post não encontrado!");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            return CustomResponse(HttpStatusCode.OK, post);
        }

        [Authorize]
        [HttpPost]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Adicionar(Post post)
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

            post.AutorId = Guid.Parse(usuarioId);
            
            await _postService.Adicionar(post);

            return CreatedAtAction(nameof(ObterPorId), new { id = post.Id }, post);

        }

        [Authorize]
        [HttpPut("{id}")]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Atualizar(Guid id, Post post)
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

            existePost.Titulo = post.Titulo;
            existePost.Conteudo = post.Conteudo;

            await _postService.Atualizar(existePost);

            return CustomResponse(HttpStatusCode.NoContent);
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Excluir(Guid id)
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (usuarioId == null)
            {
                NotificarErro("Autenticação necessária.");
                return CustomResponse(HttpStatusCode.Unauthorized);
            }

            var post = await _postService.ObterPorId(id);
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
    }
}
