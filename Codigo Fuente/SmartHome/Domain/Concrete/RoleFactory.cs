using CustomExceptions;
using Domain.Abstract;

namespace Domain.Concrete
{
    public abstract class RoleFactory
    {
        private const string InvalidRoleTypeExceptionMessage = "Invalid role type";
        private const string AdministratorRoleType = "administrator";
        private const string CompanyOwnerRoleType = "companyowner";
        private const string HomeOwnerRoleType = "homeowner";

        public static Role CreateRole(string roleType)
        {
            switch (roleType.ToLower())
            {
                case AdministratorRoleType:
                    return new Administrator();
                case CompanyOwnerRoleType:
                    return new CompanyOwner();
                case HomeOwnerRoleType:
                    return new HomeOwner();
                default:
                    throw new InputNotValid(InvalidRoleTypeExceptionMessage);
            }
        }
    }
}