namespace Blog.App.Models
{
    public class PostComentarioViewModel
    {
        public PostViewModel Post { get; set; }
        public IEnumerable<ComentarioViewModel> Comentarios { get; set; }
        public ComentarioViewModel Comentario { get; set; }
    }

}
