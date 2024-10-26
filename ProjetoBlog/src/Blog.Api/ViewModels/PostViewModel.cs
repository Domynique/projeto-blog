﻿using Blog.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Blog.Api.ViewModels
{
    public class PostViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string? Titulo { get; set; }

        [Required(ErrorMessage = "O conteúdo é obrigatório.")]
        [StringLength(1000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string? Conteudo { get; set; }

        [Required]
        public Guid AutorId { get; set; }

        [Display(Name = "Nome do Autor")]
        public string? NomeAutor { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        public DateTime DataCadastro { get; set; }

        public List<ComentarioViewModel> Comentarios { get; set; }
    }
}