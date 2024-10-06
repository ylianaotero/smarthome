using Domain.Concrete;

namespace Model.Out;

public class UsersResponse
{
    public List<UserResponse> Users { get; set; }
    
    public UsersResponse(List<User> users)
    {
        Users = users.Select(user => new UserResponse(user)).ToList();
    }
    
    public override bool Equals(object? obj)
    {
        return obj is UsersResponse response &&
                Users.SequenceEqual(response.Users);
    }
}