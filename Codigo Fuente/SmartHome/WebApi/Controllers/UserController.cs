using CustomExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;
using WebApi.Attributes;

namespace WebApi.Controllers;

[Route("api/v1/users")]
[ApiController]
[RolesWithPermissions(RoleWithPermissions)]
public class UserController(IUserService userService) : ControllerBase
{
    private const string NotFoundMessage = "The requested resource was not found.";
    private const string RoleWithPermissions = "Administrator";

    [HttpGet]
    public IActionResult GetUsers([FromQuery] GetUsersRequest request, [FromQuery] PageDataRequest pageDataRequest)
    {
        GetUsersResponse getUsersResponse;

        try
        {
            getUsersResponse = new GetUsersResponse
                (userService.GetUsersByFilter(request.ToFilter(), pageDataRequest.ToPageData()));
        }
        catch (CannotFindItemInList)
        {
            return NotFound(NotFoundMessage);
        }
        
        return Ok(getUsersResponse);
    }
    
    [HttpPost]
    [Route("{id}/roles")]
    public IActionResult PostUserRole([FromRoute] long id, [FromBody] PostUserRoleRequest request)
    {
        try
        {
            userService.AssignRoleToUser(id, request.Role);
        
            return Ok("Ok");
        }
        catch (ElementNotFound e)
        {
            return NotFound(e.Message);
        }
        catch (InputNotValid e)
        {
            return BadRequest(e.Message);
        }
        catch (CannotAddItem e)
        {
            return StatusCode(StatusCodes.Status412PreconditionFailed, e.Message);
        }
    }
}