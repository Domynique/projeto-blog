using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Blog.Api.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Blog.Core.Business.Interfaces;
using Blog.Core.Business.Models;


namespace Blog.Api.Controllers
{
    [Route("api/Posts")]
    [ApiController]
    public class PostController : MainController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPostRepository _postRepository;      
        private readonly IPostService _postService;
        private readonly IComentarioService _comentarioService;
        private readonly IMapper _mapper;

        public PostController(UserManager<ApplicationUser> userManager,        
                              IPostRepository postRepository,
                              IPostService postService,
                              IComentarioService comentarioService,
                              IMapper mapper,
                              INotificador notificador) : base(notificador)
        {
            _postRepository = postRepository;
            _postService = postService;
            _comentarioService = comentarioService;
            _mapper = mapper;
            _userManager = userManager;
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

            var usuarioId = await ObterUsuarioId();
            if (usuarioId == null)
            {
                NotificarErro("Autenticação necessária.");
                return CustomResponse(HttpStatusCode.Unauthorized);
            }

            var novoPost = _mapper.Map<Post>(postViewModel);
            novoPost.AutorId = usuarioId.AutorId;
            //novoPost.DataCadastro = DateTime.Now;           

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

            var usuarioId = await ObterUsuarioId();
            if (usuarioId == null) 
            {
                NotificarErro("Autenticação necessária.");
                return CustomResponse(HttpStatusCode.Unauthorized);
            }

            if (!await VerificarPermissao(usuarioId.AutorId, id)) 
            {
                NotificarErro("Você não tem permissão para realizar esta ação.");
                return CustomResponse(HttpStatusCode.Forbidden);

            }

            var existePost = await _postService.ObterPorId(id);
            
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
            var usuarioId = await ObterUsuarioId();
            if (usuarioId == null)
            {
                NotificarErro("Autenticação necessária.");
                return CustomResponse(HttpStatusCode.Unauthorized);
            }

            if (!await VerificarPermissao(usuarioId.AutorId, id)) 
            {
                NotificarErro("Você não tem permissão para realizar esta ação.");
                return CustomResponse(HttpStatusCode.Forbidden);
            }

            await _comentarioService.RemoverComentariosPorPost(id);
            await _postService.Remover(id);

            return CustomResponse(HttpStatusCode.NoContent);

        }

        [HttpGet("obter-usuario-id")]
        public async Task<ApplicationUser> ObterUsuarioId()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                Console.WriteLine("Usuário não encontrado.");
            }
            else
            {
                Console.WriteLine($"Usuário encontrado: {user.UserName}");
            }
            return user;
        }

        private async Task<bool> VerificarPermissao(Guid usuarioId, Guid postId)
        {
            var post = await _postService.ObterPorId(postId);
            return post != null && (post.AutorId == usuarioId || User.IsInRole("Admin"));
        }

        private async Task<PostViewModel> ObterPost(Guid id)
        {
            return _mapper.Map<PostViewModel>(await _postRepository.ObterPostAutor(id));
        }
    }
}
