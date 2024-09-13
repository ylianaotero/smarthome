using IDomain;

namespace Domain;

public class HomeOwner
{
    public List<Home> Homes { get; set; }

    public HomeOwner()
    {
        Homes = new List<Home>();
    }
}