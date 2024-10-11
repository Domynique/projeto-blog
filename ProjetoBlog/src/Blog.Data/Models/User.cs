using Microsoft.AspNetCore.Identity;

namespace Blog.Data.Models
{
    public class User : IdentityUser<Guid>
    {
        public string? Biografia { get; set; }
    }
}
