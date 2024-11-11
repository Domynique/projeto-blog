using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Blog.App.Models
{
    public class ComentarioViewModel
    {
        public Guid Id { get; set; }

        [StringLength(1000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        [DisplayName("Conteúdo")]
        public string? Conteudo { get; set; }
        public string? NomeUsuario{ get; set; }
        public Guid PostId { get; set; }
        public DateTime PublicadoEm { get; set; }
        public bool Autorizado { get; set; }

    }
}