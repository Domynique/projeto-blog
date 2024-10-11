using Blog.Data.Models.Base;

namespace Blog.Data.Models
{
    public class Comentario : Entity 
    {
        public string? Conteudo { get; set; }
        public DateTime DataCadastro { get; set; }
        public required User Autor { get; set; }
        public required Post Post { get; set; }


    }
}
