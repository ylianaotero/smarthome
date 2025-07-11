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
    
    public CompanyController(ICompanyService companyService, IUserService userService)
    {
        this._companyService = companyService;
        this._userService = userService;
    }
    
    private const string RoleWithPermissionsToGetCompanies = "Administrator";
    private const string RoleWithPermissionsToPostCompany = "CompanyOwner";
    private const string CompanyOwnerNotFound = "CompanyOwner was not found.";
    private const string UpdatedCompanyMessage = "The company was updated successfully.";
    private const string ResourceNotFoundMessage = "The requested resource was not found.";

    [HttpGet]
    [RolesWithPermissions(RoleWithPermissionsToPostCompany, RoleWithPermissionsToGetCompanies)]
    public IActionResult GetCompanies([FromQuery] GetCompaniesRequest request, [FromQuery] PageDataRequest pageDataRequest)
    {
        int count = _companyService.GetCompaniesByFilter(request.ToFilter(), null).Count; 
        GetCompaniesResponse getCompaniesResponse = new GetCompaniesResponse
            (_companyService.GetCompaniesByFilter(request.ToFilter(), pageDataRequest.ToPageData()), count);
        
        return Ok(getCompaniesResponse);
    }
    
    [HttpPost]
    [RolesWithPermissions(RoleWithPermissionsToPostCompany)]
    public IActionResult PostCompany([FromBody] PostCompanyRequest request)
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
    
    [HttpPatch]
    [Route("{id}")]
    [RolesWithPermissions(RoleWithPermissionsToPostCompany)]
    public IActionResult UpdateValidationMethod([FromRoute] long id, [FromBody] PatchCompanyRequest request)
    {
        try
        {
            _companyService.UpdateValidationMethod(id, request.ValidationMethod);
            return Ok(UpdatedCompanyMessage);
        }
        catch (ElementNotFound)
        {
            return NotFound(ResourceNotFoundMessage);
        }
    }
}