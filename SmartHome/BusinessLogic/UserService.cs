using CustomExceptions;
using Domain;
using Domain.Concrete;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class UserService(IRepository<User> userRepository) : IUserService
{
    private const string UserDoesNotExistExceptionMessage = "Member not found";
    private const string UserAlreadyExistExceptionMessage = "Member already exists";

    private IUserService _userServiceImplementation;

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

    public List<User> GetAllUsers(PageData pageData)
    {
        return userRepository.GetAll(pageData);
    }

    public List<User> GetUsersByFilter(Func<User, bool> filter, PageData pageData)
    {
        return userRepository.GetByFilter(filter, pageData);
    }

    public bool IsAdmin(string email)
    {
        User existingUser =  GetBy(u => u.Email == email, PageData.Default);
        bool hasAdministrator = existingUser.Roles.Any(role => role is Administrator);
        
        return hasAdministrator; 
    }

    private User GetUserById(long Id)
    {
        User user = userRepository.GetById(Id); 
        if (user == null)
        {
            throw new ElementNotFound(UserDoesNotExistExceptionMessage);
        }

        return user; 
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
    
    private List<CompanyOwner> GetCompanyOwnerRoles(User user)
    {
        return user.Roles
            .Where(role => role is CompanyOwner)
            .Cast<CompanyOwner>()
            .ToList();
    }

    private CompanyOwner SearchAnIncompleteCompanyOwnerRole(List<CompanyOwner> companyOwnerRoles)
    {
        CompanyOwner incompleteRole = companyOwnerRoles
            .FirstOrDefault(role => role.HasACompleteCompany == false);  

        return incompleteRole;
    }
}