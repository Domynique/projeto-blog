using Blog.Core.Business.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Core.Data.Repository
{
    public class IdentityUserRepository : IIdentityUserRepository
    {

        private readonly UserManager<IdentityUser> _userManager;

        public IdentityUserRepository(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityUser> FindById(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<IEnumerable<IdentityUser>> GetAllUsers()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task UpdateUser(IdentityUser user)
        {
            await _userManager.UpdateAsync(user);
        }

        public async Task DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId); 
            if (user != null) 
            { 
                await _userManager.DeleteAsync(user); 
            }
        }

        public void Dispose()
        {
            _userManager.Dispose();
        }

    }
}
