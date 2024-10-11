using Blog.Data.Models.Base;

namespace Blog.Data.Models
{
    public class Post : Entity 
    {
        public required string Titulo { get; set; }
        public string? Conteudo { get; set; }
        public DateTime DataCadastro { get; set; }
        public required Autor Autor { get; set; }
        public required ICollection<Comentario> Comentarios { get; set; }
    }
}
