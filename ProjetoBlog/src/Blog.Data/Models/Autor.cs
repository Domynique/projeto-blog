namespace Blog.Data.Models
{
    public class Autor : User 
    {
        public required ICollection <Post> Posts { get; set; }
        public required ICollection <Comentario> Comentarios { get; set; }
    }
}
