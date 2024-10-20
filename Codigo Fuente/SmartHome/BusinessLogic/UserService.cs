using CustomExceptions;
using Domain.Abstract;
using Domain.Concrete;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class UserService(IRepository<User> userRepository) : IUserService
{
    private const string UserDoesNotExistExceptionMessage = "Member not found";
    private const string UserAlreadyExistExceptionMessage = "Member already exists";
    private const string RoleNotAssignableExceptionMessage = "Role cannot be assigned to an existing user";
    private const string UserAlreadyHasRoleExceptionMessage = "User already has the role";
    
    private readonly List<string> _assignableRoles = ["homeowner"];

    public void CreateUser(User user)
    {
        try
        {
            GetBy(u => u.Email == user.Email, null);
            
            throw new ElementAlreadyExist(UserAlreadyExistExceptionMessage);
        }
        catch (ElementNotFound)
        {
            userRepository.Add(user);
        }
    }

    public List<User> GetUsersByFilter(Func<User, bool> filter, PageData pageData)
    {
        return userRepository.GetByFilter(filter, pageData);
    }
    
    public User GetUserById(long id)
    {
        User user = userRepository.GetById(id); 
        if (user == null)
        {
            throw new ElementNotFound(UserDoesNotExistExceptionMessage);
        }

        return user; 
    }

    public bool IsAdmin(string email)
    {
        User existingUser =  GetBy(u => u.Email == email, PageData.Default);
        bool hasAdministrator = existingUser.Roles.Exists(role => role is Administrator);
        
        return hasAdministrator; 
    }
    
    public void DeleteUser(long id)
    {
        User user = GetBy(u => u.Id == id, PageData.Default);
        userRepository.Delete(user);
    }
    
    public void UpdateUser(long id, User user)
    {
        try
        {
            User existingUser = GetBy(u => u.Id == id, PageData.Default);
            existingUser.Update(user);
            userRepository.Update(existingUser);
        }
        catch (Exception e)
        {
            throw new ElementNotFound(UserDoesNotExistExceptionMessage);
        }
    }

    public User AssignRoleToUser(long userId, string roleType)
    {
        User existingUser = GetUserById(userId);
        
        Role role = RoleFactory.CreateRole(roleType);
        
        VerifyRoleIsAssignable(roleType);
        VerifyUserDoesNotHaveRoleAlready(existingUser, roleType);
        
        existingUser.Roles.Add(role);
        userRepository.Update(existingUser);
        
        return existingUser;
    }
  
    public Company AddOwnerToCompany(long id, Company company)
    {
        User user = GetUserById(id);
        
        if (!CompanyOwnerIsComplete(id))
        {
            company.Owner = user; 
            List<CompanyOwner> companyOwnerRoles = GetCompanyOwnerRoles(user); 

            CompanyOwner  role = SearchAnIncompleteCompanyOwnerRole(companyOwnerRoles);

            role.Company = company;

            return company; 
        }
        
        throw new ElementNotFound(UserDoesNotExistExceptionMessage);
    }

    public bool CompanyOwnerIsComplete(long id)
    {
        User existingUser =  GetUserById(id);

        List<CompanyOwner> companyOwnerRoles = GetCompanyOwnerRoles(existingUser); 

        CompanyOwner  role = SearchAnIncompleteCompanyOwnerRole(companyOwnerRoles);

        if (role == null)
        {
            return true; 
        }

        return false; 
    }
    
    private User GetBy(Func<User, bool> predicate, PageData pageData)
    {
        List<User> listOfuser = userRepository.GetByFilter(predicate, pageData); 
        User user = listOfuser.FirstOrDefault();
        
        if (user == null)
        {
            throw new ElementNotFound(UserDoesNotExistExceptionMessage);
        }

        return user; 
    }
    
    private void VerifyRoleIsAssignable(string role)
    {
        if (!_assignableRoles.Contains(role.ToLower()))
        {
            throw new CannotAddItem(RoleNotAssignableExceptionMessage);
        }
    }
    
    private static void VerifyUserDoesNotHaveRoleAlready(User user, string role)
    {
        if (user.Roles.Exists(r => r.Kind.ToLower() == role.ToLower()))
        {
            throw new ElementAlreadyExist(UserAlreadyHasRoleExceptionMessage);
        }
    }
    
    private static List<CompanyOwner> GetCompanyOwnerRoles(User user)
    {
        return user.Roles
            .Where(role => role is CompanyOwner)
            .Cast<CompanyOwner>()
            .ToList();
    }

    private static CompanyOwner SearchAnIncompleteCompanyOwnerRole(List<CompanyOwner> companyOwnerRoles)
    {
        CompanyOwner incompleteRole = companyOwnerRoles
            .Find(role => !role.HasACompleteCompany);  

        return incompleteRole;
    }
}