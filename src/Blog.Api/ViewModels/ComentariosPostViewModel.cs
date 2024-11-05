namespace Blog.Api.ViewModels
{
    public class ComentariosPostViewModel
    {
        public PostViewModel Post { get; set; }
        public IEnumerable<ComentarioViewModel> Comentarios { get; set; }
    }
}
