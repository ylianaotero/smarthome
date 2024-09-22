using System.ComponentModel.DataAnnotations;
using Domain.Exceptions.HomeExceptions;

namespace Domain;

public class HomeOwner : Role
{
    private const string MessageHomeNotFound = "Home not found"; 
    public List<Home> Homes { get; set; }

    public HomeOwner()
    {
        Homes = new List<Home>();
    }

    public void AddHome(Home home)
    {
        Homes.Add(home);
    }

    public void RemoveHome(Home home)
    {
        Homes.Remove(home);
    }

    public Home SearchHome(int homeId)
    {
        Home home = Homes.Find(h => h.Id == homeId);
        if (home != null)
        {
            return home;
        }
        throw new HomeNotFoundException(MessageHomeNotFound);
    }
}