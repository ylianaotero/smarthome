using CustomExceptions;
using Domain.Abstract;

namespace Domain.Concrete;

public abstract class RoleFactory
{
    private const string InvalidRoleTypeExceptionMessage = "Invalid role type";
    public static Role CreateRole(string roleType)
    {
        switch (roleType.ToLower())
        {
            case "administrator":
                return new Administrator();
            case "companyowner":
                return new CompanyOwner();
            case "homeowner":
                return new HomeOwner();
            default:
                throw new InputNotValid(InvalidRoleTypeExceptionMessage);
        }
    }
}