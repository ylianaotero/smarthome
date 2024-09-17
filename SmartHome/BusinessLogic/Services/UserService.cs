using BusinessLogic.IServices;
using Domain;
using IDataAccess;

namespace BusinessLogic.Services;

public class UserService : IUserService
{
    private IRepository<User> _userRepository; 
    
    public UserService(IRepository<User> userRepository)
    {
        _userRepository = userRepository; 
    }
    public void CreateUser(User user)
    {
        _userRepository.Add(user);
    }
    
}