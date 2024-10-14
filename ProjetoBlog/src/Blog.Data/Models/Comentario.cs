using Blog.Data.Models.Base;

namespace Blog.Data.Models
{
    public class Comentario : Entity
    {
        public string? Conteudo { get; set; }
        public Guid AutorId { get; set; }
        public DateTime DataCadastro { get; set; }
        public Autor? Autor { get; set; }
        public Post? Post { get; set; }

        public Comentario()
        {

        }
        public Comentario(string conteudo, Guid autorId, Autor autor, Post post)
        {
            Conteudo = conteudo;
            DataCadastro = DateTime.Now;
            AutorId = autorId;
            Autor = autor;
            Post = post;
        }



    }
}
