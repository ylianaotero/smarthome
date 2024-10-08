namespace WebApi.Attributes;

public class RestrictToPrivilegedMembersAttribute(bool withListPermissions, bool withAddPermissions) : Attribute
{
    public bool WithListPermissions { get; } = withListPermissions;
    public bool WithAddPermissions { get; } = withAddPermissions;

    public override string ToString() => "RestrictToPrivilegedMembers";
}