using BusinessLogic.IServices;
using Microsoft.AspNetCore.Mvc;
using WebApi.In;
using WebApi.Out;

namespace WebApi.Controllers;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost]
    public IActionResult CreateUser([FromBody] CreateUserRequest createUserRequest)
    {
        var user = createUserRequest.ToEntity();
        _userService.CreateUser(user);

        var userResponse = new UserResponse(user);
        return Ok(userResponse);
    }
    
}