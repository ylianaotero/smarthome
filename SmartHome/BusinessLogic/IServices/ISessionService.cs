using Domain;

namespace BusinessLogic.IServices;

public interface ISessionService
{
    Session LogIn(string email, string password);
    void Logout(Guid token);
}