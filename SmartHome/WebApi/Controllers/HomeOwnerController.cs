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
public class HomeOwnerController(IUserService userService) : ControllerBase
{
    private const string ErrorMessageUnexpectedException =  "An unexpected error occurred. Please try again later.";
    private const int StatusCodeInternalServerError = 500; 
    private const string RoleWithPermissions = "HomeOwner";

    [HttpPost]
    [AllowAnonymous]
    public IActionResult CreateHomeOwner([FromBody] PostHomeOwnerRequest postHomeOwnerRequest)
    {
        try
        {
            userService.CreateUser(postHomeOwnerRequest.ToEntity());
            PostHomeOwnerResponse response = new PostHomeOwnerResponse(postHomeOwnerRequest.ToEntity());
            
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
    }
    
     [HttpPut]
     [Route("{id}")]
     [RolesWithPermissions(RoleWithPermissions)]
     public IActionResult UpdateHomeOwner([FromRoute] long id, [FromBody] PutHomeOwnerRequest putHomeOwnerRequest)
     { 
         try 
         {
               userService.UpdateUser(id, putHomeOwnerRequest.ToEntity());
               PostHomeOwnerResponse response = new PostHomeOwnerResponse(putHomeOwnerRequest.ToEntity());
               
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