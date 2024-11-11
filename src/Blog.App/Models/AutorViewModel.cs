using System.ComponentModel.DataAnnotations;

namespace Blog.App.Models
{
    public class AutorViewModel
    {
        public Guid Id { get; set; }       
        public string? Nome { get; set; }
        public string? UsuarioId { get; set; }

    }
}
