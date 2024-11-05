using Blog.Core.Business.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Blog.Core.Business.Services
{
    public class IdentityUserService : IIdentityUserService
    {
        private readonly IIdentityUserRepository _identityUserRepository;

        public IdentityUserService(IIdentityUserRepository identityUserRepository)
        {
            _identityUserRepository = identityUserRepository;
        }
        public async Task<IdentityUser> GetUserById(string userId)
        {
           return await _identityUserRepository.FindById(userId);
        }

        public async Task<IEnumerable<IdentityUser>> GetAllUsers()
        {
            return await _identityUserRepository.GetAllUsers();
        }

        public async Task UpdateUser(IdentityUser user)
        {
            await _identityUserRepository.UpdateUser(user);
        }        

        public async Task DeleteUser(string userId)
        {
            await _identityUserRepository.DeleteUser(userId);
        }
        
        public void Dispose()
        {
            _identityUserRepository?.Dispose();
        }

    }
}
