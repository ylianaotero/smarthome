using Domain.Concrete;

namespace Model.In;

public class GetCompaniesRequest
{
    public string? Name { get; set; }
    public string? Owner { get; set; }
    
    public Func<Company, bool> ToFilter()
    {
        (string,string) splitName = SplitFullName();
        
        if (splitName.Item2 == "")
        {
            return FilterWithEmptyOrIncompleteFullName(splitName.Item1);
        }

        return FilterWithFullName(splitName.Item1, splitName.Item2);
    }
    
    private (string,string) SplitFullName()
    {
        string[] splitName;

        if (Owner != null)
        {
            splitName = Owner.Split(" ");
            
            if (splitName.Length == 0)
            {
                return ("","");
            }
            
            if (splitName.Length == 1)
            {
                return (splitName[0], "");
            }
            
            return (splitName[0], splitName[1]);
        }

        return ("", "");
    }
    
    private Func<Company,bool> FilterWithEmptyOrIncompleteFullName(string? name)
    {
        return company => (string.IsNullOrEmpty(name) || 
                        company.Owner.Name.ToLower().Contains(name.ToLower()) || 
                        company.Owner.Surname.Contains(name.ToLower())) &&
                          (string.IsNullOrEmpty(Name) || company.Name.ToLower().Contains(Name.ToLower()));
                          
    }
    
    private Func<Company,bool> FilterWithFullName(string name, string surname)
    {
        string lowerCaseName = name.ToLower();
        string lowerCaseSurname = surname.ToLower();

        return company => (company.Owner.Name.ToLower().Contains(lowerCaseName) &&
                        company.Owner.Surname.ToLower().Contains(lowerCaseSurname)) &&
                          (string.IsNullOrEmpty(Name) || company.Name.ToLower().Contains(Name.ToLower()));
    }
}