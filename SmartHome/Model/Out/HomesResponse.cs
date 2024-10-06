using Domain;
using Domain.Concrete;

namespace Model.Out;

public class HomesResponse
{ 
    public List<HomeResponse> Homes { get; set; }

    public HomesResponse(List<Home> homes)
    {
        Homes = homes.Select(home => new HomeResponse(home)).ToList();
    }
    
    public override bool Equals(object? obj)
    {
        return obj is HomesResponse response &&
               Homes.SequenceEqual(response.Homes);
    }
}