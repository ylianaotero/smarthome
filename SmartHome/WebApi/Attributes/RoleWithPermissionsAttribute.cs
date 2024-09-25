namespace WebApi.Attributes;

public class RolesWithPermissionsAttribute : Attribute
{
    public List<string> RolesWithPermissions { get; }

    public RolesWithPermissionsAttribute(params string[] rolesWithPermissions)
    {
        RolesWithPermissions = rolesWithPermissions.ToList();
    }
}