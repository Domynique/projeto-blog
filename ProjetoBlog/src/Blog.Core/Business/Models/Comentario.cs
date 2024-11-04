using Blog.Core.Business.Models.Base;
using Microsoft.AspNetCore.Identity;

namespace Blog.Core.Business.Models
{
    public class Comentario : Entity
    {
        public string? Conteudo { get; set; }
        public Guid PostId { get; set; }
        public Post? Post { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public Comentario()
        {
            UserId = string.Empty;
            User = new IdentityUser();
        }

        public Comentario(string userId, IdentityUser user)
        {
           UserId = userId;
           User = user;
        }



    }
}
