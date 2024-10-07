using CustomExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;
using WebApi.Attributes;

namespace WebApi.Controllers;

[Route("api/v1/company-owners")]
[ApiController]
public class CompanyOwnerController(IUserService userService) : ControllerBase
{
    private const string RoleWithPermissions = "Administrator";
    
    [HttpPost]
    [RolesWithPermissions(RoleWithPermissions)]
    public IActionResult CreateCompanyOwner([FromBody] PostCompanyOwnerRequest postCompanyOwnerRequest)
    {
        try
        {
            userService.CreateUser(postCompanyOwnerRequest.ToEntity());
            PostCompanyOwnerResponse response = new PostCompanyOwnerResponse(postCompanyOwnerRequest.ToEntity());
            
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