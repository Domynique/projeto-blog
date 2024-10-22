using Blog.Business.Models.Base;
using System.Text.Json.Serialization;

namespace Blog.Business.Models
{
    public class Post : Entity
    {
        public string? Titulo { get; set; }
        public string? Conteudo { get; set; }
        public Guid AutorId { get; set; }
        public DateTime DataCadastro { get; set; }
        [JsonIgnore]
        public Autor? Autor { get; set; }
        [JsonIgnore]
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
