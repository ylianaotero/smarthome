using Domain;

namespace IBusinessLogic;

public interface IUserService
{
    void CreateUser(User user);
    
    List<User> GetAllUsers();
    
    bool IsAdmin(string email);
    
    void DeleteUser(long id);
}