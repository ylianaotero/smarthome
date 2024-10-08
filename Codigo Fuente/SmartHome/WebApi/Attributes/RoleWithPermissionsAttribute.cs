namespace WebApi.Attributes;

public class RolesWithPermissionsAttribute(params string[] rolesWithPermissions) : Attribute
{
    public List<string> RolesWithPermissions { get; } = rolesWithPermissions.ToList();
}