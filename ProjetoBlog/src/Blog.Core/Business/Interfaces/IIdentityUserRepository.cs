using Microsoft.AspNetCore.Identity;

namespace Blog.Core.Business.Interfaces
{
    public interface IIdentityUserRepository : IDisposable
    {
        Task<IdentityUser> FindById(string userId); 
        Task<IEnumerable<IdentityUser>> GetAllUsers(); 
        Task UpdateUser(IdentityUser user);
        Task DeleteUser(string userId);
    }
}
