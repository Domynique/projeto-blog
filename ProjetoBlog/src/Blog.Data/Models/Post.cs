using Blog.Data.Models.Base;

namespace Blog.Data.Models
{
    public class Post : Entity
    {
        public string? Titulo { get; set; }
        public string? Conteudo { get; set; }
        public Guid AutorId { get; set; }
        public DateTime DataCadastro { get; set; }
        public Autor? Autor { get; set; }
        public ICollection<Comentario>? Comentarios { get; set; }

        public Post()
        {

        }

        public Post(string titulo, string conteudo, Guid autorId, Autor autor)
        {
            Titulo = titulo;
            Conteudo = conteudo;
            AutorId = autorId;
            DataCadastro = DateTime.Now;
            Autor = autor;
            Comentarios = new List<Comentario>();
        }
    }
}
