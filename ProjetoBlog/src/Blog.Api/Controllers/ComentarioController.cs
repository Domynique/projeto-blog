using Blog.Data.Models;
using Blog.Data.Notifications;
using Blog.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Blog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentarioController : MainController
    {
        private readonly IComentarioService _comentarioService;

        public ComentarioController(IComentarioService comentarioService, INotificador notificador) : base(notificador)
        {
            _comentarioService = comentarioService;
        }

        [HttpGet]
        [ProducesDefaultResponseType(typeof(IEnumerable<Comentario>))]
        public async Task<ActionResult<IEnumerable<Comentario>>> ObterTodos()
        {
            var comentarios = await _comentarioService.ObterTodos();
            return CustomResponse(HttpStatusCode.OK, comentarios);
        }

        [HttpGet("{id}")]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Comentario>> ObterPorId(Guid id)
        {
            var comentario = await _comentarioService.ObterPorId(id);
            if (comentario == null)
            {
                NotificarErro("Comentário não encontrado!");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            return CustomResponse(HttpStatusCode.OK, comentario);

        }

        [Authorize]
        [HttpPost]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Adicionar(Comentario comentario)
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

            comentario.AutorId = Guid.Parse(usuarioId);

            await _comentarioService.Adicionar(comentario);

            return CreatedAtAction(nameof(ObterPorId), new { id = comentario.Id }, comentario);

        }

        [Authorize]
        [HttpPut("{id}")]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Atualizar(Guid id, Comentario comentario)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            var usuarioID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if( usuarioID == null)
            {
                NotificarErro("Autenticação necessária.");
                return CustomResponse(HttpStatusCode.Unauthorized);
            }

            var existeComentario = await _comentarioService.ObterPorId(id);
            if (existeComentario == null || (comentario.AutorId != Guid.Parse(usuarioID) && !User.IsInRole("Admin")))
            {
                NotificarErro("Você não tem permissão para realizar esta ação.");
                return CustomResponse(HttpStatusCode.Forbidden);
            }

            existeComentario.Conteudo = comentario.Conteudo;

            await _comentarioService.Atualizar(existeComentario);

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

            var comentario = await _comentarioService.ObterPorId(id);
            if (comentario == null)
            {
                return CustomResponse(HttpStatusCode.NotFound);
            }

            if (comentario.AutorId != Guid.Parse(usuarioId) && !User.IsInRole("Admin")) 
            {
                NotificarErro("Você não tem permissão para realizar esta ação.");
                return CustomResponse(HttpStatusCode.Forbidden);
            }

            await _comentarioService.Remover(id, usuarioId, User.IsInRole("Admin"));
            return CustomResponse(HttpStatusCode.NoContent);

        }
    
    }
}
