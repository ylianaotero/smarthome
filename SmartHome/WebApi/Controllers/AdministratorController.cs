using BusinessLogic.Exceptions;
using Domain;
using Domain.Exceptions.GeneralExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.In;
using WebApi.Out;

namespace WebApi.Controllers;

[Route("api/v1/administrators")]
public class AdministratorController : ControllerBase
{
    private const string ErrorMessageUnauthorizedAccess =  "You do not have permission to access this resource.";
    private const string ErrorMessageUnexpectedException =  "An unexpected error occurred. Please try again later.";
    
    private readonly IUserService _userService;
    private readonly ISessionService _sessionService;

    public AdministratorController(IUserService userService, ISessionService sessionService)
    {
        _userService = userService;
        _sessionService = sessionService; 
    }
    
    [HttpPost]
    public IActionResult CreateUser([FromBody] CreateAdminRequest createAdminRequest, [FromHeader] Guid token)
    {
        try
        {
            User userInSession = _sessionService.GetUser(token);
            if (_userService.IsAdmin(userInSession.Email))
            {
                var user = createAdminRequest.ToEntity();
                _userService.CreateUser(user);
                var userResponse = new AdminResponse(user);
                return CreatedAtAction(nameof(CreateUser), userResponse);
            }
            
            return StatusCode(403, new { message =  ErrorMessageUnauthorizedAccess});
        }
        catch (CannotFindItemInList cannotFindItemInList)
        {
            return Unauthorized(new { message = cannotFindItemInList.Message });
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
            return StatusCode(500, new { message =  ErrorMessageUnexpectedException});
        }
    }
}