using Microsoft.AspNetCore.Identity;

namespace Blog.Core.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public Guid AutorId { get; set; }
        public Autor Autor { get; set; }
    }
}
