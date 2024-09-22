using BusinessLogic.IServices;
using Domain.Exceptions.GeneralExceptions;
using Microsoft.AspNetCore.Mvc;
using WebApi.In;
using WebApi.Out;

namespace WebApi.Controllers;

[Route("api/v1/administrators")]
public class AdministratorController : ControllerBase
{
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
            var user = createAdminRequest.ToEntity();
            _userService.CreateUser(user);
            var userResponse = new AdminResponse(user);
            return CreatedAtAction(nameof(CreateUser), userResponse);
        }
        /*catch ()
        {
            
        }*/
        catch (InputNotValid inputNotValid)
        {
            return BadRequest(new { message = inputNotValid.Message });
        }
       
        
    }
}