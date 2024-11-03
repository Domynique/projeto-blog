using Blog.Core.Models;
using Blog.Core.Notifications;
using Blog.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using AutoMapper;
using Blog.Api.ViewModels;
using Blog.Core.Services;
using Microsoft.AspNetCore.Identity;

namespace Blog.Api.Controllers
{
    [Route("api/Comentarios")]
    [ApiController]
    public class ComentarioController : MainController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IComentarioRepository _comentarioRepository;
        private readonly IComentarioService _comentarioService;
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public ComentarioController(IComentarioRepository comentarioRepository, 
                                    IComentarioService comentarioService, 
                                    IPostService postService,
                                    IMapper mapper, 
                                    INotificador notificador) : base(notificador)
        {
            _comentarioRepository = comentarioRepository;
            _comentarioService = comentarioService;
            _postService = postService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ComentarioViewModel>>> ObterTodos()
        {
            var comentarios = _mapper.Map<IEnumerable<ComentarioViewModel>>(await _comentarioRepository.ObterComentariosPosts());
            return CustomResponse(HttpStatusCode.OK, comentarios);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ComentarioViewModel>> ObterPorId(Guid id)
        {
            var comentario = await ObterComentario(id);
            if (comentario == null)
            {
                NotificarErro("Comentário não encontrado!");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            return CustomResponse(HttpStatusCode.OK, comentario);

        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Adicionar(ComentarioViewModel comentarioViewModel)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            var usuarioId = await ObterUsuarioId();
            if (usuarioId == null)
            {
                NotificarErro("Autenticação necessária.");
                return CustomResponse(HttpStatusCode.Unauthorized);
            }

            var novoComentario = _mapper.Map<Comentario>(comentarioViewModel);
            novoComentario.AutorId = usuarioId.Value;
            //novoComentario.DataCadastro = DateTime.Now;

            await _comentarioService.Adicionar(novoComentario);

            return CreatedAtAction(nameof(ObterPorId), new { id = novoComentario.Id }, novoComentario);

        }

        [Authorize]
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Atualizar(Guid id, ComentarioViewModel comentarioViewModel)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            var usuarioId = await ObterUsuarioId();
            if( usuarioId == null)
            {
                NotificarErro("Autenticação necessária.");
                return CustomResponse(HttpStatusCode.Unauthorized);
            }

            if (!await VerificarPermissao(usuarioId.Value, id))
            {
                NotificarErro("Você não tem permissão para realizar esta ação.");
                return CustomResponse(HttpStatusCode.Forbidden);

            }
            var existeComentario = await _comentarioService.ObterPorId(id);

            _mapper.Map(comentarioViewModel, existeComentario);

            await _comentarioService.Atualizar(existeComentario);

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
            var usuarioId = await ObterUsuarioId();
            if (usuarioId == null)
            {
                NotificarErro("Autenticação necessária.");
                return CustomResponse(HttpStatusCode.Unauthorized);
            }

            var comentario = await ObterComentario(id);
            if (comentario == null)
            {
                return CustomResponse(HttpStatusCode.NotFound);
            }
           
            if (!await VerificarPermissao(usuarioId.Value, id))
            {
                NotificarErro("Você não tem permissão para realizar esta ação.");
                return CustomResponse(HttpStatusCode.Forbidden);

            }

            await _comentarioService.Remover(id);
            return CustomResponse(HttpStatusCode.NoContent);

        }
        private async Task<Guid?> ObterUsuarioId()
        {
            var user = await _userManager.GetUserAsync(User);
            return user?.Id;
        }

        private async Task<bool> VerificarPermissao(Guid usuarioId, Guid postId)
        {
            var post = await _comentarioService.ObterPorId(postId);
            return post != null && (post.AutorId == usuarioId || User.IsInRole("Admin"));
        }

        private async Task<ComentarioViewModel> ObterComentario(Guid id)
        {
            return _mapper.Map<ComentarioViewModel>(await _comentarioRepository.ObterComentarioPost(id));
        }
    }
}
