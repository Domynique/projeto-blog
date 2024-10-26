using Blog.Core.Models.Base;

namespace Blog.Core.Models
{
    public class Autor : Entity
    {
        public string? Nome { get; set; }
        public string? Biografia { get; set; }
        public ICollection<Post>? Posts { get; set; }
        public ICollection<Comentario>?  Comentarios { get; set; }
    }
}
