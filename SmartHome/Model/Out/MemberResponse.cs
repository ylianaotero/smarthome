using Domain;

namespace Model.Out;

public class MemberResponse
{
    public string Name { get; set; }
    public string Email { get; set; }
    public bool Permission { get; set; }

    public MemberResponse(Member member)
    {
        Name = member.Name;
        Email = member.Email;
        Permission = member.Permission;
    }

    public override bool Equals(object? obj)
    {
        return obj is MemberResponse response &&
               Name == response.Name &&
               Email == response.Email &&
               Permission == response.Permission;
    }
}