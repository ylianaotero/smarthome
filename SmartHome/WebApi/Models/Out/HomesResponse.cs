namespace WebApi.Models.Out;

public class HomesResponse
{ 
    public List<HomeResponse> Homes { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is HomesResponse response &&
               Homes.SequenceEqual(response.Homes);
    }
}