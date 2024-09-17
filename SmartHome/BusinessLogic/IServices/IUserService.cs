using Domain;

namespace BusinessLogic.IServices;

public interface IUserService
{
    void CreateUser(User user);
    
    List<User> GetAllUsers();
}