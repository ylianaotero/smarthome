using CustomExceptions;
using Domain;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class SessionService : ISessionService
{
    private const string UserDoesNotExistExceptionMessage = "User not found";
    private const string SessionDoesNotExistExceptionMessage = "User not found";
    
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Session> _sessionRepository;
    
    public SessionService(IRepository<User> userRepository, IRepository<Session> sessionRepository)
    {
        _userRepository = userRepository;
        _sessionRepository = sessionRepository;
    }


    public Session LogIn(string email, string password)
    {
        User user = _userRepository.GetByFilter(u => u.Email == email && u.Password == password).FirstOrDefault();
        
        if (user == null)
        {
            throw new CannotFindItemInList(UserDoesNotExistExceptionMessage); 
        }

        Session newSession = new Session();
        newSession.User = user; 
        _sessionRepository.Add(newSession);
        return newSession; 
    }

    public void LogOut(Guid token)
    {
        Session session = _sessionRepository.GetByFilter(s => s.Id == token).FirstOrDefault();

        if (session == null)
        {
            throw new CannotFindItemInList(SessionDoesNotExistExceptionMessage); 
        }

        _sessionRepository.Delete(session);
    }

    public User GetUser(Guid token)
    {
        Session session = _sessionRepository.GetByFilter(s => s.Id == token).FirstOrDefault();

        if (session == null)
        {
            throw new CannotFindItemInList(SessionDoesNotExistExceptionMessage); 
        }

        return session.User; 
    }
    
    public bool UserHasPermissions(Guid? authorization, string roleWithPermissions)
    {
        return GetUser(authorization.Value).Roles.Exists(r => RoleIsAdequate(r, roleWithPermissions));
    }
    
    public bool AuthorizationIsValid(Guid? authorization)
    {
        return authorization != null && UserIsAuthenticated(authorization.Value);
    } 
    
    private bool RoleIsAdequate(Role role, string roleWithPermissions)
    {
        return role.GetType().Name == roleWithPermissions;
    }
    
    private bool UserIsAuthenticated(Guid authorization)
    {
        Session session = _sessionRepository.GetByFilter(s => s.Id == authorization).FirstOrDefault();
        return session != null && session.User != null;
    }
}