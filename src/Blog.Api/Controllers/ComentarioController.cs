using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using AutoMapper;
using Blog.Api.ViewModels;
using Blog.Core.Business.Interfaces;
using Blog.Core.Business.Models;


namespace Blog.Api.Controllers
{
    [Route("api/Comentarios")]
    [ApiController]
    public class ComentarioController : MainController
    {
        private readonly IComentarioRepository _comentarioRepository;
        private readonly IComentarioService _comentarioService;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public ComentarioController(IComentarioRepository comentarioRepository, 
                                    IComentarioService comentarioService,
                                    IPostRepository postRepository,
                                    IMapper mapper, 
                                    INotificador notificador, IAppUser appUser) : base(notificador, appUser)
        {
            _comentarioRepository = comentarioRepository;
            _comentarioService = comentarioService;
            _postRepository = postRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ComentarioViewModel>>> ObterTodos(Guid postId)
        {
            var comentarios = _mapper.Map<IEnumerable<ComentarioViewModel>>(await _comentarioRepository.ObterComentariosPorPost(postId));
            return CustomResponse(HttpStatusCode.OK, comentarios);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ComentarioViewModel>> ObterPorId(Guid id,Guid postId)
        {;
            var comentario = _mapper.Map<ComentarioViewModel>(await _comentarioRepository.ObterComentarioPorPost(id, postId));
            if (comentario == null)
            {
                NotificarErro("Comentário não encontrado.");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            return CustomResponse(HttpStatusCode.OK, comentario);

        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Adicionar(Guid postId, [FromBody]ComentarioViewModel comentarioViewModel)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            if (postId != comentarioViewModel.PostId)
            {
                NotificarErro("IDs não conferem.");
                return CustomResponse();
            }

            var post = await _postRepository.ObterPorId(postId);

            if (post == null)
            {
                NotificarErro("Post não encontrado.");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            await _comentarioService.Adicionar(_mapper.Map<Comentario>(comentarioViewModel));

            return CustomResponse(HttpStatusCode.Created);

        }

        [Authorize]
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Atualizar(Guid id, Guid postId, [FromBody]ComentarioViewModel comentarioViewModel)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            if (postId != comentarioViewModel.PostId)
            {
                NotificarErro("IDs não conferem.");
                return CustomResponse();
            }

            var comentario = await _comentarioRepository.ObterComentarioPorPost(id, postId);
            if (comentario == null)
            {
                NotificarErro("Comentário não encontrado.");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            var autorizado = _appUser.IsUserAuthorize(comentario.Post.Autor.UsuarioId);
            if (!autorizado)
            {
                NotificarErro("Autorização necessária.");
                return CustomResponse(HttpStatusCode.Unauthorized);
            }

            comentario.Conteudo = comentarioViewModel.Conteudo;

            await _comentarioService.Atualizar(comentario);

            return CustomResponse(HttpStatusCode.NoContent);

        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Excluir(Guid id, Guid postId)
        {

            var comentario = await _comentarioRepository.ObterComentarioPorPost(id, postId);
            if (comentario == null)
            {
                NotificarErro("Comentário não encontrado.");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            var autorizado = _appUser.IsUserAuthorize(comentario.Post.Autor.UsuarioId);
            if (!autorizado)
            {
                NotificarErro("Autorização necessária.");
                return CustomResponse(HttpStatusCode.Unauthorized);
            }

            await _comentarioService.Remover(id);
           
            return CustomResponse(HttpStatusCode.NoContent);

        }
      
    }
}
