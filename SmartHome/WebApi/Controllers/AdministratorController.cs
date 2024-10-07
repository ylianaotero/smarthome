using CustomExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;
using WebApi.Attributes;

namespace WebApi.Controllers;

[Route("api/v1/administrators")]
public class AdministratorController(IUserService userService) : ControllerBase
{
    private const string RoleWithPermissions = "Administrator";
    private const string ErrorMessageUnexpectedException =  "An unexpected error occurred. Please try again later.";
    private const int StatusCodeInternalServerError = 500;

    [HttpPost]
    [RolesWithPermissions(RoleWithPermissions)]
    public IActionResult CreateAdministrator([FromBody] PostAdministratorRequest postAdministratorRequest)
    {
        try
        {
            userService.CreateUser(postAdministratorRequest.ToEntity());
            PostAdministratorResponse userResponse = new PostAdministratorResponse(postAdministratorRequest.ToEntity());
            
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
    [RolesWithPermissions(RoleWithPermissions)]
    [Route("{id}")]
    public IActionResult DeleteAdministrator([FromRoute] long id)
    {
        try
        {
            userService.DeleteUser(id);
        }
        catch (ElementNotFound)
        {
            return NotFound();
        }

        return Ok();
    }
}