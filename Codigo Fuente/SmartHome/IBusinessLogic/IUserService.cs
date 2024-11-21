using Domain.Concrete;
using IDataAccess;

namespace IBusinessLogic;

public interface IUserService
{
    long CreateUser(User user);
    List<User> GetUsersByFilter(Func<User, bool> filter, PageData pageData);
    User GetUserById(long id);
    bool IsAdmin(string email);
    void DeleteUser(long id);
    void UpdateUser(long id, User user);
    bool CompanyOwnerIsComplete(long id);
    Company AddOwnerToCompany(long id, Company company);
    User AssignRoleToUser(long userId, string roleType);
}