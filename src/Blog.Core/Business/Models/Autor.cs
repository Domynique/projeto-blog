using Blog.Core.Business.Models.Base;
using Microsoft.AspNetCore.Identity;

namespace Blog.Core.Business.Models
{
    public class Autor : Entity
    {
        public string UsuarioId { get; set; }
        public IdentityUser Usuario { get; set; }
    }
}
