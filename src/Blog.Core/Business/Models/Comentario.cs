using Blog.Core.Business.Models.Base;
using Microsoft.AspNetCore.Identity;

namespace Blog.Core.Business.Models
{
    public class Comentario : Entity
    {
        public string? Conteudo { get; set; }
        public Guid PostId { get; set; }
        public Post? Post { get; set; }
        public string UsuarioId { get; set; }
        public IdentityUser Usuario { get; set; }


    }
}
