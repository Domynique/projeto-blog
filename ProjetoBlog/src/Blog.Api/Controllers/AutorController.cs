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
    public class AutorController : MainController
    {
        private readonly IAutorService _autorService;

        public AutorController(IAutorService autorService, INotificador notificador) : base(notificador)
        {
            _autorService = autorService;

        }

        [HttpGet]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<Autor>>> ObterTodos()
        {
            var autores = await _autorService.ObterTodos();
            return CustomResponse(HttpStatusCode.OK, autores);
        }

        [HttpGet("{id}")]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Autor>> ObterPorId(Guid id) 
        { 
            var autor = await _autorService.ObterPorId(id);

            if (autor == null)
            {
                NotificarErro("Autor não encontrado!");
                return CustomResponse(HttpStatusCode.NotFound);
            }
            
            return CustomResponse(HttpStatusCode.OK, autor);

        }

        [Authorize]
        [HttpPut("{id}")]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Atualizar(Guid id, Autor autor) 
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
            
            var autorExistente = await _autorService.ObterPorId(id);
            if (autorExistente == null || (autorExistente.Id != Guid.Parse(usuarioId) && !User.IsInRole("Admin"))) 
            {
                NotificarErro("Você não tem permissão para realizar esta ação.");
                return CustomResponse(HttpStatusCode.Forbidden);
            }

            autorExistente.Email = autor.Email;
            autorExistente.UserName = autor.UserName;
            autorExistente.Nome = autor.Nome;
            autorExistente.Biografia = autor.Biografia;

            await _autorService.Atualizar(autorExistente);
            return CustomResponse(HttpStatusCode.NoContent);

        }

        [Authorize(Roles = "Admin")]
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

            var autorExistente = await _autorService.ObterPorId(id);
            if (autorExistente == null) 
            { 
                return CustomResponse(HttpStatusCode.NotFound);
            }

            await _autorService.Remover(id, usuarioId, true);
            return CustomResponse(HttpStatusCode.NoContent);
        }
      

    }
}
