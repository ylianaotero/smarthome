using Domain.Concrete;

namespace Model.In;

public class GetUsersRequest
{
    public string? FullName { get; set; }
    public string? Role { get; set; }

    private const string EmptyString = "";
    private const char SpaceChar = ' ';

    public Func<User, bool> ToFilter()
    {
        (string, string) splitName = SplitFullName();
        
        if (splitName.Item2 == EmptyString)
        {
            return FilterWithEmptyOrIncompleteFullName(splitName.Item1);
        }

        return FilterWithFullName(splitName.Item1, splitName.Item2);
    }
    
    private (string, string) SplitFullName()
    {
        string[] splitName;

        if (FullName != null)
        {
            splitName = FullName.Split(SpaceChar);
            
            if (splitName.Length == 0)
            {
                return (EmptyString, EmptyString);
            }
            
            if (splitName.Length == 1)
            {
                return (splitName[0], EmptyString);
            }
            
            return (splitName[0], splitName[1]);
        }

        return (EmptyString, EmptyString);
    }
    
    private Func<User, bool> FilterWithEmptyOrIncompleteFullName(string? name)
    {
        return user => (string.IsNullOrEmpty(name) || 
                        user.Name.ToLower().Contains(name.ToLower()) || 
                        user.Surname.ToLower().Contains(name.ToLower())) &&
                       (string.IsNullOrEmpty(Role) || 
                        user.Roles.Exists(role => role.GetType().Name.ToLower() == Role.ToLower()));
    }
    
    private Func<User, bool> FilterWithFullName(string name, string surname)
    {
        string lowerCaseName = name.ToLower();
        string lowerCaseSurname = surname.ToLower();
        
        return user => (user.Name.ToLower().Contains(lowerCaseName) && 
                        user.Surname.ToLower().Contains(lowerCaseSurname)) &&
                       (string.IsNullOrEmpty(Role) || 
                        user.Roles.Exists(role => role.GetType().Name.ToLower() == Role.ToLower()));
    }
}
