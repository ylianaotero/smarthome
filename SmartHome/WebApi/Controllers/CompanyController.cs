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
    
    private const string RoleWithPermissions = "CompanyOwner";
    private const string CreatedMessage = "The resource was created successfully.";

    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    /*
    [HttpGet]
    public IActionResult GetCompanies([FromQuery] CompanyRequest request, [FromQuery] PageDataRequest pageDataRequest)
    {
        CompaniesResponse companiesResponse = new CompaniesResponse
            (_companyService.GetCompaniesByFilter(request.ToFilter(), pageDataRequest.ToPageData()));
        
        return Ok(companiesResponse);
    }
    */
    
    [HttpPost]
    [Route("{id}/companies")]
    [RolesWithPermissions(RoleWithPermissions)]
    public IActionResult PostCompany([FromRoute] long id, [FromBody] CompanyRequest request)
    {
        _companyService.CreateCompany(request.ToEntity());
        return Created(CreatedMessage,"/companies/");
        
    }
}