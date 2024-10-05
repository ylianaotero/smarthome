using CustomExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;
using WebApi.Attributes;

namespace WebApi.Controllers;

[Route("api/v1/administrators")]
public class AdministratorController : ControllerBase
{
    private const string RoleWithPermissions = "Administrator";
    private const string ErrorMessageUnexpectedException =  "An unexpected error occurred. Please try again later.";
    private const int StatusCodeInternalServerError = 500; 
    
    private readonly IUserService _userService;

    public AdministratorController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost]
    [AllowAnonymous]
    [RolesWithPermissions(RoleWithPermissions)]
    public IActionResult CreateAdministrator([FromBody] CreateAdminRequest createAdminRequest)
    {
        try
        {
            _userService.CreateUser(createAdminRequest.ToEntity());
            AdminResponse userResponse = new AdminResponse(createAdminRequest.ToEntity());
            
            return CreatedAtAction(nameof(CreateAdministrator), userResponse);
        }
        catch (ElementAlreadyExist elementAlreadyExist)
        {
            return Conflict(new { message = elementAlreadyExist.Message });
        }
        catch (Exception)
        {
            return StatusCode(StatusCodeInternalServerError, new { message =  ErrorMessageUnexpectedException});
        }
    }
}