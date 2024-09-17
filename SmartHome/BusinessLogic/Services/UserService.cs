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

    
}