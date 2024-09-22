using Domain;

namespace IBusinessLogic;

public interface ISessionService
{
    Session LogIn(string email, string password);
    void LogOut(Guid token);
    
    User GetUser(Guid token);
}