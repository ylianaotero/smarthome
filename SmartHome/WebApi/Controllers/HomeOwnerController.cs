using CustomExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;
using WebApi.Attributes;

namespace WebApi.Controllers;

[Route("api/v1/home-owners")]
[ApiController]
[AllowAnonymous]
public class HomeOwnerController : ControllerBase
{
    private const string ErrorMessageUnexpectedException =  "An unexpected error occurred. Please try again later.";
    
    private readonly IUserService _userService;
    
    private const string RoleWithPermissions = "Administrator";

    private const int StatusCodeInternalServerError = 500; 

    public HomeOwnerController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost]
    public IActionResult CreateHomeOwner([FromBody] CreateHomeOwnerRequest createHomeOwnerRequest)
    {
        try
        {
            _userService.CreateUser(createHomeOwnerRequest.ToEntity());
            HomeOwnerResponse response = new HomeOwnerResponse(createHomeOwnerRequest.ToEntity());
            
            return CreatedAtAction(nameof(CreateHomeOwner), response);
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
            return StatusCode(StatusCodeInternalServerError, new { message = ErrorMessageUnexpectedException });
        }
    }
    
     [HttpPut]
       [Route("{id}")]
       public IActionResult UpdateHomeOwner([FromRoute] long id, [FromBody] UpdateHomeOwnerRequest updateHomeOwnerRequest)
       {
           try
           {
               _userService.UpdateUser(id, updateHomeOwnerRequest.ToEntity());
               HomeOwnerResponse response = new HomeOwnerResponse(updateHomeOwnerRequest.ToEntity());
               
               return Ok(response);
           }
           catch (InputNotValid inputNotValid)
           {
               return BadRequest(new { message = inputNotValid.Message });
           }
           catch (ElementNotFound elementNotFound)
           {
               return NotFound(new { message = elementNotFound.Message });
           }
       }
    
}