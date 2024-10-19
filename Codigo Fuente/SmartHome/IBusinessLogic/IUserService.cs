using Domain.Abstract;
using Domain.Concrete;
using IDataAccess;

namespace IBusinessLogic;

public interface IUserService
{
    void CreateUser(User user);
    List<User> GetUsersByFilter(Func<User, bool> filter, PageData pageData);
    bool IsAdmin(string email);
    void DeleteUser(long id);
    void UpdateUser(long id, User user);
    bool CompanyOwnerIsComplete(long id);
    Company AddOwnerToCompany(long id, Company company);
    void AssignRoleToUser(long userId, Role role);
}