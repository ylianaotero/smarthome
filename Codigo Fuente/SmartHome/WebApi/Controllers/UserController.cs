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
    private const string AdministratorRole = "Administrator";
    private const string HomeOwnerRole = "HomeOwner";

    [HttpGet]
    [RolesWithPermissions(AdministratorRole)]
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
    
    [HttpGet]
    [Route("{id}")]
    [RolesWithPermissions(AdministratorRole)]
    public IActionResult GetUser([FromRoute] long id)
    {
        GetUserResponse getUserResponse;
        
        try
        {
            getUserResponse = new GetUserResponse(userService.GetUserById(id));
        }
        catch (ElementNotFound e)
        {
            return NotFound(e.Message);
        }
        
        return Ok(getUserResponse);
    }
    
    [HttpPost]
    [Route("{id}/roles")]
    [RolesWithPermissions(AdministratorRole, HomeOwnerRole)]
    public IActionResult PostUserRole([FromRoute] long id, [FromBody] PostUserRoleRequest request)
    {
        GetUserResponse getUserResponse;

        try
        {
            getUserResponse = new GetUserResponse(userService.AssignRoleToUser(id, request.Role));
        }
        catch (ElementNotFound)
        {
            return NotFound(NotFoundMessage);
        }
        catch (InputNotValid e)
        {
            return BadRequest(e.Message);
        }
        catch (CannotAddItem e)
        {
            return StatusCode(StatusCodes.Status412PreconditionFailed, e.Message);
        }
        catch (ElementAlreadyExist e)
        {
            return Conflict(e.Message);
        }

        return Ok(getUserResponse);
    }
}