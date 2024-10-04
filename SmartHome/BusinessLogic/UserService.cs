using CustomExceptions;
using Domain;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public class UserService : IUserService
{
    private const string UserDoesNotExistExceptionMessage = "User not found";
    private const string UserAlreadyExistExceptionMessage = "User already exists";
    
    private IRepository<User> _userRepository;
    private IUserService _userServiceImplementation;

    public UserService(IRepository<User> userRepository)
    {
        _userRepository = userRepository; 
    }
    public void CreateUser(User user)
    {
        try
        {
            GetBy(u => u.Email == user.Email, PageData.Default);
            
            throw new ElementAlreadyExist(UserAlreadyExistExceptionMessage);
        }
        catch (ElementNotFound)
        {
            _userRepository.Add(user);
        }
        
    }

    private User GetBy(Func<User, bool> predicate, PageData pageData)
    {
        List<User> listOfuser = _userRepository.GetByFilter(predicate, pageData); 
        User user = listOfuser.FirstOrDefault();
        
        if (user == null)
        {
            throw new ElementNotFound(UserDoesNotExistExceptionMessage);
        }

        return user; 
    }

    public List<User> GetAllUsers(PageData pageData)
    {
        return _userRepository.GetAll(pageData);
    }

    public List<User> GetUsersByFilter(Func<User, bool> filter, PageData pageData)
    {
        return _userRepository.GetByFilter(filter, pageData);
    }

    public bool IsAdmin(string email)
    {
        User existingUser =  GetBy(u => u.Email == email, PageData.Default);
        bool hasAdministrator = existingUser.Roles.Any(role => role is Administrator);
        return hasAdministrator; 
    }

    private User GetUserById(long Id)
    {
        User user = _userRepository.GetById(Id); 
        if (user == null)
        {
            throw new ElementNotFound(UserDoesNotExistExceptionMessage);
        }

        return user; 
    }
    
    public bool CompanyOwnerIsComplete(long id)
    {
        User existingUser =  GetUserById(id);
        
        List<CompanyOwner> companyOwnerRoles = existingUser.Roles
            .Where(role => role is CompanyOwner)
            .Cast<CompanyOwner>()
            .ToList();

        Role role = searchAnIncompleteCompanyOwnerRole(companyOwnerRoles);

        if (role == null)
        {
            return true; 
        }

        return false; 
    }

    private Role searchAnIncompleteCompanyOwnerRole(List<CompanyOwner> companyOwnerRoles)
    {
        Role incompleteRole = companyOwnerRoles
            .FirstOrDefault(role => role.HasACompleteCompany == false);  

        return incompleteRole;
    }
    
    public void DeleteUser(long id)
    {
        User user = GetBy(u => u.Id == id, PageData.Default);
        _userRepository.Delete(user);
    }
    
    public void UpdateUser(long id, User user)
    {
        try
        {
            User existingUser = GetBy(u => u.Id == id, PageData.Default);
            existingUser.Update(user);
            _userRepository.Update(existingUser);
        }
        catch (Exception e)
        {
            throw new ElementNotFound(UserDoesNotExistExceptionMessage);
        }
    }
    
}