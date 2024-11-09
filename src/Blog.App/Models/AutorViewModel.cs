using System.ComponentModel.DataAnnotations;

namespace Blog.App.Models
{
    public class AutorViewModel
    {
        public Guid Id { get; set; }       
        public string? NomeAutor { get; set; }
        public string? UserId { get; set; }

    }
}
