using System.ComponentModel.DataAnnotations;

namespace Blog.App.Models
{
    public class ComentarioViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(1000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string? Conteudo { get; set; }
  
        [Display(Name = "Autor")]
        public string? NomeAutor { get; set; }

        [Required]
        public Guid PostId { get; set; }
           
        public DateTime PublicadoEm { get; set; }

    }
}