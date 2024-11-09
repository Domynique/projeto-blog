using AutoMapper;
using Blog.App.Models;
using Blog.Core.Business.Interfaces;
using Blog.Core.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.App.Controllers
{
    [Route("{postId:Guid}/comentarios")]
    public class ComentarioController : BaseController
    {

        private readonly IComentarioRepository _comentarioRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        private readonly IAppUser _user;

        public ComentarioController(IComentarioRepository comentarioRepository,
                                    IPostRepository postRepository,
                                    IMapper mapper,
                                    INotificador notificador, IAppUser appUser) : base(notificador, appUser)
        {
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

            return PartialView("_ComentarioPartial", comentarioViewModel);
        }

        [Authorize]
        [HttpPost("adicionar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ComentarioViewModel comentarioViewModel, Guid postId)
        {        
            
            if (!ModelState.IsValid)
            {
                return PartialView("_ComentarioPartial", comentarioViewModel);
            }

            var post = await _postRepository.ObterPorId(postId);

            if (post == null)
            {
                return RedirectToAction("Index", "Error", new { statusCode = 404 });
            }
            
            var postViewModel = ObterAutorizacao(post, _user);
           
            await _comentarioRepository.Adicionar(_mapper.Map<Comentario>(comentarioViewModel));

            ViewBag.Autorizado = postViewModel.Autorizado;
            

            return PartialView("_ComentarioListaPartial", _mapper.Map<IEnumerable<ComentarioViewModel>>(postViewModel.Comentarios));
        }

        [Authorize]
        [HttpGet("editar/{id:Guid}")]
        public async Task<IActionResult> Edit(Guid id, Guid postId)
        {
            var comentario = await _comentarioRepository.ObterComentarioPorPost(id, postId);

            var validationStatusCode = ValidarComentario(comentario);

            if (validationStatusCode.HasValue)
            {
                return RedirectToAction("Index", "Error", new { statusCode = validationStatusCode.Value });
            }

            var comentarioViewModel = _mapper.Map<ComentarioViewModel>(comentario);

            return PartialView("_ComentarioPartial", comentarioViewModel);
        }

        [Authorize]
        [HttpPost("editar/{id:Guid}")]
        public async Task<IActionResult> Edit(Guid id, ComentarioViewModel comentarioViewModel, Guid postId)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_ComentarioPartial", comentarioViewModel);
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
            
            var comentarios = _mapper.Map<IEnumerable<ComentarioViewModel>>(await _comentarioRepository.ObterComentariosPorPost(comentario.PostId));
           
            var postViewModel = ObterAutorizacao(comentario.Post, _user);

            ViewBag.Autorizado = postViewModel.Autorizado;          
           
            return PartialView("_ComentarioListaPartial", comentarios);
        }

        [Authorize]
        [HttpPost("excluir/{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id, Guid postId)
        {
            var comentario = await _comentarioRepository.ObterComentarioPorPost(id, postId);

            var validationStatusCode = ValidarComentario(comentario);

            if (validationStatusCode.HasValue)
            {
                return RedirectToAction("Index", "Error", new { statusCode = validationStatusCode.Value });
            }

            await _comentarioRepository.Remover(id);
            
            var comentarios = await _comentarioRepository.ObterComentariosPorPost(comentario.PostId);

            var postViewModel = ObterAutorizacao(comentario.Post, _user);

            ViewBag.Autorizado = postViewModel.Autorizado;
          
            return PartialView("_ComentarioPartial", comentarios);
        }

        private int? ValidarComentario(Comentario comentario) 
        { 
            if (comentario == null) return 404;
            
            var autorizado = ValidarPermissao(comentario.Post!.Autor!.UserId); 
            
            if (!autorizado) return 403;
            
            return null; 
        }


        private PostViewModel ObterAutorizacao(Post? post, IAppUser appUser)
        {
            var postViewModel = _mapper.Map<PostViewModel>(post);
            
            postViewModel.Autorizado = appUser.IsUserAuthorize(postViewModel.Autor!.UserId!);

            return postViewModel;
        }
    }
}
