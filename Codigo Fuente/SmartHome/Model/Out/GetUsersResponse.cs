using Domain.Concrete;

namespace Model.Out;

public class GetUsersResponse
{
    public List<GetUserResponse> Users { get; set; }
    
    public int TotalCount { get; set; }
    
    public GetUsersResponse(List<User> users, int number)
    {
        Users = users.Select(user => new GetUserResponse(user)).ToList();
        TotalCount = number; 
    }
    
    public override bool Equals(object? obj)
    {
        return obj is GetUsersResponse response &&
                Users.SequenceEqual(response.Users);
    }
}