using Blog.Api.Models;
using Blog.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<Autor> _userManager;
        private readonly SignInManager<Autor> _signInManager;

        public AuthController(UserManager<Autor> userManager, SignInManager<Autor> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        [ProducesDefaultResponseType(typeof(void))]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var autor = new Autor
            {
                UserName = model.Email,
                Email = model.Email,
                Nome = model.Nome,
                Biografia = model.Biografia
            };

            var result = await _userManager.CreateAsync(autor, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }

        [HttpPost("login")]
        [ProducesDefaultResponseType(typeof(void))]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            if (!result.Succeeded)
            {
                return Unauthorized();
            }

            // Gerar e retornar JWT ou autenticar sessão (dependendo da configuração)

            return Ok();
        }
    }
}
