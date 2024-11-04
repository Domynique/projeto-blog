namespace Blog.Core.Business.Interfaces
{
    public interface IAppUser
    {
        string GetUserId();
        bool IsAuthenticated();
        bool IsAdmin();
        bool BusinessRule(string user);

        
    }
}
