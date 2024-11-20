using CustomExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;
using WebApi.Attributes;

namespace WebApi.Controllers;

[Route("api/v1/administrators")]
[ApiController]
[RolesWithPermissions(RoleWithPermissions)]
public class AdministratorController : ControllerBase
{
    private readonly IUserService _userService;
    
    public AdministratorController(IUserService userService)
    {
        this._userService = userService;
    }
    
    private const string RoleWithPermissions = "Administrator";
    
    [HttpPost]
    public IActionResult CreateAdministrator([FromBody] PostAdministratorRequest postAdministratorRequest)
    {
        try
        {
            long id = _userService.CreateUser(postAdministratorRequest.ToEntity());
            PostAdministratorResponse userResponse = new PostAdministratorResponse(postAdministratorRequest.ToEntity(), id);

            
            return CreatedAtAction(nameof(CreateAdministrator), userResponse);
        }
        catch (InputNotValid inputNotValid)
        {
            return BadRequest(new { message = inputNotValid.Message });
        }
        catch (ElementAlreadyExist elementAlreadyExist)
        {
            return Conflict(new { message = elementAlreadyExist.Message });
        }
    }
    
    [HttpDelete]
    [Route("{id}")]
    public IActionResult DeleteAdministrator([FromRoute] long id)
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