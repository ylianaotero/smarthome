using Domain;
using IDataAccess;

namespace IBusinessLogic;

public interface IUserService
{
    void CreateUser(User user);
    
    List<User> GetAllUsers(PageData pageData);
    
    bool IsAdmin(string email);
    
    void DeleteUser(long id);
    void UpdateUser(long id, User user);
}