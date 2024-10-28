using AutoMapper;
using Blog.Api.ViewModels;
using Blog.Core.Interfaces;
using Blog.Core.Models;
using Blog.Core.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    [Route("comentario")]
    public class ComentarioController : BaseController
    {
        private IComentarioRepository _comentarioRepository;
        private IMapper _mapper;

        public ComentarioController(IComentarioRepository comentarioRepository,
                                    IMapper mapper, 
                                    INotificador notificador) : base(notificador)
        {
            _comentarioRepository = comentarioRepository;
            _mapper = mapper;
        }
        
        [HttpGet("{postId}")]
        public async Task<IActionResult> Index(Guid postId) 
        {
            var comentariosViewModel = _mapper.Map<IEnumerable<ComentarioViewModel>>(await _comentarioRepository.ObterComentariosPorPost(postId));
            
            ViewBag.PostId = postId;
            
            return View(comentariosViewModel);
        }
        [HttpPost("{postId}")]
        public async Task<IActionResult> Adicionar(ComentarioViewModel comentarioViewModel)
        {
            if (!ModelState.IsValid) return View("Index", comentarioViewModel);

            await _comentarioRepository.Adicionar(_mapper.Map<Comentario>(comentarioViewModel));

            return RedirectToAction("Index", new {postId = comentarioViewModel.PostId});
        }
    }
}
