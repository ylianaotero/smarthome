using CustomExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;
using WebApi.Attributes;

namespace WebApi.Controllers;

[Route("api/v1/companies")]
[ApiController]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;
    private readonly IUserService _userService; 
    
    private const string RoleWithPermissionsToGetCompanies = "Administrator";
    private const string RoleWithPermissionsToPostCompany = "CompanyOwner";
    private const string CompanyOwnerNotFound = "CompanyOwner was not found.";

    public CompanyController(ICompanyService companyService,IUserService userService)
    {
        _companyService = companyService;
        _userService = userService; 
    }
    
    [HttpGet]
    [RolesWithPermissions(RoleWithPermissionsToGetCompanies)]
    public IActionResult GetCompanies([FromQuery] CompaniesRequest request, [FromQuery] PageDataRequest pageDataRequest)
    {
        CompaniesResponse companiesResponse = new CompaniesResponse
            (_companyService.GetCompaniesByFilter(request.ToFilter(), pageDataRequest.ToPageData()));
        
        return Ok(companiesResponse);
    }
    
    
    [HttpPost]
    [RolesWithPermissions(RoleWithPermissionsToPostCompany)]
    public IActionResult PostCompany([FromBody] CompanyRequest request)
    {
        try
        {
            _companyService.CreateCompany(_userService.AddOwnerToCompany(request.OwnerId,request.ToEntity()));
            
            return CreatedAtAction(nameof(PostCompany), request);
        }
        catch (ElementNotFound)
        {
            return NotFound(CompanyOwnerNotFound);
        }
    }
}