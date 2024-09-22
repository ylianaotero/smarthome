using Domain;

namespace TestWebApi.Controllers;

public class RoleTest : Role
{
    public RoleTest(long id)
    {
        Id = id;
    }
}