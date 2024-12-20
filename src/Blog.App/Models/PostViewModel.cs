﻿using System.ComponentModel.DataAnnotations;

namespace Blog.App.Models
{
    public class PostViewModel
    {
        public Guid Id { get; set; }

        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string? Titulo { get; set; }

        [StringLength(1000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string? Conteudo { get; set; }
        public DateTime? PublicadoEm { get; set; }        
        public Guid AutorId { get; set; }
        public AutorViewModel? Autor { get; set; }
        public IEnumerable<ComentarioViewModel>? Comentarios { get; set; }
        public bool Autorizado { get; set; }

    }
}