using BusinessLogic.IServices;
using Domain;
using Microsoft.AspNetCore.Mvc;
using WebApi.Out;

namespace WebApi.Controllers;

[Route("api/v1/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ISessionService _sessionService;

    public UserController(IUserService userService, ISessionService sessionService)
    {
        _userService = userService;
        _sessionService = sessionService; 
    }
    
    [HttpGet]
    public IActionResult GetUsers([FromHeader] Guid Authorization)
    {
        User userInSession = _sessionService.GetUser(Authorization);
        if (_userService.IsAdmin(userInSession.Email))
        {
            return Ok(_userService.GetAllUsers().Select(u => new UserResponse(u)).ToList());
        }

        return StatusCode(403, "Unauthorized");
    }
    
}