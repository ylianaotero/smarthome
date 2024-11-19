using CustomExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;
using WebApi.Attributes;

namespace WebApi.Controllers;

[Route("api/v1/company-owners")]
[ApiController]
[RolesWithPermissions(RoleWithPermissions)]
public class CompanyOwnerController : ControllerBase
{
    private readonly IUserService _userService;
    
    public CompanyOwnerController(IUserService userService)
    {
        this._userService = userService;
    }
    
    private const string RoleWithPermissions = "Administrator";
    
    [HttpPost]
    public IActionResult CreateCompanyOwner([FromBody] PostCompanyOwnerRequest postCompanyOwnerRequest)
    {
        try
        {
            _userService.CreateUser(postCompanyOwnerRequest.ToEntity());
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