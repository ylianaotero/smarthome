using CustomExceptions;
using Domain.Abstract;
using Domain.Concrete;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class SessionService(IRepository<User> userRepository, IRepository<Session> sessionRepository)
    : ISessionService
{
    private const string UserDoesNotExistExceptionMessage = "Member not found";
    private const string SessionDoesNotExistExceptionMessage = "Member not found";

    public Session LogIn(string email, string password)
    {
        User user = userRepository
            .GetByFilter(u => u.Email == email && u.Password == password, PageData.Default)
            .FirstOrDefault();
        
        if (user == null)
        {
            throw new CannotFindItemInList(UserDoesNotExistExceptionMessage); 
        }

        Session newSession = new Session();
        newSession.User = user; 
        
        sessionRepository.Add(newSession);
        
        return newSession; 
    }

    public void LogOut(Guid token)
    {
        Session session = sessionRepository.GetByFilter(s => s.Id == token, PageData.Default).FirstOrDefault();

        if (session == null)
        {
            throw new CannotFindItemInList(SessionDoesNotExistExceptionMessage); 
        }

        sessionRepository.Delete(session);
    }

    public User GetUser(Guid token)
    {
        Session session = sessionRepository.GetByFilter(s => s.Id == token, null).FirstOrDefault();

        if (session == null)
        {
            throw new CannotFindItemInList(SessionDoesNotExistExceptionMessage); 
        }

        return session.User; 
    }
    
    public bool UserHasCorrectRole(Guid? authorization, string roleWithPermissions)
    {
        return GetUser(authorization.Value).Roles.Exists(r => RoleIsAdequate(r, roleWithPermissions));
    }
    
    public bool AuthorizationIsValid(Guid? authorization)
    {
        return authorization != null && UserIsAuthenticated(authorization.Value);
    }

    public bool UserCanListDevicesInHome(Guid token, Home home)
    {
        return UserHasPermissionInHomeOrIsOwner(token, home, m => m.HasPermissionToListDevices);
    }

    public bool UserCanAddDevicesInHome(Guid token, Home home)
    {
        return UserHasPermissionInHomeOrIsOwner(token, home, m => m.HasPermissionToAddADevice);
    }
    
    private bool UserHasPermissionInHomeOrIsOwner(Guid token, Home home, Func<Member, bool> permissionCheck)
    {
        User user = GetUser(token);
        bool isOwner = home.Owner == user;
        if (isOwner)
        {
            return true;
        }

        bool hasPermission = home.Members.Exists(m => m.User == user && permissionCheck(m));

        return hasPermission;
    }
    
    private bool RoleIsAdequate(Role role, string roleWithPermissions)
    {
        return role.Kind == roleWithPermissions;
    }
    
    private bool UserIsAuthenticated(Guid authorization)
    {
        Session session = sessionRepository
            .GetByFilter(s => s.Id == authorization, PageData.Default)
            .FirstOrDefault();
        
        return session != null && session.User != null;
    }
}