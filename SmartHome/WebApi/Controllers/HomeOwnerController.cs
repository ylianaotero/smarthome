using BusinessLogic.Exceptions;
using Domain.Exceptions.GeneralExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.In;
using WebApi.Out;

namespace WebApi.Controllers;

[Route("api/v1/home-owners")]
[ApiController]
public class HomeOwnerController : ControllerBase
{
    private const string ErrorMessageUnexpectedException =  "An unexpected error occurred. Please try again later.";
    
    private readonly IUserService _userService;

    public HomeOwnerController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost]
    public IActionResult CreateHomeOwner([FromBody] CreateHomeOwnerRequest createHomeOwnerRequest)
    {
        try
        {
            var user = createHomeOwnerRequest.ToEntity();
            _userService.CreateUser(user);
            var userResponse = new HomeOwnerResponse(user);
            return CreatedAtAction(nameof(CreateHomeOwner), userResponse);
        }
        catch (InputNotValid inputNotValid)
        {
            return BadRequest(new { message = inputNotValid.Message });
        }
        catch (ElementAlreadyExist elementAlreadyExist)
        {
            return Conflict(new { message = elementAlreadyExist.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = ErrorMessageUnexpectedException });
        }
        
    }
    
}