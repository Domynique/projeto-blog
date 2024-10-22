using Blog.Business.Models.Base;

namespace Blog.Business.Models
{
    public class Comentario : Entity
    {
        public string? Conteudo { get; set; }
        public Guid AutorId { get; set; }
        public Guid PostId { get; set; }
        public DateTime DataCadastro { get; set; }
        public Autor? Autor { get; set; }
        public Post? Post { get; set; }

        public Comentario()
        {

        }
        public Comentario(string conteudo, Guid autorId, Guid postId, Autor autor, Post post)
        {
            Conteudo = conteudo;
            DataCadastro = DateTime.Now;
            AutorId = autorId;
            PostId = postId;
            Autor = autor;
            Post = post;
        }



    }
}
