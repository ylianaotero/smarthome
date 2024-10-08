using CustomExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;
using WebApi.Attributes;

namespace WebApi.Controllers;

[Route("api/v1/companies")]
[ApiController]
public class CompanyController(ICompanyService companyService, IUserService userService) : ControllerBase
{
    private const string RoleWithPermissionsToGetCompanies = "Administrator";
    private const string RoleWithPermissionsToPostCompany = "CompanyOwner";
    private const string CompanyOwnerNotFound = "CompanyOwner was not found.";

    [HttpGet]
    [RolesWithPermissions(RoleWithPermissionsToGetCompanies)]
    public IActionResult GetCompanies([FromQuery] GetCompaniesRequest request, [FromQuery] PageDataRequest pageDataRequest)
    {
        GetCompaniesResponse getCompaniesResponse = new GetCompaniesResponse
            (companyService.GetCompaniesByFilter(request.ToFilter(), pageDataRequest.ToPageData()));
        
        return Ok(getCompaniesResponse);
    }
    
    [HttpPost]
    [RolesWithPermissions(RoleWithPermissionsToPostCompany)]
    public IActionResult PostCompany([FromBody] PostCompanyRequest request)
    {
        try
        {
            companyService.CreateCompany(userService.AddOwnerToCompany(request.OwnerId,request.ToEntity()));
            
            return CreatedAtAction(nameof(PostCompany), request);
        }
        catch (ElementNotFound)
        {
            return NotFound(CompanyOwnerNotFound);
        }
    }
}