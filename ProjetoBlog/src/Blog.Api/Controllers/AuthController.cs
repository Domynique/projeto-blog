using Blog.Api.Models;
using Blog.Data.Models;
using Blog.Data.Notifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;

namespace Blog.Api.Controllers
{


    [ApiController]
    [Route("api/Conta")]
    public class AuthController : MainController
    {
        private readonly UserManager<Autor> _userManager;
        private readonly SignInManager<Autor> _signInManager;
        private readonly JwtSettings _jwtSettings;

        public AuthController(UserManager<Autor> userManager,
                              SignInManager<Autor> signInManager,
                              IOptions<JwtSettings> jwtSettings, INotificador notificador) : base(notificador)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel registerAutor)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var autor = new Autor
            {
                Nome = registerAutor.Nome,
                Biografia = registerAutor.Biografia,
                UserName = registerAutor.Email,
                Email = registerAutor.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(autor, registerAutor.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    NotificarErro(error.Description);
                }
                return CustomResponse(HttpStatusCode.BadRequest);
            }

            var token = GerarJwt();

            return Ok(new { token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel loginAutor)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var result = await _signInManager.PasswordSignInAsync(loginAutor.Email, loginAutor.Password, false, false);
            if (!result.Succeeded)
            {
                NotificarErro("Autenticação falhou.");
                return CustomResponse(HttpStatusCode.Unauthorized);
            }
  
            var token = GerarJwt();

            return Ok(new { token });

        }

        private string GerarJwt()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Segredo);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Emissor,
                Audience = _jwtSettings.Audiencia,
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            return encodedToken;
        }

    }
}