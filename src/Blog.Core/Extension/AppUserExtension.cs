using Blog.Core.Business.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Blog.Core.Extension
{
    public class AppUserExtension : IAppUser
    {
        private readonly IHttpContextAccessor _accessor;

        public AppUserExtension(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string GetUserId()
        {
            return IsAuthenticated() ? _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value : string.Empty;
        }

        public bool IsAdmin()
        {
            return _accessor.HttpContext?.User.IsInRole("Admin") ?? false;
        }

        public bool IsAuthenticated()
        {
            return _accessor.HttpContext?.User.Identity is { IsAuthenticated: true };
        }

        public bool IsUserAuthorize(string? user)
        {

            return  IsAdmin() || user == GetUserId();

        }
    }
}
