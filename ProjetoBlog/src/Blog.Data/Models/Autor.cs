using Microsoft.AspNetCore.Identity;

namespace Blog.Data.Models
{
    public class Autor : IdentityUser<Guid>
    {
        public string? Nome { get; set; }
        public string? Biografia { get; set; }
        public ICollection<Post>? Posts { get; set; }
        public ICollection<Comentario>?  Comentarios { get; set; }
    }
}
