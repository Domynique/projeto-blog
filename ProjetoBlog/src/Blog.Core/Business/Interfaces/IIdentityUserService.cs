using Microsoft.AspNetCore.Identity;

namespace Blog.Core.Business.Interfaces
{
    public interface IIdentityUserService : IDisposable
    {
        Task<IdentityUser> GetUserById(string userId);
        Task<IEnumerable<IdentityUser>> GetAllUsers(); 
        Task UpdateUser(IdentityUser user);
        Task DeleteUser(string userId);
    }
}
