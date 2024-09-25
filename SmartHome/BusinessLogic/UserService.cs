using CustomExceptions;
using Domain;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class UserService : IUserService
{
    private const string UserDoesNotExistExceptionMessage = "User not found";
    private const string UserAlreadyExistExceptionMessage = "User already exists";
    
    private IRepository<User> _userRepository;
    private IUserService _userServiceImplementation;

    public UserService(IRepository<User> userRepository)
    {
        _userRepository = userRepository; 
    }
    public void CreateUser(User user)
    {
        try
        {
            GetBy(u => u.Email == user.Email);
            
            throw new ElementAlreadyExist(UserAlreadyExistExceptionMessage);
        }
        catch (ElementNotFound)
        {
            _userRepository.Add(user);
        }
        
    }

    private User GetBy(Func<User, bool> predicate)
    {
        List<User> listOfuser = _userRepository.GetByFilter(predicate); 
        User user = listOfuser.FirstOrDefault();
        
        if (user == null)
        {
            throw new ElementNotFound(UserDoesNotExistExceptionMessage);
        }

        return user; 
    }

    public List<User> GetAllUsers()
    {
        return _userRepository.GetAll(); 
    }

    public bool IsAdmin(string email)
    {
        User existingUser =  GetBy(u => u.Email == email);
        bool hasAdministrator = existingUser.Roles.Any(role => role is Administrator);
        return hasAdministrator; 
    }
}