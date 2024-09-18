using BusinessLogic.IServices;
using Domain.Exceptions.GeneralExceptions;
using Microsoft.AspNetCore.Mvc;
using WebApi.In;
using WebApi.Out;

namespace WebApi.Controllers;

[Route("api/v1/home-owners")]
[ApiController]
public class HomeOwnerController : ControllerBase
{
    private readonly IUserService _userService;

    public HomeOwnerController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost]
    public IActionResult CreateUser([FromBody] CreateHomeOwnerRequest createHomeOwnerRequest)
    {
        try
        {
            var user = createHomeOwnerRequest.ToEntity();
            _userService.CreateUser(user);
            var userResponse = new HomeOwnerResponse(user);
            return Ok(userResponse);
        }
        catch (InputNotValid inputNotValid)
        {
            return BadRequest(new { message = inputNotValid.Message });
        }
        catch (ElementAlreadyExist elementAlreadyExist)
        {
            return Conflict(new { message = elementAlreadyExist.Message });
        }
        catch (Exception exception)
        {
            return StatusCode(500, new { message = "An unexpected error occurred. Please try again later." });
        }
        
    }
    
}