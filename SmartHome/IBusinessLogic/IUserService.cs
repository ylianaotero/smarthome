using Domain;
using IDataAccess;

namespace IBusinessLogic;

public interface IUserService
{
    void CreateUser(User user);
    
    List<User> GetAllUsers(PageData pageData);
    
    List<User> GetUsersByFilter(Func<User, bool> filter, PageData pageData);
    
    bool IsAdmin(string email);
    
    void DeleteUser(long id);
    void UpdateUser(long id, User user);

    bool CompanyOwnerIsComplete(long id);

    Company AddOwnerToCompany(long id, Company company); 
}