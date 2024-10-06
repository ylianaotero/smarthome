using CustomExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;
using WebApi.Attributes;

namespace WebApi.Controllers;

[Route("api/v1/users")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    private const string NotFoundMessage = "The requested resource was not found.";
    private const string RoleWithPermissions = "Administrator";

    [HttpGet]
    [RolesWithPermissions(RoleWithPermissions)]
    public IActionResult GetUsers([FromQuery] UsersRequest request, [FromQuery] PageDataRequest pageDataRequest)
    {
        UsersResponse usersResponse;

        try
        {
            usersResponse = new UsersResponse
                (userService.GetUsersByFilter(request.ToFilter(), pageDataRequest.ToPageData()));
        }
        catch (CannotFindItemInList)
        {
            return NotFound(NotFoundMessage);
        }
        
        return Ok(usersResponse);
    }
}