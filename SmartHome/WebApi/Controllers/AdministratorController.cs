using CustomExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApi.Attributes;
using WebApi.Models.In;
using WebApi.Models.Out;

namespace WebApi.Controllers;

[Route("api/v1/administrators")]
public class AdministratorController : ControllerBase
{
    private const string RoleWithPermissions = "Administrator";

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
    
    [HttpDelete]
    [RolesWithPermissions(RoleWithPermissions)]
    [Route("{id}")]
    public IActionResult DeleteUser([FromRoute] long id)
    {
        try
        {
            _userService.DeleteUser(id);
        }
        catch (ElementNotFound)
        {
            return NotFound();
        }

        return Ok();
    }
}