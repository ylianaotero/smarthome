using CustomExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;
using WebApi.Attributes;

namespace WebApi.Controllers;

[Route("api/v1/users")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    private const string NotFoundMessage = "The requested resource was not found.";
    private const string RoleWithPermissions = "Administrator";

    [HttpGet]
    [RolesWithPermissions(RoleWithPermissions)]
    public IActionResult GetUsers([FromQuery] GetUsersRequest request, [FromQuery] PageDataRequest pageDataRequest)
    {
        GetUsersResponse getUsersResponse;

        try
        {
            getUsersResponse = new GetUsersResponse
                (userService.GetUsersByFilter(request.ToFilter(), pageDataRequest.ToPageData()));
        }
        catch (CannotFindItemInList)
        {
            return NotFound(NotFoundMessage);
        }
        
        return Ok(getUsersResponse);
    }
    
    [HttpPost]
    [RolesWithPermissions(RoleWithPermissions)]
    public IActionResult CreateCompanyOwner([FromBody] PostCompanyOwnerRequest postComapnyOwnerRequest)
    {
        try
        {
            userService.CreateUser(postComapnyOwnerRequest.ToEntity());
            PostCompanyOwnerResponse response = new PostCompanyOwnerResponse(postComapnyOwnerRequest.ToEntity());
            
            return CreatedAtAction(nameof(CreateCompanyOwner), response);
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
}