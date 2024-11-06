using System.ComponentModel.DataAnnotations;

namespace Blog.App.Models
{
    public class AutorViewModel
    {
        public Guid Id { get; set; }
        
        [Display(Name = "Autor")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string? NomeAutor { get; set; }
        public string? UserId { get; set; }

    }
}
