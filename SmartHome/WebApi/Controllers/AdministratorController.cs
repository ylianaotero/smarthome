using BusinessLogic.Exceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.In;
using WebApi.Models.Out;

namespace WebApi.Controllers;

[Route("api/v1/administrators")]
public class AdministratorController : ControllerBase
{
    private const string ErrorMessageUnexpectedException =  "An unexpected error occurred. Please try again later.";
    
    private readonly IUserService _userService;

    public AdministratorController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost]
    public IActionResult CreateUser([FromBody] CreateAdminRequest createAdminRequest)
    {
        try
        {
            _userService.CreateUser(createAdminRequest.ToEntity());
            var userResponse = new AdminResponse(createAdminRequest.ToEntity());
            return CreatedAtAction(nameof(CreateUser), userResponse);
        }
        catch (ElementAlreadyExist elementAlreadyExist)
        {
            return Conflict(new { message = elementAlreadyExist.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message =  ErrorMessageUnexpectedException});
        }
    }
}