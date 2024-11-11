namespace Blog.Core.Business.Interfaces
{
    public interface IAppUser
    {
        public string GetUserId();
        bool IsAuthenticated();
        public bool IsAdmin();
        public bool IsUserAuthorize(string user);

        
    }
}
