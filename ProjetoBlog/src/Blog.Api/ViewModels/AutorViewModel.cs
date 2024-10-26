using System.ComponentModel.DataAnnotations;

namespace Blog.Api.ViewModels
{
    public class AutorViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string? Nome { get; set; }

        [StringLength(1000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string? Biografia { get; set; }

        public List<PostViewModel> Posts { get; set; }
        public List<ComentarioViewModel> Comentarios { get; set; }
    }
}
