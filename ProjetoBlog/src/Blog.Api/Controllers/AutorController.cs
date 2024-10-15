using Blog.Data.Models;
using Blog.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers
{
    
    
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AutorController : ControllerBase
    {
        private readonly IAutorService _autorService;        

        public AutorController(IAutorService autorService)
        {
            _autorService = autorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Autor>>> GetAutores()
        {
            var autores = await _autorService.ObterTodos();
            return Ok(autores);
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<Autor>> GetAutor(Guid id) 
        { 
            var autor = await _autorService.ObterPorId(id);

            if (autor == null) return NotFound();

            return Ok(autor);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostAutor(Autor autor) 
        { 

            if (ModelState.IsValid)
            {
                var nvautor = new Autor
                {
                    UserName = autor.UserName,
                    Email = autor.Email,
                    Nome = autor.Nome,
                    Biografia = autor.Biografia
                };
                await _autorService.Adicionar(nvautor);
                return CreatedAtAction(nameof(GetAutor), new { id = autor.Id }, autor);
            }
            
            return ValidationProblem(new ValidationProblemDetails(ModelState)
            {
                Title = "Um ou mais erros de validação ocorreram!"
            });

        }


    }
}
