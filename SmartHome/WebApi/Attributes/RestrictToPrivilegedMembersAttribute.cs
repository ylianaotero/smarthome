namespace WebApi.Attributes;

public class RestrictToPrivilegedMembersAttribute : Attribute
{
    public bool WithListPermissions { get; }
    public bool WithAddPermissions { get; }
    
    public RestrictToPrivilegedMembersAttribute(bool withListPermissions, bool withAddPermissions)
    {
        WithListPermissions = withListPermissions;
        WithAddPermissions = withAddPermissions;
    }
    
    public override string ToString() => "RestrictToPrivilegedMembers";
}