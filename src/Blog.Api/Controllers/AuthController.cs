﻿using Blog.Api.ViewModels;
using Blog.Core.Business.Interfaces;
using Blog.Core.Business.Models;
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly JwtSettings _jwtSettings;

        public AuthController(UserManager<IdentityUser> userManager,
                              SignInManager<IdentityUser> signInManager,
                              IOptions<JwtSettings> jwtSettings, 
                              INotificador notificador,
                              IAppUser appUser) : base(notificador, appUser)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("registrar")]
        public async Task<ActionResult> Registrar(RegisterViewModel registerAutor)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            var autor = new IdentityUser
            {
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
        public async Task<ActionResult> Login(LoginViewModel loginAutor)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            var result = await _signInManager.PasswordSignInAsync(loginAutor.Email!, loginAutor.Password, false, true);
            
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
            
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Segredo!);

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