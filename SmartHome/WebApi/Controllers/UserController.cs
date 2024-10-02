using CustomExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;
using WebApi.Attributes;

namespace WebApi.Controllers;

[Route("api/v1/users")]
[ApiController]
public class UserController : ControllerBase
{
    private const string NotFoundMessage = "The requested resource was not found.";
    private const string RoleWithPermissions = "Administrator";
    
    private readonly IUserService _userService;
    private readonly ISessionService _sessionService;

    public UserController(IUserService userService, ISessionService sessionService)
    {
        _userService = userService;
        _sessionService = sessionService; 
    }
    
    [HttpGet]
    [RolesWithPermissions(RoleWithPermissions)]
    public IActionResult GetUsers([FromQuery] UsersRequest request, [FromQuery] PageDataRequest pageDataRequest)
    {
        UsersResponse usersResponse;

        try
        {
            usersResponse = new UsersResponse
                (_userService.GetUsersByFilter(request.ToFilter(), pageDataRequest.ToPageData()));
        }
        catch (CannotFindItemInList)
        {
            return NotFound(NotFoundMessage);
        }
        
        return Ok(usersResponse);
    }
}