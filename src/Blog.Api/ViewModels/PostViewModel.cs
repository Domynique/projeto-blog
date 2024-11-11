using System.ComponentModel.DataAnnotations;

namespace Blog.Api.ViewModels
{
    public class PostViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string? Titulo { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(1000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string? Conteudo { get; set; }
        [Required]
        public AutorViewModel? Autor { get; set; }
        public IEnumerable<ComentarioViewModel>? Comentarios { get; set; }
        public DateTime PublicadoEm { get; set; }

    }
}