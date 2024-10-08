using Domain.Concrete;

namespace Model.Out;

public class GetHomesResponse
{ 
    public List<GetHomeResponse> Homes { get; set; }

    public GetHomesResponse(List<Home> homes)
    {
        Homes = homes.Select(home => new GetHomeResponse(home)).ToList();
    }
    
    public override bool Equals(object? obj)
    {
        return obj is GetHomesResponse response &&
               Homes.SequenceEqual(response.Homes);
    }
}