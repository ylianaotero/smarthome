using Domain;

namespace IBusinessLogic;

public interface ISessionService
{
    Session LogIn(string email, string password);
    void LogOut(Guid token);
    User GetUser(Guid token);
    bool UserHasCorrectRole(Guid? authorization, string roleWithPermissions);
    bool AuthorizationIsValid(Guid? authorization);
    bool UserCanListDevicesInHome(Guid token, Home home);
    bool UserCanAddDevicesInHome(Guid token, Home home);
}