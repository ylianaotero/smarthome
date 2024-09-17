using BusinessLogic.IServices;
using Microsoft.AspNetCore.Mvc;
using WebApi.Out;

namespace WebApi.Controllers;

[Route("api/v1/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    public IActionResult GetUsers([FromHeader] string Authorization)
    {
        return Ok(_userService.GetAllUsers().Select(u => new UserResponse(u)).ToList());
    }
    
}