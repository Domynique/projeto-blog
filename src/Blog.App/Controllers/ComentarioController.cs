using AutoMapper;
using Blog.App.Models;
using Blog.Core.Business.Interfaces;
using Blog.Core.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Blog.App.Controllers
{
    [Route("/posts/{postId:Guid}/comentarios")]
    public class ComentarioController : BaseController
    {

        private readonly IComentarioService _comentarioService;
        private readonly IComentarioRepository _comentarioRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        private readonly IAppUser _user;

        public ComentarioController(IComentarioService comentarioService,
                                    IComentarioRepository comentarioRepository,
                                    IPostRepository postRepository,
                                    IMapper mapper,
                                    INotificador notificador, IAppUser appUser) : base(notificador, appUser)
        {
            _comentarioService = comentarioService;
            _comentarioRepository = comentarioRepository;
            _postRepository = postRepository;
            _mapper = mapper;
            _user = appUser;
        }

        [HttpGet("detalhes/{id:Guid}")]
        public async Task<IActionResult> Index(Guid id, Guid postId)
        {
            var comentario = await _comentarioRepository.ObterComentarioPorPost(id, postId);

            var validationStatusCode = ValidarComentario(comentario); 
            
            if (validationStatusCode.HasValue) 
            { 
                return RedirectToAction("Index", "Error", new { statusCode = validationStatusCode.Value }); 
            }

            var postViewModel = ObterAutorizacao(comentario.Post, _user);

            ViewBag.Autorizado = postViewModel.Autorizado;

            var comentarioViewModel = _mapper.Map<ComentarioViewModel>(comentario);

            return PartialView("_ComentarioForm", comentarioViewModel);
        }

        [Authorize]
        [HttpPost("adicionar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ComentarioViewModel comentarioViewModel, Guid postId)
        {        
            
            if (!ModelState.IsValid)
            {
                return PartialView("_ComentarioForm", comentarioViewModel);
            }

            var post = await _postRepository.ObterPostPorId(postId);

            if (post == null)
            {
                return RedirectToAction("Index", "Error", new { statusCode = 404 });
            }
            
            var postViewModel = ObterAutorizacao(post, _user);

            await _comentarioService.Adicionar(_mapper.Map<Comentario>(comentarioViewModel));
           
            var comentariosViewModel = _mapper.Map<IEnumerable<ComentarioViewModel>>(post.Comentarios);

            ViewBag.Autorizado = postViewModel.Autorizado;
            
            return PartialView("_ComentarioList", comentariosViewModel);
        }

        [Authorize]
        [HttpGet("editar/{id:Guid}")]
        public async Task<IActionResult> Edit(Guid id, Guid postId)
        {
            var comentario = await _comentarioRepository.ObterComentarioPorPost(id, postId);
            var post = await _postRepository.ObterPostPorId(postId);

            if (comentario == null || post == null) 
            {
                return RedirectToAction("Index", "Error", new { statusCode = 404 });
            }

            var validationStatusCode = ValidarComentario(comentario);

            if (validationStatusCode.HasValue)
            {
                return RedirectToAction("Index", "Error", new { statusCode = validationStatusCode.Value });
            }

            var comentarioViewModel = _mapper.Map<ComentarioViewModel>(comentario);
            var postViewModel = _mapper.Map<PostViewModel>(post);
            var comentarios = await _comentarioRepository.ObterComentariosPorPost(postId);

            var comentariosViewModel = _mapper.Map<IEnumerable<ComentarioViewModel>>(comentarios);

            var postComentarioViewModel = new PostComentarioViewModel
            {
                Post = postViewModel,
                Comentarios = comentariosViewModel,
                Comentario = comentarioViewModel
            };

            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest") 
            { 
                return PartialView("_ComentarioForm", comentarioViewModel); 
            }
            
            return View(postComentarioViewModel);
        }

        [Authorize]
        [HttpPost("editar/{id:Guid}")]
        public async Task<IActionResult> Edit(Guid id, ComentarioViewModel comentarioViewModel, Guid postId)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_ComentarioForm", comentarioViewModel);
            }
            
            if (id != comentarioViewModel.Id || postId != comentarioViewModel.PostId)
            {
                return RedirectToAction("Index", "Error", new { statusCode = 404 });
            }
          
            var comentario = await _comentarioRepository.ObterComentarioPorPost(id, postId);

            var validationStatusCode = ValidarComentario(comentario); 
            
            if (validationStatusCode.HasValue) 
            { 
                return RedirectToAction("Index", "Error", new { statusCode = validationStatusCode.Value }); 
            }

            comentario.Conteudo = comentarioViewModel.Conteudo;

            await _comentarioRepository.Atualizar(comentario);

            var comentariosAtualizados = await _comentarioRepository.ObterComentariosPorPost(comentario.PostId);
            var comentariosViewModel = _mapper.Map<IEnumerable<ComentarioViewModel>>(comentariosAtualizados);

            var postViewModel = ObterAutorizacao(comentario.Post, _user);

            ViewBag.Autorizado = postViewModel.Autorizado;          
           
            return PartialView("_ComentarioList", comentariosViewModel);
        }

        [Authorize]
        [HttpGet("excluir/{id:Guid}")] 
        public async Task<IActionResult> Delete(Guid id, Guid postId) 
        { 
            var comentario = await _comentarioRepository.ObterComentarioPorPost(id, postId); 
            var validationStatusCode = ValidarComentario(comentario); 
            if (validationStatusCode.HasValue) 
            { 
                return RedirectToAction("Index", "Error", new { statusCode = validationStatusCode.Value }); 
            } 
            
            var comentarioViewModel = _mapper.Map<ComentarioViewModel>(comentario); 
            
            return View(comentarioViewModel); 
        }

        [Authorize]
        [HttpPost("excluir/{id:Guid}")]
        public async Task<IActionResult> DeleteConfirmed(Guid id, Guid postId)
        {
            var comentario = await _comentarioRepository.ObterComentarioPorPost(id, postId);

            var validationStatusCode = ValidarComentario(comentario);

            if (validationStatusCode.HasValue)
            {
                return RedirectToAction("Index", "Error", new { statusCode = validationStatusCode.Value });
            }

            await _comentarioRepository.Remover(id);

            var comentariosAtualizados = await _comentarioRepository.ObterComentariosPorPost(comentario.PostId); 
            var comentariosViewModel = _mapper.Map<List<ComentarioViewModel>>(comentariosAtualizados);
            
            var postViewModel = ObterAutorizacao(comentario.Post, _user);
            
            ViewBag.Autorizado = postViewModel.Autorizado;

            return RedirectToAction("Index", "Post");
        }


        private int? ValidarComentario(Comentario comentario) 
        { 
            if (comentario == null) return 404;
            
            var autorizado = ValidarPermissao(comentario.Post!.Autor!.UsuarioId); 
            
            if (!autorizado) return 403;
            
            return null; 
        }


        private PostViewModel ObterAutorizacao(Post? post, IAppUser appUser)
        {
            var postViewModel = _mapper.Map<PostViewModel>(post);
            
            postViewModel.Autorizado = appUser.IsUserAuthorize(postViewModel.Autor!.UsuarioId!);

            return postViewModel;
        }
    }
}
