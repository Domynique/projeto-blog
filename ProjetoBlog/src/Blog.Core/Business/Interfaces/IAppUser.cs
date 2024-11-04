namespace Blog.Core.Business.Interfaces
{
    public interface IAppUser
    {
        string GetUserId();
        bool IsAuthenticated();
        bool IsAdmin();
        bool IsInBusinessRule(string user);

        
    }
}
