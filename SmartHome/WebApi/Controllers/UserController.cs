using BusinessLogic.IServices;
using Microsoft.AspNetCore.Mvc;
using WebApi.Out;

namespace WebApi.Controllers;

[Route("api/v1/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ISessionService _sessionService;

    public UserController(IUserService userService/*, ISessionService sessionService*/)
    {
        _userService = userService;
        //_sessionService = sessionService; 
    }
    
    [HttpGet]
    public IActionResult GetUsers([FromHeader] string Authorization)
    {
        return Ok(_userService.GetAllUsers().Select(u => new UserResponse(u)).ToList());

    }
    
}