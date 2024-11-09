using System.Net;
using AutoMapper;
using Blog.Api.ViewModels;
using Blog.Core.Business.Interfaces;
using Blog.Core.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Blog.Api.Controllers
{
    [ApiController]
    [Route("api/Posts")]
    public class PostController : MainController
    {
        private readonly IPostRepository _postRepository;      
        private readonly IPostService _postService;
        private readonly IComentarioService _comentarioService;
        private readonly IMapper _mapper;

        public PostController(IPostRepository postRepository,
                              IPostService postService,
                              IComentarioService comentarioService,
                              IMapper mapper,
                              INotificador notificador,
                              IAppUser appUser) : base(notificador, appUser)
        {
            _postRepository = postRepository;
            _postService = postService;
            _comentarioService = comentarioService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PostViewModel>>> ObterTodos()
        {
            var posts = _mapper.Map<IEnumerable<PostViewModel>>(await _postRepository.ObterTodos());
            return CustomResponse(HttpStatusCode.OK, posts);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PostViewModel>> ObterPorId(Guid id)
        {
            var post = await _postRepository.ObterPorId(id);
            if (post == null) 
            {
                NotificarErro("Post não encontrado.");
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

            var novoPost = _mapper.Map<Post>(postViewModel);

            await _postService.Adicionar(novoPost);

            return CustomResponse(HttpStatusCode.Created);

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

            if (id != postViewModel.Id)
            {
                NotificarErro("IDs não conferem.");
                return CustomResponse();
            }

            var post = await _postRepository.ObterPorId(id);
            
            if (post == null)
            {
                NotificarErro("Post não encontrado.");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            var autorizado = _appUser.IsUserAuthorize(post.Autor!.UserId);

            if (!autorizado) 
            {
                NotificarErro("Autorização necessária.");
                return CustomResponse(HttpStatusCode.Unauthorized);
            }

            post.Titulo = postViewModel.Titulo;
            post.Conteudo = postViewModel.Conteudo;

            await _postService.Atualizar(post);

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

            var post = await _postRepository.ObterPorId(id);

            if (post == null)
            {
                NotificarErro("Post não encontrado.");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            var autorizado = _appUser.IsUserAuthorize(post.Autor!.UserId);

            if (!autorizado)
            {
                NotificarErro("Autorização necessária.");
                return CustomResponse(HttpStatusCode.Unauthorized);
            }

            await _postService.Remover(id);

            return CustomResponse(HttpStatusCode.NoContent);

        }

    }
}
