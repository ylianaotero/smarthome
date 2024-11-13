using Domain.Concrete;

namespace Model.In;

public class GetCompaniesRequest
{
    public string? Name { get; set; }
    public string? Owner { get; set; }
    public string? OwnerEmail { get; set; }
    
    private const string EmptyString = "";
    private const char SpaceChar = ' ';

    public Func<Company, bool> ToFilter()
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

        if (Owner != null)
        {
            splitName = Owner.Split(SpaceChar);
            
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
    
    private Func<Company, bool> FilterWithEmptyOrIncompleteFullName(string? name)
    {
        return company => (string.IsNullOrEmpty(name) || 
                        company.Owner.Name.ToLower().Contains(name.ToLower()) || 
                        company.Owner.Surname.ToLower().Contains(name.ToLower())) &&
                          (string.IsNullOrEmpty(Name) || company.Name.ToLower().Contains(Name.ToLower())) && 
                          (string.IsNullOrEmpty(OwnerEmail) || company.Owner.Email.ToLower().Equals(OwnerEmail.ToLower()));
                          
    }
    
    private Func<Company, bool> FilterWithFullName(string name, string surname)
    {
        string lowerCaseName = name.ToLower();
        string lowerCaseSurname = surname.ToLower();

        return company => (company.Owner.Name.ToLower().Contains(lowerCaseName) &&
                        company.Owner.Surname.ToLower().Contains(lowerCaseSurname)) &&
                          (string.IsNullOrEmpty(Name) || company.Name.ToLower().Contains(Name.ToLower())) && 
                          (string.IsNullOrEmpty(OwnerEmail) || company.Owner.Email.ToLower().Equals(OwnerEmail.ToLower()));
    }
}
