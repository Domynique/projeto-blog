using Blog.Core.Business.Models.Base;

namespace Blog.Core.Business.Models
{
    public class Post : Entity
    {
        public string? Titulo { get; set; }
        public string? Conteudo { get; set; }
        public Guid AutorId { get; set; }
        public Autor? Autor { get; set; }
        public ICollection<Comentario>? Comentarios { get; set; }

      
    }
}
