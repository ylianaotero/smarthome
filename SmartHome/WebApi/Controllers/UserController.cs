using CustomExceptions;
using Domain;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;

namespace WebApi.Controllers;

[Route("api/v1/users")]
[ApiController]
public class UserController : ControllerBase
{
    private const string ErrorMessageUnexpectedException =  "An unexpected error occurred. Please try again later.";
    private const string ErrorMessageUnauthorizedAccess =  "You do not have permission to access this resource.";
    
    private readonly IUserService _userService;
    private readonly ISessionService _sessionService;

    public UserController(IUserService userService, ISessionService sessionService)
    {
        _userService = userService;
        _sessionService = sessionService; 
    }
    
    [HttpGet]
    public IActionResult GetUsers([FromHeader] Guid Authorization, [FromQuery] PageDataRequest pageDataRequest)
    {
        try
        {
            User userInSession = _sessionService.GetUser(Authorization);
            if (_userService.IsAdmin(userInSession.Email))
            {
                return Ok(_userService
                        .GetAllUsers(pageDataRequest.ToPageData())
                        .Select(u => new UserResponse(u))
                        .ToList());
            }

            return StatusCode(403, ErrorMessageUnauthorizedAccess);
        }
        catch (CannotFindItemInList cannotFindItemInList)
        {
            return Unauthorized(new { message = cannotFindItemInList.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = ErrorMessageUnexpectedException});
        }
        
    }
    
}