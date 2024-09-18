using BusinessLogic.IServices;
using Domain;
using Domain.Exceptions.GeneralExceptions;
using IDataAccess;

namespace BusinessLogic.Services;

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
        User existingUser = GetBy(u => u.Email == user.Email);
        if (existingUser != null)
        {
            throw new ElementAlreadyExist(UserAlreadyExistExceptionMessage);
        }
        _userRepository.Add(user);
    }

    private User? GetBy(Func<User, bool> predicate)
    {
        return _userRepository.GetByFilter(predicate).FirstOrDefault();
    }

    public List<User> GetAllUsers()
    {
        return _userRepository.GetAll(); 
    }

    public bool IsAdmin(string email)
    {
        User existingUser = GetBy(u => u.Email == email);
        if (existingUser == null)
        {
            throw new ElementNotFound(UserDoesNotExistExceptionMessage);
        }
        bool hasAdministrator = existingUser.Roles.Any(role => role is Administrator);
        return hasAdministrator; 
    }
}